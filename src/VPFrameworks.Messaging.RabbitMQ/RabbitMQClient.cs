using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using VPFrameworks.Messaging.RabbitMQ.Connections;
using VPFrameworks.Messaging.RabbitMQ.Serialization;

namespace VPFrameworks.Messaging.RabbitMQ
{
    /// <summary>
    /// Base type to diferent comunication patterns with RabbitMQ server
    /// </summary>
    public abstract class RabbitMQClient
    {
        /// <summary>
        /// Gets the server base address 
        /// </summary>
        public string ServerUri { get; private set; }

        /// <summary>
        /// Gets the connection handler to the server
        /// </summary>
        protected IModel channel;

        /// <summary>
        /// Gets the name of the Exchange that supports the comunication
        /// </summary>
        public string Exchange { get; private set; }

        /// <summary>
        /// Gets the serialization negotiator
        /// </summary>
        protected ISerializerNegotiator SerializerNegotiator { get; }
        
        /// <summary>
        /// Gets the connection Poll instance to retrieve valid reusable connections to an server
        /// </summary>
        protected IConnectionPool ConnectionPool { get; }

        /// <summary>
        /// Gets the logger
        /// </summary>
        public ILogger logger { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected SemaphoreSlim channelControl = new SemaphoreSlim(1);

        
        /// <summary>
        /// creates instance of RabbitMQClient
        /// </summary>
        /// <param name="serverUri"></param>
        /// <param name="exchange"></param>
        /// <param name="connectionPool"></param>
        /// <param name="serializerNegotiator"></param>
        /// <param name="loggerProvider"></param>
        protected RabbitMQClient(string serverUri, string exchange, IConnectionPool connectionPool, ISerializerNegotiator serializerNegotiator, ILoggerProvider loggerProvider)
        {
            this.ServerUri = serverUri;
            this.Exchange = exchange;
            this.SerializerNegotiator = serializerNegotiator;
            this.ConnectionPool = connectionPool;

            this.logger = loggerProvider.CreateLogger("TopicPublisher");
        }

        /// <summary>
        /// Connects to the server
        /// </summary>
        /// <returns></returns>
        public virtual Task<bool> Connect()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            bool result = true;
            try
            {
                this.channel = this.ConnectionPool.GetOrCreateChannel(this.ServerUri);

                this.channel.ModelShutdown += Channel_ModelShutdown;
                
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, this.ServerUri, this.channel);
                result = false;
            }
            finally
            {
                tcs.SetResult(result);
            }


            return tcs.Task;


        }

        /// <summary>
        /// Disconnects from the server and dispose the connection
        /// </summary>
        /// <remarks>
        /// This cannot be executed here because this instance is not the owner of the connection. This connection should set a signal of release
        /// and then if shoudl be released by the pool
        /// </remarks>
        /// <returns></returns>
        public virtual Task<bool> Disconnect()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            bool result = true;

            try
            {
                channelControl.Wait();

                if (!this.channel.IsClosed)
                {
                    this.channel.Close();
                    this.channel.Dispose();

                }

                channelControl.Release();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, this.channel);
                result = false;
            }
            finally
            {
                tcs.SetResult(result);
            }


            return tcs.Task;

        }

        /// <summary>
        /// Channel shotdown occorred
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Channel_ModelShutdown(object sender, ShutdownEventArgs e)
        {
            this.logger.LogInformation(new EventId(1, "Channel_ModelShutdown"), "Channel_ModelShutdown fired ", sender, e);
            //this.logger.Info($"TopicPublisher.Channel_ModelShutdown; Sender: {sender.GetType().ToString()} -  Cause: {e.Cause}, ReplyCode: {e.ReplyCode}, ReplyText: {e.ReplyText} ");
        }


       


    }
}
