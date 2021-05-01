using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Abstractions;
using Azure.Storage.Abstractions.Blobs;

namespace VPFrameworks.Storage.Abstractions
{
    /// <summary>
    /// API to provide comunication to a Blob Storage
    /// </summary>
    public interface IBlobStorageProvider
    {
        /// <summary>
        /// Delete one Blob
        /// </summary>
        /// <param name="location"></param>
        /// <param name="fileName"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<BlobDescriptionDetails> DeleteBlob(string location, string fileName, CancellationToken ct);
        
        /// <summary>
        /// Download blob using the stream parameter
        /// </summary>
        /// <param name="location"></param>
        /// <param name="fileName"></param>
        /// <param name="destination"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<BlobDescriptionDetails> Download(string location, string fileName, Stream destination, CancellationToken ct);

        /// <summary>
        /// Upload the Blob from the stream to a specfic location
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="location"></param>
        /// <param name="fileName"></param>
        /// <param name="contentType"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<BlobDescriptionDetails> Upload(Stream stream, string location, string fileName, string contentType, CancellationToken ct);

        /// <summary>
        /// Get complete information about a specfic blob
        /// </summary>
        /// <param name="location"></param>
        /// <param name="name"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<BlobDescriptionDetails> GetBlobDetails(string location, string name, CancellationToken ct);
        
        /// <summary>
        /// List of blobs in a specific location
        /// </summary>
        /// <param name="location"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ListResult<BlobDescription>> ListBlobs(string location, CancellationToken ct);
        
        /// <summary>
        /// List all the containers under a storage account 
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ListResult<ContainerItem>> ListContainers(CancellationToken ct);
        
        /// <summary>
        /// Move blobs between locations
        /// </summary>
        /// <param name="currentlocation"></param>
        /// <param name="newLocation"></param>
        /// <param name="fileName"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task MoveBlob(string currentlocation, string newLocation, string fileName, CancellationToken ct);
        
        /// <summary>
        /// Rename Blobs
        /// </summary>
        /// <param name="location"></param>
        /// <param name="currentFileName"></param>
        /// <param name="newFileName"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task RenameBlob(string location, string currentFileName, string newFileName, CancellationToken ct);
      
    }
}