using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastrutureClients.Serialization.Abstractions
{

    /// <summary>
    /// Groups all the settings needed to use during serialization
    /// </summary>
    public class SerializationSettings
    {
        /// <summary>
        /// Creates a new instance of SerializationSettings
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="serializationEncoding"></param>
        public SerializationSettings(string contentType, Encoding serializationEncoding)
        {
            this.ContentType = contentType;
            this.SerializationEncoding = serializationEncoding;
        }
        /// <summary>
        /// Gets or sets a description  of the data type ( application/json, application/xml, application/binary etc)
        /// </summary>
        public string ContentType { get;  }

        /// <summary>
        /// Gets or sets the encoding of the serialization
        /// </summary>
        public Encoding SerializationEncoding { get; }
    }
}
