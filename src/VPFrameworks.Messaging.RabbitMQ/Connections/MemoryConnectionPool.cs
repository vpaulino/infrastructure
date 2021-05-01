using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using VPFrameworks.Messaging.RabbitMQ.Connections;

namespace VPFrameworks.Messaging.RabbitMQ.Connections
{

    /// <summary>
    /// Connections are managed in memory
    /// </summary>
    public class MemoryConnectionPool : IConnectionPool
    {
        private IConnectionFactory connectionFactory = new ConnectionFactory();
        private ConcurrentDictionary<string, IConnection> connectionPool = new ConcurrentDictionary<string, IConnection>();
        SemaphoreSlim createConnectionSem = new SemaphoreSlim(1);
        private ILogger logger;

        /// <summary>
        /// creates new instance
        /// </summary>
        /// <param name="loggerProvider"></param>
        public MemoryConnectionPool(ILoggerProvider loggerProvider)
        {
            logger = loggerProvider.CreateLogger("MemoryConnectionPool");
        }
            
        /// <summary>
        /// Gets or creates a new connection to the server
        /// </summary>
        /// <param name="serverUri"></param>
        /// <returns></returns>
        public IModel GetOrCreateChannel(string serverUri)
        {

            createConnectionSem.Wait();

            IConnection connection = connectionPool.GetOrAdd(serverUri, CreateConnection);
            IModel result = connection.CreateModel();

            createConnectionSem.Release();

            return result;
        }

        private IConnection CreateConnection(string serverUri)
        {
            var conn = connectionFactory.CreateConnection(new List<AmqpTcpEndpoint>() { new AmqpTcpEndpoint(new Uri(serverUri)) });

            conn.ConnectionShutdown += Conn_ConnectionShutdown;
            conn.ConnectionRecoveryError += Conn_ConnectionRecoveryError;
            conn.RecoverySucceeded += Conn_RecoverySucceeded;

            return conn;
        }

        /// <summary>
        /// Event handler that receives notification about some connection has recovered with success
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Conn_RecoverySucceeded(object sender, EventArgs e)
        {
            logger.LogInformation($"Conn_RecoverySucceeded - {sender.ToString()} : {e.ToString()}");
        }

        /// <summary>
        /// Event handler that receives notification that connection has not recovered with success
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Conn_ConnectionRecoveryError(object sender, ConnectionRecoveryErrorEventArgs e)
        {
            logger.LogInformation($"Conn_ConnectionRecoveryError - {sender.ToString()} : {e.ToString()}");
        }

        /// <summary>
        /// Event handler that receives notification that connection was shutdown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Conn_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            logger.LogInformation($"Conn_ConnectionShutdown - {sender.ToString()} : {e.ToString()}");
        }
    }
}
