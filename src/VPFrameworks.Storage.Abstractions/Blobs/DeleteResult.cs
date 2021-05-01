using System;

namespace Azure.Storage.Abstractions.Blobs
{
    /// <summary>
    /// Represents the Delete operation over a blob 
    /// </summary>
    public class DeleteResult
    {
        /// <summary>
        /// Creates instance of this type
        /// </summary>
        /// <param name="deleted"></param>
        /// <param name="fileDeleted"></param>
        public DeleteResult(bool deleted, Uri fileDeleted)
        {
            this.Deleted = deleted;
            this.FileDeleted = fileDeleted;
        }

        /// <summary>
        /// Gets info if the file was deleted of not
        /// </summary>
        public bool Deleted { get; }

        /// <summary>
        /// Gets the Deleted File Uri
        /// </summary>
        public Uri FileDeleted { get;  }
    }
}