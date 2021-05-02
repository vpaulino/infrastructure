using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using InfrastrutureClients.Messaging.Abstractions;
using InfrastrutureClients.Messaging.Abstractions.Subscriber;
using InfrastrutureClients.Serialization.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfrastrutureClients.Storage.Queues.Azure
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueueSubscriber<T> : ISubscriber<T>, IMessageReceivedCallback<T>
    {
        private ITextSerializer textSerializer;
        private ConnectionSettings subscriberSettings;
        private QueueClient queueClient;
        

        private IMessageReceivedHandler<T> clientMessageHandler;
        private Task SubscriptionHandler;
        private Guid subscriptionId;
        private ILogger logger;
        private CancellationTokenSource ctSource;
        private SerializationSettings serializationSettings;
        private MessageOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="textSerializer"></param>
        /// <param name="subscriberSettings"></param>
        /// <param name="serializationSettings"></param>
        public QueueSubscriber(ILogger logger, ITextSerializer textSerializer, ConnectionSettings subscriberSettings, SerializationSettings serializationSettings)
        {
            this.logger = logger;
            ctSource = new CancellationTokenSource();
            this.serializationSettings = serializationSettings;
            this.textSerializer = textSerializer;
            this.subscriberSettings = subscriberSettings;

            if (string.IsNullOrEmpty(subscriberSettings.Path))
            {
                throw new ArgumentNullException(nameof(subscriberSettings.Path));
            }


            queueClient = new(subscriberSettings.ConnectionString, subscriberSettings.Path);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        public virtual Task CancelSubscriptionAsync(Subscription subscription)
        {
            if (subscription.Id is Guid && this.subscriptionId != (Guid)subscription.Id) 
                return Task.CompletedTask; ;

            ctSource.Cancel();

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
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task SuccessAckAsync(MessageReceived<T> message, CancellationToken token = default)
        {
            await this.queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageHandler"></param>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual async Task<Subscription> SubscribeAsync(IMessageReceivedHandler<T> messageHandler, MessageOptions options, CancellationToken token)
        {
            await this.queueClient.CreateIfNotExistsAsync();
            var groupedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, this.ctSource.Token);
            this.options = options;
            this.clientMessageHandler = messageHandler;
            this.SubscriptionHandler = Task.Run( async ()=> await GetMessages(groupedCancellationTokenSource.Token), groupedCancellationTokenSource.Token);

            this.subscriptionId = Guid.NewGuid();
            
            this.logger.LogInformation($"Subscription created to Queue {this.subscriberSettings.Path} with success with id {this.subscriptionId}");

            return new Subscription(this.subscriptionId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected virtual async Task GetMessages(CancellationToken token)
        {
            while (!token.IsCancellationRequested && await this.queueClient.ExistsAsync(token)) 
            {
                try
                {
                    var queueResponse = await this.queueClient.ReceiveMessagesAsync(options.ConcurrentCalls, options.InitialVisibilityDelay, token);

                    if (queueResponse.Value == null)
                        continue;

                    await ProcessMessages(queueResponse.Value, token);
                }
                catch (OperationCanceledException ex)
                {
                    this.logger.LogError($"Message Received from {this.subscriberSettings.Path} with subscription id {this.subscriptionId} thrown Exception: {ex}");
                }
            }
        }

        private async Task ProcessMessages(IEnumerable<QueueMessage> messagesReceived, CancellationToken token)
        {
            foreach (var messageReceived in messagesReceived)
            {
                BinaryData messageBody = messageReceived.Body;
                T body = messageBody.ToObjectFromJson<T>();
                var handlingResult = await this.clientMessageHandler.HandleAsync(new MessageReceived<T>(messageReceived.MessageId, messageReceived.PopReceipt, null, body), token);
                if (handlingResult.Mode == HandleMode.Sync && handlingResult.Success)
                    await this.queueClient.DeleteMessageAsync(messageReceived.MessageId, messageReceived.PopReceipt);
            }
        }
    }
}
