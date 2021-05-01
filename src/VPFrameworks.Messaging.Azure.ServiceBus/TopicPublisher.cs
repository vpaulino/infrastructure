using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VPFrameworks.Messaging.Abstractions;
using VPFrameworks.Messaging.Abstractions.Publisher;
using VPFrameworks.Messaging.Azure.ServiceBus.Topics;
using VPFrameworks.Serialization.Abstractions;

namespace VPFrameworks.Messaging.Azure.ServiceBus.Topics
{
    /// <summary>
    /// 
    /// </summary>
    public class TopicPublisher : IPublisher
    {
        private ITextSerializer textSerializer;
        private ConnectionSettings connectionSettings;
        private SerializationSettings serializationSettings;
        private TopicClient topicClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="textSerializer"></param>
        /// <param name="serializationSettings"></param>
        public TopicPublisher(ConnectionSettings connection, ITextSerializer textSerializer, SerializationSettings serializationSettings)
        {
            this.textSerializer = textSerializer;
            this.connectionSettings = connection;
            this.serializationSettings = serializationSettings;
            this.topicClient = new TopicClient(this.connectionSettings.ConnectionString, this.connectionSettings.Path);
        }

       /// <summary>
       /// 
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="payload"></param>
       /// <param name="serialization"></param>
       /// <returns></returns>
        protected virtual async Task<Message> CreateCloudMessage<T>(T payload, SerializationSettings serialization)
        {
            string payloadSerialized = await this.textSerializer.Serialize<T>(payload, serialization);
            var message = new Message(Encoding.UTF8.GetBytes(payloadSerialized));
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
            var message = await CreateCloudMessage(payload, this.serializationSettings);
            SetupMessage(message, options as TopicMessageOptions);
            
            await this.topicClient.SendAsync(message);
            
            return new PublishResult(message.MessageId, message.ExpiresAtUtc, DateTime.UtcNow, null);
        }

        private void SetupMessage(Message message, TopicMessageOptions options)
        {
            foreach (var item in options.UserHeaders)
            {
                message.UserProperties.Add(item.Key, item.Value);
            }

            message.TimeToLive = options.TimeToLive.Value;
            message.SessionId = options.SessionId;
            message.CorrelationId = options.CorrelationId;
            message.To = options.Destination;
            message.ReplyTo = options.ReplyTo;
            message.ScheduledEnqueueTimeUtc = options.ScheduledEnqueueTimeUtc;
            
        }
    }
}
