using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastrutureClients.Messaging.Abstractions
{
    /// <summary>
    /// Gets a connection string needed info 
    /// </summary>
    public class ConnectionSettings
    {
        /// <summary>
        /// Creates a new instance of <see cref="ConnectionSettings"/>
        /// </summary>
        public ConnectionSettings(string connectionString, string path)
        {
            this.ConnectionString = connectionString;
            this.Path = path;
        }

        /// <summary>
        /// Gets the Full Url to the endpoint 
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// Gets the name of the resource to where the connection is going to be made
        /// </summary>
        public string Path { get; }
        
    }
}
