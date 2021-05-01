using System;
using System.Collections.Generic;
using System.Text;

namespace VPFrameworks.Messaging.Abstractions
{
    /// <summary>
    /// Represents all the options that are possible to configure in a broker operation relevant to messages process
    /// </summary>
    public class MessageOptions
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
        public MessageOptions(string clientId, int concurrentCalls = 1, TimeSpan? timeToLive = null, TimeSpan? initialVisibilityDelay = null, TimeSpan? serverTimeout = null, TimeSpan? maximumExecutionTime = null, IDictionary<string, string> userHeaders = null, string CustomUserAgent = null)
        {
            this.ConcurrentCalls = concurrentCalls;
            TimeToLive = timeToLive;
            this.ServerTimeout = serverTimeout;
            this.MaximumExecutionTime = maximumExecutionTime;
            this.CustomUserAgent = CustomUserAgent;
            this.ClientId = clientId;
            this.InitialVisibilityDelay = initialVisibilityDelay;
            this.UserHeaders = userHeaders ?? new Dictionary<string,string>();
        }

        /// <summary>
        /// Gets the ammount of time the message will exist on the brokers
        /// </summary>
        public TimeSpan? TimeToLive { get; }

        /// <summary>
        /// /// Gets the ammount of time the message will not be visible after publised
        /// </summary>
        public TimeSpan? InitialVisibilityDelay { get; }

        /// <summary>
        /// /// Gets the ammount of time the server can give timeout while sending messages
        /// </summary>
        public TimeSpan? ServerTimeout { get;  }

        /// <summary>
        /// Gets the maximum of time the publish action takes to be executed
        /// </summary>
        public TimeSpan? MaximumExecutionTime { get;  }

        /// <summary>
        /// Gets the identification of the publisher type
        /// </summary>
        public string CustomUserAgent { get; }

        /// <summary>
        /// Gets the client id 
        /// </summary>
        public string ClientId { get; }

        /// <summary>
        /// Gets all the application parameters headers that a client application can send
        /// </summary>
        public IDictionary<string, string> UserHeaders { get; }

        /// <summary>
        /// Gets the Number of messages that can be processed in simultaneous
        /// </summary>
        public int ConcurrentCalls { get; }
        
    }
}
