using Azure.Storage.Abstractions.Blobs;
using Azure.Storage.Blobs;

namespace InfrastrutureClients.Storage.Azure.Blobs
{
    /// <summary>
    /// Extensions do azure blob 
    /// </summary>
    public static class CloudBlockBlobExtensions
    {
        /// <summary>
        /// Converts between azure sdk type and this sdk types
        /// </summary>
        /// <param name="blob"></param>
        /// <returns></returns>
        public static BlobDescriptionDetails ConvertToBlobDescriptionDetails(this BlobClient blob)
        {
            var blobProperties = blob.GetProperties().Value;
           return new BlobDescriptionDetails(blob.Uri, blob.BlobContainerName, false, blobProperties.ContentType, blobProperties.ContentLanguage, blobProperties.CreatedOn, blobProperties.ETag.ToString(), blobProperties.LastModified, blobProperties.ContentLength, blobProperties.LeaseStatus.ToString(), blobProperties.LeaseState.ToString(), blobProperties.LeaseDuration.ToString());
        }
    }
}
