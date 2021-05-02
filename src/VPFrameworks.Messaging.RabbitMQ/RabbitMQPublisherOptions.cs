using System;
using System.Collections.Generic;
using System.Text;
using VPFrameworks.Messaging.Abstractions;

namespace InfrastrutureClients.Messaging.RabbitMQ
{
    /// <summary>
    /// Information that describes the message to be sent
    /// </summary>
    public class RabbitMQPublisherOptions : MessageOptions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="concurrentCalls"></param>
        /// <param name="timeToLive"></param>
        /// <param name="initialVisibilityDelay"></param>
        /// <param name="serverTimeout"></param>
        /// <param name="maximumExecutionTime"></param>
        /// <param name="userHeaders"></param>
        /// <param name="customUserAgent"></param>
        public RabbitMQPublisherOptions(string clientId, int concurrentCalls = 1, TimeSpan? timeToLive = null, TimeSpan? initialVisibilityDelay = null, TimeSpan? serverTimeout = null, TimeSpan? maximumExecutionTime = null, IDictionary<string, string> userHeaders = null, string customUserAgent = null)
            : base(clientId, concurrentCalls , timeToLive , initialVisibilityDelay , serverTimeout, maximumExecutionTime , userHeaders, customUserAgent)
        {

        }

        /// <summary>
        ///  Gets or sets the priority of the message
        /// </summary>
        public byte? Priority { get; set; }

        /// <summary>
        /// Gets or sets persistence levels 
        /// </summary>
        public MessagePersistence DeliveryMode { get; set; }

        /// <summary>
        /// message content type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Id message to correlate between messages to trace logical sequences of messages
        /// </summary>
        public Guid CorrelationId { get; internal set; }
        
        /// <summary>
        /// Type of enconding to the messages
        /// </summary>
        public string ContentEncoding { get; set; }
    }
}
