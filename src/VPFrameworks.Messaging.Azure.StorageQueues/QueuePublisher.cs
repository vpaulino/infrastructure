using System;
using System.Threading;
using System.Threading.Tasks;
using InfrastrutureClients.Messaging.Abstractions.Publisher;
using InfrastrutureClients.Serialization.Abstractions;
using InfrastrutureClients.Messaging.Abstractions;
using Azure.Storage.Queues;
using Azure.Core;

namespace InfrastrutureClients.Storage.Queues.Azure
{
    /// <summary>
    /// Crreates an instance of QueuePublisher
    /// </summary>
    public class QueuePublisher : IPublisher
    {
        private ITextSerializer textSerializer;
        private ConnectionSettings publisherSettings;
        private QueueClient queueClient;
        private MessageOptions messageOptions;
        private SerializationSettings serializationSettings;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSerializer"></param>
        /// <param name="publisherSettings"></param>
        /// <param name="serializationSettings"></param>
        public QueuePublisher(ITextSerializer textSerializer, ConnectionSettings publisherSettings, SerializationSettings serializationSettings)
        {
            
            this.textSerializer = textSerializer;
            this.publisherSettings = publisherSettings;
            this.serializationSettings = serializationSettings;

            if (string.IsNullOrEmpty(publisherSettings.Path))
            {
                throw new ArgumentNullException(nameof(publisherSettings.Path));
            }

            queueClient = new(publisherSettings.ConnectionString, publisherSettings.Path);

        }

        ///
        public QueuePublisher(ITextSerializer textSerializer, MessageOptions messageOptions, ConnectionSettings publisherSettings, SerializationSettings serializationSettings) 
            : this(textSerializer, publisherSettings, serializationSettings)
        {

            QueueClientOptions options = new QueueClientOptions();
            options.Retry.Delay = new TimeSpan(0, 1, 0);
            options.Retry.MaxRetries = 3;
            options.Retry.Mode = RetryMode.Exponential;
            options.Diagnostics.ApplicationId = "useragent-id";
            options.Diagnostics.IsLoggingEnabled = true;
            options.Diagnostics.IsDistributedTracingEnabled = true;
            options.Diagnostics.IsLoggingContentEnabled = false;
            options.MessageEncoding = QueueMessageEncoding.Base64;
            
            //this.messageOptions = messageOptions;
            //queueRequestOptions = new QueueRequestOptions() { MaximumExecutionTime = messageOptions.MaximumExecutionTime, ServerTimeout = messageOptions.ServerTimeout };
            //operationContext = new OperationContext() { ClientRequestID = messageOptions.ClientId, CustomUserAgent = messageOptions.CustomUserAgent, UserHeaders = messageOptions.UserHeaders, LogLevel = logLevel };
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
            if (payload == null)
                throw new ArgumentNullException(nameof(payload));

            var message = await CreateCloudMessage(payload);
           
            await queueClient.CreateIfNotExistsAsync();

            var response = await queueClient.SendMessageAsync(message, options.InitialVisibilityDelay, options.TimeToLive, token);
                        
            return new PublishResult(response.Value.MessageId, response.Value.ExpirationTime, response.Value.InsertionTime, response.Value.PopReceipt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="payload"></param>
        /// <returns></returns>
        protected virtual async ValueTask<string> CreateCloudMessage<T>(T payload)
        {
            string payloadSerialized = await this.textSerializer.Serialize<T>(payload, serializationSettings);
            
            return payloadSerialized;
        }

        




    }
}
