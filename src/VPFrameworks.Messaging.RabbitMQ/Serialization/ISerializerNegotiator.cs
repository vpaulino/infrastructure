using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace InfrastrutureClients.Messaging.RabbitMQ.Serialization
{
    /// <summary>
    /// APIs that allows support for diferent types of serialization
    /// </summary>
    public interface ISerializerNegotiator
    {
      
        /// <summary>
        /// Negotiates the correct ISerializer Instance
        /// </summary>
        /// <param name="messageProperties"></param>
        /// <returns></returns>
        ISerializer Negotiate(IBasicProperties messageProperties);
        
    }
}
