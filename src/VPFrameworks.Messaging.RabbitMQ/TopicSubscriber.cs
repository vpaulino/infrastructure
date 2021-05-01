using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using VPFrameworks.Messaging.Abstractions;
using VPFrameworks.Messaging.Abstractions.Subscriber;
using VPFrameworks.Messaging.RabbitMQ;
using VPFrameworks.Messaging.RabbitMQ.Connections;
using VPFrameworks.Messaging.RabbitMQ.Serialization;

namespace RabbitMQFacade.Subscriber
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TopicSubscriber<T>: RabbitMQClient,  IBasicConsumer, ISubscriber<T>, IMessageReceivedCallback<T>
    {
        private IMessageReceivedHandler<T> messageHandler;
        private string queueName;
        
        /// <summary>
        /// Notification that the current consumer subscription was cancelled on the server
        /// </summary>
        public event EventHandler<ConsumerEventArgs> ConsumerCancelled;
        

        /// <summary>
        /// Gets the comunication channel
        /// </summary>
        public IModel Model => this.channel;

        /// <summary>
        /// Gets the identification of this consumer on the broker
        /// </summary>
        public string ConsumerTag { get; private set; }

        /// <summary>
        /// Creates instances of Topic Subscriber
        /// </summary>
        /// <param name="serverUri"></param>
        /// <param name="exchange"></param>
        /// <param name="queueName"></param>
        /// <param name="connectionPool"></param>
        /// <param name="serializerNegotiator"></param>
        /// <param name="loggerProvider"></param>
        public TopicSubscriber(string serverUri, string exchange, string queueName, IConnectionPool connectionPool, ISerializerNegotiator serializerNegotiator, ILoggerProvider loggerProvider)
             : base(serverUri, exchange, connectionPool, serializerNegotiator, loggerProvider)
        {

            this.queueName = queueName;

            this.logger = loggerProvider.CreateLogger("TopicPublisher");
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageHandler"></param>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<Subscription> SubscribeAsync(IMessageReceivedHandler<T> messageHandler, MessageOptions options, CancellationToken token)
        {
            TaskCompletionSource<Subscription> tcs = new TaskCompletionSource<Subscription>();
            RabbitMQSubscriberOptions rabbitOptions = options as RabbitMQSubscriberOptions;
            Subscription result = null;
            this.messageHandler = messageHandler;
            try
            {
                channelControl.Wait();
                this.ConsumerTag = this.channel.BasicConsume(queueName, false, this);
                result = new Subscription(this.ConsumerTag);
                channelControl.Release();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, this.channel);
                
            }
            finally
            {
                tcs.SetResult(result);
            }


            return tcs.Task;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        public  Task CancelSubscriptionAsync(Subscription subscription)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            bool result = true;
            

            if (!subscription.Id.ToString().Equals(this.ConsumerTag))
                return Task.CompletedTask;

            try
            {   
                this.channel.BasicCancel(this.ConsumerTag);
                tcs.SetResult(result);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
                throw;
            }

            return tcs.Task;
        }



        #region interface implementation
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="consumerTag"></param>
        public void HandleBasicCancel(string consumerTag)
        {
            this.logger.LogInformation($"TopicSubscriber.HandleBasicCancel - consumerTag: {consumerTag}");
            ConsumerCancelled?.Invoke(this, new ConsumerEventArgs(consumerTag));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consumerTag"></param>
        public void HandleBasicCancelOk(string consumerTag)
        {
            this.logger.LogInformation($"TopicSubscriber.HandleBasicCancelOk - consumerTag: {consumerTag}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consumerTag"></param>
        public void HandleBasicConsumeOk(string consumerTag)
        {
            this.logger.LogInformation($"TopicSubscriber.HandleBasicConsumeOk - consumerTag: {consumerTag}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consumerTag"></param>
        /// <param name="deliveryTag"></param>
        /// <param name="redelivered"></param>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="properties"></param>
        /// <param name="body"></param>
        public void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            this.logger.LogInformation($"TopicSubscriber.HandleBasicConsumeOk - {{ consumerTag: \"{consumerTag}\",deliveryTag : \"{deliveryTag}\",redelivered : \"{redelivered}\",exchange : \"{exchange}\",routingKey : \"{routingKey}\"   }} ");

            ISerializer serializer = this.SerializerNegotiator.Negotiate(properties);

            T  result = serializer.DeSerialize<T>(body, properties);

            var resultTask = this.messageHandler.HandleAsync(new MessageReceived<T>(properties.MessageId, deliveryTag.ToString(), properties.AppId, result));

            var handleResult = resultTask.GetAwaiter().GetResult();

            if (handleResult.Mode == HandleMode.Sync)
            {
                if (handleResult.Success)
                    channel.BasicAck(deliveryTag, false);
                else
                    channel.BasicNack(deliveryTag, false, true);

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="reason"></param>
        public void HandleModelShutdown(object model, ShutdownEventArgs reason)
        {
            this.logger.LogInformation($"TopicSubscriber.HandleBasicConsumeOk - ShutdownEventArgs.Cause: {reason.Cause}, ShutdownEventArgs.ReplyText: {reason.ReplyText}, ChannelNumber: {(model as IModel).ChannelNumber} IsClosed : {(model as IModel).IsClosed},  CloseReason : {(model as IModel).CloseReason}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task SuccessAckAsync(MessageReceived<T> message, CancellationToken token = default)
        {
            this.channel.BasicAck(ulong.Parse(message.PopReceipt), false);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task FailedAckAsync(MessageReceived<T> message, CancellationToken token = default)
        {
            this.channel.BasicNack(ulong.Parse(message.PopReceipt), false, false);
            return Task.CompletedTask;
        }


        #endregion
    }
}
