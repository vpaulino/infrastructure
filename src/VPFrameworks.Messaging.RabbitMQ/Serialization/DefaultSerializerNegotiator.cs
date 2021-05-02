using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using System.Linq;

namespace InfrastrutureClients.Messaging.RabbitMQ.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultSerializerNegotiator : ISerializerNegotiator
    {

        ICollection<ISerializer> Serializers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializers"></param>
        public DefaultSerializerNegotiator(ICollection<ISerializer>  serializers)
        {
            this.Serializers = serializers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public ISerializer Negotiate(IBasicProperties properties)
        {
            return this.Serializers.Where((serializer) => !string.IsNullOrEmpty(properties.ContentType) && !string.IsNullOrEmpty(properties.ContentEncoding) && properties.ContentType.Equals(serializer.ContentType) && properties.ContentEncoding.Equals(serializer.ContentEncoding)).FirstOrDefault();
        }
    }
}
