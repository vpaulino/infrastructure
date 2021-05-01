using System;
using System.Collections.Generic;
using VPFrameworks.Messaging.Abstractions;

namespace RabbitMQFacade.Subscriber
{
    internal class RabbitMQSubscriberOptions : MessageOptions
    {
        public RabbitMQSubscriberOptions(string clientId, int concurrentCalls = 1, TimeSpan? timeToLive = null, TimeSpan? initialVisibilityDelay = null, TimeSpan? serverTimeout = null, TimeSpan? maximumExecutionTime = null, IDictionary<string, string> userHeaders = null, string CustomUserAgent = null) : base(clientId, concurrentCalls, timeToLive, initialVisibilityDelay, serverTimeout, maximumExecutionTime, userHeaders, CustomUserAgent)
        {
        }
    }
}