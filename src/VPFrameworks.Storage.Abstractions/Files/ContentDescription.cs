using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.Storage.Abstractions.Files
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentDescription
    {
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shareName"></param>
        /// <param name="uri"></param>
        /// <param name="eTag"></param>
        /// <param name="lastModified"></param>
        /// <param name="metadata"></param>
        /// <param name="IsDirectory"></param>
        public ContentDescription(string shareName, Uri uri, string eTag, DateTimeOffset? lastModified, IDictionary<string, string> metadata, bool IsDirectory)
        {
            this.ShareName = shareName;
            this.Uri = uri;
            this.ETag = eTag;
            this.LastModified = lastModified;
            this.Metadata = metadata ?? new Dictionary<string, string>();
        }

        /// <summary>
        /// Get File share name
        /// </summary>
        public string ShareName { get; }
        
        /// <summary>
        /// Get share Uri
        /// </summary>
        public Uri Uri { get; }
        
        /// <summary>
        /// Get ETag value
        /// </summary>
        public string ETag { get; }
        
        /// <summary>
        /// Get the LastModified date
        /// </summary>
        public DateTimeOffset? LastModified { get; }

        /// <summary>
        /// Get the metadata
        /// </summary>
        public IDictionary<string, string> Metadata { get; }
    }
}
