using Azure.Storage.Abstractions.Blobs;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;

namespace VPFrameworks.Storage.Azure.Blobs
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
        public static BlobDescriptionDetails ConvertToBlobDescriptionDetails(this ICloudBlob blob)
        {
           return new BlobDescriptionDetails(blob.Uri, blob.Container.Name, false, blob.Properties.ContentType, blob.Properties.ContentLanguage, blob.Properties.Created, blob.Properties.ETag, blob.Properties.LastModified, blob.Properties.Length, blob.Properties.LeaseStatus.ToString(), blob.Properties.LeaseState.ToString(), blob.Properties.LeaseDuration.ToString());
        }
    }
}
