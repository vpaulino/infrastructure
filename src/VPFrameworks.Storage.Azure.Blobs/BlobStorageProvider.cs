using Azure.Storage.Abstractions;
using Azure.Storage.Abstractions.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VPFrameworks.Storage.Abstractions;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure;

namespace InfrastrutureClients.Storage.Azure.Blobs
{
    /// <summary>
    ///  Azure Blob Storage provider
    /// </summary>
    public class BlobStorageProvider : StorageProvider, IBlobStorageProvider
    {

        private BlobServiceClient blobServiceClient;
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        public BlobStorageProvider(StorageSettings settings) : base(settings)
        {
            this.blobServiceClient = new(settings.ConnectionString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        protected virtual async Task<BlobContainerClient> GetOrCreateContainer(string location)
        {
            var blobContainerclient = blobServiceClient.GetBlobContainerClient(location);
            await blobContainerclient.CreateIfNotExistsAsync();
            return blobContainerclient;
        }
         

        /// <summary>
        /// Lists all containers that exists on the account
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public virtual async Task<ListResult<ContainerItem>> ListContainers(CancellationToken ct)
        {
            
            var blobContainerItems =  this.blobServiceClient.GetBlobContainersAsync(cancellationToken:ct);
            var containerItems  = new ListResult<ContainerItem>();
            await foreach (BlobContainerItem item in blobContainerItems) 
            {                
                containerItems.Add(new ContainerItem(new Uri($"{this.blobServiceClient.Uri.AbsoluteUri}/{item.Name}"), item.Name));
            }

            return containerItems;
        }

        /// <summary>
        /// Renames the blob to a new name
        /// </summary>
        /// <param name="location">container name</param>
        /// <param name="currentFileName">current file name to be changed</param>
        /// <param name="newFileName">new file name</param>
        /// <param name="ct">operation cancellation token</param>
        /// <returns></returns>
        public virtual async Task RenameBlob(string location, string currentFileName, string newFileName, CancellationToken ct)
        {   
            BlobContainerClient containerClient = this.blobServiceClient.GetBlobContainerClient(location);
            var existingblob = containerClient.GetBlobClient(currentFileName);
            if (!await containerClient.ExistsAsync())
                throw new InvalidOperationException($"Container  {location} not found");

            if(! await existingblob.ExistsAsync())
                throw new InvalidOperationException($"blob {location}/{currentFileName} not found");


            BlobClient blobClient = containerClient.GetBlobClient(newFileName);
            var operation = await blobClient.StartCopyFromUriAsync(containerClient.GetBlobClient(currentFileName).Uri, new BlobCopyFromUriOptions(), ct);

            await operation.WaitForCompletionAsync(ct);
            await existingblob.DeleteAsync(cancellationToken: ct);

        }

        /// <summary>
        /// Moves one blob from one location to other location
        /// </summary>
        /// <param name="currentlocation">current container name</param>
        /// <param name="newLocation">destination container name</param>
        /// <param name="fileName">file to move</param>
        /// <param name="ct">operation container name</param>
        /// <returns></returns>
        public virtual async Task MoveBlob(string currentlocation, string newLocation, string fileName, CancellationToken ct)
        {
            BlobContainerClient currentContainerClient = this.blobServiceClient.GetBlobContainerClient(currentlocation);
            BlobContainerClient newContainerClient = this.blobServiceClient.GetBlobContainerClient(newLocation);

            var existingblob = currentContainerClient.GetBlobClient(fileName);
            if (!await currentContainerClient.ExistsAsync())
                throw new InvalidOperationException($"Container  {currentlocation} not found");

            if (!await existingblob.ExistsAsync())
                throw new InvalidOperationException($"blob {currentlocation}/{fileName} not found");

            BlobClient blobClient = newContainerClient.GetBlobClient(fileName);
            
            var operation = await blobClient.StartCopyFromUriAsync(currentContainerClient.GetBlobClient(fileName).Uri, new BlobCopyFromUriOptions(), ct);

            await operation.WaitForCompletionAsync(ct);
            await existingblob.DeleteAsync(cancellationToken: ct);
        }

        /// <summary>
        /// Lists all blobs that exists on the location
        /// </summary>
        /// <param name="location">container name</param>
        /// <param name="ct">operation cancellation token</param>
        /// <returns></returns>
        public virtual async Task<ListResult<BlobDescription>> ListBlobs(string location,  CancellationToken ct)
        {
            BlobContainerClient containerClient = this.blobServiceClient.GetBlobContainerClient(location);

            AsyncPageable<BlobItem> blobItems = containerClient.GetBlobsAsync(cancellationToken: ct);

            ListResult<BlobDescription> blobDescriptions = new(); 
            await foreach (BlobItem item in blobItems) 
            {
                blobDescriptions.Add(new BlobDescription(new Uri($"{containerClient.Uri.AbsoluteUri}/{item.Name}"), containerClient.Name, false));
            }

            return blobDescriptions;
        }
         
        /// <summary>
        /// Gets all the detailed information regarding an existing blob
        /// </summary>
        /// <param name="location">container name</param>
        /// <param name="name">file name</param>
        /// <param name="ct">operation cancellation token</param>
        /// <returns></returns>
        public virtual async Task<BlobDescriptionDetails> GetBlobDetails(string location, string name, CancellationToken ct)
        {
            BlobContainerClient containerClient = this.blobServiceClient.GetBlobContainerClient(location);
            BlobClient blobClient = containerClient.GetBlobClient(name);

            return await Task.FromResult(blobClient.ConvertToBlobDescriptionDetails());

        }

        /// <summary>
        /// Deletes blob from the blob storage
        /// </summary>
        /// <param name="location">blob container name</param>
        /// <param name="fileName">file name</param>
        /// <param name="ct">operation cancellation token</param>
        /// <returns></returns>
        public virtual async Task<BlobDescriptionDetails> DeleteBlob(string location, string fileName, CancellationToken ct)
        {
            BlobContainerClient containerClient = this.blobServiceClient.GetBlobContainerClient(location);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            var details = blobClient.ConvertToBlobDescriptionDetails();
            await blobClient.DeleteAsync(cancellationToken: ct);
            return details;
        }

        /// <summary>
        /// Downloads blob from the cloud storage
        /// </summary>
        /// <param name="location">container name</param>
        /// <param name="fileName">file name to download</param>
        /// <param name="destination">stream destination</param>
        /// <param name="ct">operation cancellation token</param>
        /// <returns></returns>
        public virtual async Task<BlobDescriptionDetails> Download(string location, string fileName, Stream destination, CancellationToken ct)
        {

            BlobContainerClient containerClient = this.blobServiceClient.GetBlobContainerClient(location);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            Response<BlobDownloadInfo> downloadInfo = await blobClient.DownloadAsync();
            await downloadInfo.Value.Content.CopyToAsync(destination);
            
            return blobClient.ConvertToBlobDescriptionDetails();
        }

        /// <summary>
        /// Uploads stream content as a blob 
        /// </summary>
        /// <param name="stream">content to be uploaded</param>
        /// <param name="location">container name</param>
        /// <param name="fileName">blob name to be set</param>
        /// <param name="contentType">blob name to be set</param>
        /// <param name="ct">operation cancellation token</param>
        /// <returns></returns>
        public virtual async Task<BlobDescriptionDetails> Upload(Stream stream, string location, string fileName, string contentType, CancellationToken ct)
        {
            BlobContainerClient containerClient = this.blobServiceClient.GetBlobContainerClient(location);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            Response<BlobContentInfo>  uploadInfo = await blobClient.UploadAsync(stream, new BlobUploadOptions() { HttpHeaders = new BlobHttpHeaders() { ContentType = contentType } }, ct);
            return blobClient.ConvertToBlobDescriptionDetails();
        }
    }
}
