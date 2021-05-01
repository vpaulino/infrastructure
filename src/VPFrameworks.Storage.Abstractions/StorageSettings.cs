namespace Azure.Storage.Abstractions
{

     /// <summary>
     /// Gets the settings to enable the comunication to a storage provider
     /// </summary>
    public class StorageSettings
    {
        
        /// <summary>
        /// Creates new instance of
        /// </summary>
        /// <param name="connectionString"></param>
        public StorageSettings(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        
        /// <summary>
        /// Gets the connection string
        /// </summary>
        public string ConnectionString { get; }
    }
}