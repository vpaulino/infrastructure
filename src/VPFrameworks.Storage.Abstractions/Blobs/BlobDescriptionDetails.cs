using System;
using System.Linq;

namespace Azure.Storage.Abstractions.Blobs
{
    /// <summary>
    /// Complete details of a azure Blob 
    /// </summary>
    public class BlobDescriptionDetails : BlobDescription
    {
        /// <summary>
        /// Creates a new instance of detailed description
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="containerName"></param>
        /// <param name="isDirectory"></param>
        /// <param name="contentType"></param>
        /// <param name="contentLanguage"></param>
        /// <param name="created"></param>
        /// <param name="eTag"></param>
        /// <param name="lastModified"></param>
        /// <param name="length"></param>
        /// <param name="leaseStatus"></param>
        /// <param name="leaseState"></param>
        /// <param name="leaseDuration"></param>
        public BlobDescriptionDetails(Uri uri, string containerName, bool isDirectory, string contentType, string contentLanguage, DateTimeOffset? created, string eTag, DateTimeOffset? lastModified, long length, string leaseStatus, string leaseState, string leaseDuration) : base(uri, containerName, isDirectory)
        {
            this.ContentType = contentType;
            this.ContentLanguage = contentLanguage;
            this.Created = created;
            this.ETag = eTag;
            this.LastModified = lastModified;
            this.Length = length;
            this.LeaseStatus = leaseStatus;
            this.LeaseState = leaseState;
            this.LeaseDuration = leaseDuration;
        }

        /// <summary>
        /// Gets the file content type
        /// </summary>
        public string ContentType { get;  }
        
        /// <summary>
        /// Gets the language of the file
        /// </summary>
        public string ContentLanguage { get; }

        /// <summary>
        /// Gets the date wich the blob was created
        /// </summary>
        public DateTimeOffset? Created { get; }

        /// <summary>
        /// Gets the ETag of the file
        /// </summary>
        public string ETag { get; }

        /// <summary>
        /// Gets the last modified date of the file
        /// </summary>
        public DateTimeOffset? LastModified { get; }

        /// <summary>
        /// Gets the number of bytes
        /// </summary>
        public long Length { get; }

        /// <summary>
        /// Gets the Lease Status of the blob 
        /// </summary>
        public string LeaseStatus { get; }


        /// <summary>
        /// Gets the Lease State 
        /// </summary>
        public string LeaseState { get; set; }

        /// <summary>
        /// Gets the Lease Duration
        /// </summary>
        public string LeaseDuration { get; }

        /// <summary>
        /// Overriden ToString 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{{ Uri:\"{this.Uri}\", Created:\"{Created}\", Length:\"{Length}\" }}";
        }
    }
}