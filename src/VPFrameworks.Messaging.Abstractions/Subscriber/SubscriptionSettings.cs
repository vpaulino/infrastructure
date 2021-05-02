using System;

namespace InfrastrutureClients.Messaging.Abstractions
{
    /// <summary>
    /// Represents some subscription settings
    /// </summary>
    public class SubscriptionSettings
    {
        /// <summary>
        /// Creates an instance of settings of the subscriptions
        /// </summary>
        public SubscriptionSettings(bool autoComplete, int maxConcurrentCalls, TimeSpan maxRenewDuration)
        {
            this.AutoComplete = autoComplete;
            this.MaxConcurrentCalls = MaxConcurrentCalls;
            this.MaxRenewDuration = maxRenewDuration;

        }

        /// <summary>
        /// Gets information if messages should be auto completed or not 
        /// </summary>
        public bool AutoComplete { get;   }

        /// <summary>
        /// Get the max concurrent number of fetching data that can happen
        /// </summary>
        public int MaxConcurrentCalls { get;  }

        /// <summary>
        /// Gets the RenewDuration of a message in the channel where she was published to 
        /// </summary>
        public TimeSpan MaxRenewDuration { get;  }
    }
}