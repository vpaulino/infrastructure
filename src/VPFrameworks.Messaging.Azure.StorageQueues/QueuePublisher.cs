using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using VPFrameworks.Messaging.Abstractions;
using VPFrameworks.Serialization.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;
using VPFrameworks.Messaging.Abstractions.Publisher;

namespace VPFrameworks.Azure.Storage.Queues
{
    /// <summary>
    /// Crreates an instance of QueuePublisher
    /// </summary>
    public class QueuePublisher : IPublisher
    {
        private ITextSerializer textSerializer;
        private ConnectionSettings publisherSettings;
        private LogLevel logLevel;
        private CloudQueueClient cloudClient;
        private CloudQueue cloudQueue;
        private MessageOptions messageOptions;
        private QueueRequestOptions queueRequestOptions;
        private OperationContext operationContext;
        private SerializationSettings serializationSettings;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSerializer"></param>
        /// <param name="publisherSettings"></param>
        /// <param name="serializationSettings"></param>
        /// <param name="logLevel"></param>
        public QueuePublisher(ITextSerializer textSerializer, ConnectionSettings publisherSettings, SerializationSettings serializationSettings, LogLevel logLevel = LogLevel.Error)
        {
            this.textSerializer = textSerializer;
            this.publisherSettings = publisherSettings;
            this.logLevel = logLevel;
            this.serializationSettings = serializationSettings;

            var storageAccount = CloudStorageAccount.Parse(publisherSettings.ConnectionString);
            cloudClient = storageAccount.CreateCloudQueueClient();

            if (string.IsNullOrEmpty(publisherSettings.Path))
            {
                throw new ArgumentNullException(nameof(publisherSettings.Path));
            }

            cloudQueue = cloudClient.GetQueueReference(publisherSettings.Path);
         

        }

        ///
        public QueuePublisher(ITextSerializer textSerializer, MessageOptions messageOptions, ConnectionSettings publisherSettings, SerializationSettings serializationSettings, LogLevel logLevel = LogLevel.Error) 
            : this(textSerializer, publisherSettings, serializationSettings, logLevel)
        {
           
            this.messageOptions = messageOptions;
            queueRequestOptions = new QueueRequestOptions() { MaximumExecutionTime = messageOptions.MaximumExecutionTime, ServerTimeout = messageOptions.ServerTimeout };
            operationContext = new OperationContext() { ClientRequestID = messageOptions.ClientId, CustomUserAgent = messageOptions.CustomUserAgent, UserHeaders = messageOptions.UserHeaders, LogLevel = logLevel };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="payload"></param>
        /// <param name="path"></param>
        /// <param name="options"></param>
        /// <param name="queueRequestOptions"></param>
        /// <param name="operationContext"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        protected virtual async Task<PublishResult> PublishAsync<T>(T payload, string path, MessageOptions options, QueueRequestOptions queueRequestOptions, OperationContext operationContext, CancellationToken token)
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(payload));

            var message = await CreateCloudMessage(payload);
           

            CloudQueue scopedQueue = this.cloudQueue;
            if (!string.IsNullOrEmpty(path))
            {
                scopedQueue = cloudClient.GetQueueReference(path);
            }

            await scopedQueue.CreateIfNotExistsAsync();

            await scopedQueue.AddMessageAsync(message, options.TimeToLive, options.InitialVisibilityDelay, queueRequestOptions, operationContext, token);
                        
            return new PublishResult(message.Id, message.ExpirationTime, message.InsertionTime, message.PopReceipt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="payload"></param>
        /// <returns></returns>
        protected virtual async Task<CloudQueueMessage> CreateCloudMessage<T>(T payload)
        {
            string payloadSerialized = await this.textSerializer.Serialize<T>(payload, serializationSettings);
            var message = new CloudQueueMessage(payloadSerialized);

            return message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="payload"></param>
        /// <param name="path"></param>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<PublishResult> PublishAsync<T>(T payload, string path, MessageOptions options, CancellationToken token)
        {

            var queueRequestOptions = new QueueRequestOptions() { MaximumExecutionTime = options.MaximumExecutionTime, ServerTimeout = options.ServerTimeout };
            var operationContext = new OperationContext() { ClientRequestID = options.ClientId, CustomUserAgent = options.CustomUserAgent, UserHeaders = options.UserHeaders, LogLevel = logLevel };

            return await PublishAsync(payload, path, options, queueRequestOptions, operationContext, token);

        }




    }
}
