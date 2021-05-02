using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace InfrastrutureClients.Messaging.RabbitMQ.Connections
{
    /// <summary>
    /// API that provides valid connection from a pool of connections to a server
    /// </summary>
    public interface IConnectionPool
    {
        /// <summary>
        /// Gets or creates a connection
        /// </summary>
        /// <param name="serverUri"></param>
        /// <returns></returns>
        IModel GetOrCreateChannel(string serverUri);
    }
}
