using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastrutureClients.Messaging.RabbitMQ
{
    /// <summary>
    /// Enum information if ithe message should be persisted by the broker or not
    /// </summary>
    public enum MessagePersistence
    {
        /// <summary>
        /// IT will not be persisted
        /// </summary>
        NoPersistent = 1,
        
        /// <summary>
        /// It will be persisted
        /// </summary>
        Persistent = 2,
        
    }
}
