using System;
using System.Collections.Generic;
using System.Text;
using VPFrameworks.Messaging.Abstractions;

namespace VPFrameworks.Messaging.Azure.ServiceBus.Topics
{
    /// <summary>
    /// 
    /// </summary>
    public class TopicMessageOptions : MessageOptions
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
        /// <param name="CustomUserAgent"></param>
        public TopicMessageOptions(string clientId, int concurrentCalls = 1, TimeSpan? timeToLive = null, TimeSpan? initialVisibilityDelay = null, TimeSpan? serverTimeout = null, TimeSpan? maximumExecutionTime = null, IDictionary<string, string> userHeaders = null, string CustomUserAgent = null) 
            : base(clientId, concurrentCalls, timeToLive , initialVisibilityDelay , serverTimeout , maximumExecutionTime , userHeaders ,CustomUserAgent)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public string SessionId { get; internal set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string CorrelationId { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string Destination { get; internal set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string ReplyTo { get; internal set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime ScheduledEnqueueTimeUtc { get; internal set; }
    }
}
