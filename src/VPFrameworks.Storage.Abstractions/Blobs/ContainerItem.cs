using System;
using System.Linq;

namespace Azure.Storage.Abstractions.Blobs
{
    /// <summary>
    /// Represents a container information
    /// </summary>
    public class ContainerItem
    {
        /// <summary>
        /// Create instances of this type
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="containerName"></param>
        public ContainerItem(Uri uri, string containerName)
        {
            this.Uri = uri;
            this.ContainerName = containerName;
        }

        /// <summary>
        /// Gets the Uri of the Container
        /// </summary>
        public Uri Uri { get;  }

        /// <summary>
        /// Gets the container name
        /// </summary>
        public string ContainerName { get; }
       

    }
}