using System;
using System.Linq;

namespace Azure.Storage.Abstractions.Blobs
{
    /// <summary>
    /// 
    /// </summary>
    public class BlobDescription
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="containerName"></param>
        /// <param name="isDirectory"></param>
        public BlobDescription(Uri uri, string containerName, bool isDirectory)
        {
            this.Uri = uri;
            this.ContainerName = containerName;
            this.FileName = this.Uri.AbsoluteUri.Split('/').Last() ;
            this.IsDirectory = isDirectory;
        }

        /// <summary>
        /// Gets the blob Uri file location
        /// </summary>
        public Uri Uri { get;  }

        /// <summary>
        /// Gets the container name where the blob is stored
        /// </summary>
        public string ContainerName { get; }

        /// <summary>
        /// Gets the blob file name 
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Gets the info if the fileName is a directory or a file
        /// </summary>
        public bool IsDirectory { get; }
    }
}