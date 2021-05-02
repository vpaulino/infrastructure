using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using VPFrameworks.Messaging.Abstractions;
using VPFrameworks.Messaging.Abstractions.Publisher;
using VPFrameworks.Messaging.RabbitMQ.Connections;
using VPFrameworks.Messaging.RabbitMQ.Serialization;

namespace InfrastrutureClients.Messaging.RabbitMQ
{
    /// <summary>
    /// Topic implementation to RAbbitMQ
    /// </summary>
    public class TopicPublisher : RabbitMQClient, IPublisher
    {
       /// <summary>
       /// Creates new instance
       /// </summary>
       /// <param name="serverUri"></param>
       /// <param name="exchange"></param>
       /// <param name="connectionPool"></param>
       /// <param name="serializerNegotiator"></param>
       /// <param name="loggerProvider"></param>
        public TopicPublisher(string serverUri, string exchange, IConnectionPool connectionPool, ISerializerNegotiator serializerNegotiator, ILoggerProvider loggerProvider) 
            : base(serverUri, exchange, connectionPool, serializerNegotiator, loggerProvider) 
        {
            
            this.logger = loggerProvider.CreateLogger("TopicPublisher");
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
        public Task<PublishResult> PublishAsync<T>(T payload, string path, MessageOptions options, CancellationToken token)
        {
            
            TaskCompletionSource<PublishResult> tcs = new TaskCompletionSource<PublishResult>();

            try
            {

                IBasicProperties properties = CreateBasicProperties<T>(options);
                
                ISerializer serializer = this.SerializerNegotiator.Negotiate(properties);

                if (serializer == null)
                {
                    throw new ArgumentException("Not found compatible contentType nor content encoding on message to publish");
                }

                var bytes = serializer.Serialize(payload);

                this.channel.BasicPublish(this.Exchange, path, properties, bytes);

                tcs.SetResult(new PublishResult(properties.MessageId.ToString(), new DateTimeOffset(DateTime.UtcNow, options.TimeToLive.Value), DateTime.UtcNow, null));
            }
            catch (Exception ex)
            {

                tcs.SetException(ex);
                tcs.SetCanceled();
            }


            return tcs.Task;
        }

        /// <summary>
        /// Creates all the properties to publish with the message
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <returns></returns>
        protected virtual IBasicProperties CreateBasicProperties<T>(MessageOptions options)
        {
            var rabbitOptions = options as RabbitMQPublisherOptions;
            
            IBasicProperties properties = this.channel.CreateBasicProperties();

            properties.Headers = properties.Headers ?? new Dictionary<string, object>();

            foreach (var userHeader in options.UserHeaders)
            {
                properties.Headers.Add(userHeader.Key, userHeader.Value);
            }

            properties.ContentType = rabbitOptions.ContentType;
            properties.ContentEncoding = rabbitOptions.ContentEncoding.ToString();
            properties.Expiration = rabbitOptions.TimeToLive.Value.TotalMilliseconds.ToString();
            properties.AppId = rabbitOptions.ClientId;
            properties.Priority = rabbitOptions.Priority.HasValue ? rabbitOptions.Priority.Value : properties.Priority;
            properties.Type = typeof(T).FullName;
            properties.Timestamp = new AmqpTimestamp((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
            properties.MessageId = Guid.NewGuid().ToString();
            properties.CorrelationId = rabbitOptions.CorrelationId.ToString();
           
            return properties;

        }
    }
}
