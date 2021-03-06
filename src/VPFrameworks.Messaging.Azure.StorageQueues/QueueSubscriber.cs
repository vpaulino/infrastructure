using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VPFrameworks.Messaging.Abstractions;
using VPFrameworks.Messaging.Abstractions.Subscriber;
using VPFrameworks.Serialization.Abstractions;

namespace VPFrameworks.Azure.Storage.Queues
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueueSubscriber<T> : ISubscriber<T>, IMessageReceivedCallback<T>
    {
        private ITextSerializer textSerializer;
        private ConnectionSettings subscriberSettings;
        private CloudQueueClient cloudClient;
        private CloudQueue cloudQueue;

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
            var storageAccount = CloudStorageAccount.Parse(subscriberSettings.ConnectionString);
            cloudClient = storageAccount.CreateCloudQueueClient();

            if (string.IsNullOrEmpty(subscriberSettings.Path)) 
            {
                throw new ArgumentNullException(nameof(subscriberSettings.Path));
            }

            cloudQueue = cloudClient.GetQueueReference(subscriberSettings.Path);
            
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
            await this.cloudQueue.DeleteMessageAsync(message.MessageId, message.PopReceipt);
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
            await this.cloudQueue.CreateIfNotExistsAsync();
            var groupedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, this.ctSource.Token);
            this.options = options;
            this.clientMessageHandler = messageHandler;
            SubscriptionHandler = Task.Run( async ()=> await GetMessages(groupedCancellationTokenSource.Token), groupedCancellationTokenSource.Token);

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
            var queueRequestOptions = new QueueRequestOptions() { ServerTimeout = options.ServerTimeout, MaximumExecutionTime = options.MaximumExecutionTime };
            var operationContext = new OperationContext() { ClientRequestID = options.ClientId, CustomUserAgent = options.CustomUserAgent, UserHeaders = options.UserHeaders };
            while (!token.IsCancellationRequested && await this.cloudQueue.ExistsAsync(queueRequestOptions, operationContext, token)) 
            {
                try
                {
                    var messagesReceived = await this.cloudQueue.GetMessagesAsync(options.ConcurrentCalls, options.InitialVisibilityDelay, queueRequestOptions, operationContext, token);

                    if (messagesReceived == null)
                        continue;

                    await ProcessMessages(messagesReceived, token);
                }
                catch (OperationCanceledException ex)
                {
                    this.logger.LogError($"Message Received from {this.subscriberSettings.Path} with subscription id {this.subscriptionId} thrown Exception: {ex}");
                    await this.cloudQueue.DeleteAsync();
                }
            }
        }

        private async Task ProcessMessages(IEnumerable<CloudQueueMessage> messagesReceived, CancellationToken token)
        {
            foreach (var messageReceived in messagesReceived)
            {
                string messageBody = messageReceived.AsString;
                T body = await this.textSerializer.Deserialize<T>(messageBody, serializationSettings);
                var handlingResult = await this.clientMessageHandler.HandleAsync(new MessageReceived<T>(messageReceived.Id, messageReceived.PopReceipt, null, body), token);
                if (handlingResult.Mode == HandleMode.Sync && handlingResult.Success)
                    await this.cloudQueue.DeleteMessageAsync(messageReceived);

            }
        }
    }
}
