using System;
using System.Collections.Generic;
using System.Text;

namespace VPFrameworks.Persistence.Abstractions
{
    /// <summary>
    /// Defines the base settings that should exist to configure the access to any database
    /// </summary>
    public class DatabaseSettings
    {
        /// <summary>
        /// Gets or sets the ID of the application that is connecting to the database
        /// </summary>
        public string ClientApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the connection string 
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name of the database
        /// </summary>
        public string DataBaseName { get; set; }

        /// <summary>
        /// Get or sets the name of the data set associated to this connection
        /// </summary>
        public string SetName { get; set; }
        /// <summary>
        /// Gets the number of seconds to execute an command 
        /// </summary>
        public int? CommandsTimeout { get; set; }
    }
}
