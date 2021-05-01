using Azure.Storage.Abstractions;
using Azure.Storage.Abstractions.Blobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VPFrameworks.Storage.Abstractions;

namespace VPFrameworks.Storage.Azure.Blobs
{
    /// <summary>
    ///  Azure Blob Storage provider
    /// </summary>
    public class BlobStorageProvider : StorageProvider, IBlobStorageProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual CloudStorageAccount GetCloudStorageAccount()
        {
            return CloudStorageAccount.Parse(this.settings.ConnectionString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        public BlobStorageProvider(StorageSettings settings) : base(settings)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="blobClient"></param>
        /// <returns></returns>
        protected virtual async Task<CloudBlobContainer> GetOrCreateContainer(string location, CloudBlobClient blobClient)
        {
            CloudBlobContainer container = blobClient.GetContainerReference(location);
            await container.CreateIfNotExistsAsync();
            return container;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="storageAccount"></param>
        /// <returns></returns>
        protected virtual CloudBlobContainer GetBlobContainer(string location, CloudStorageAccount storageAccount)
        {
            
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            return blobClient.GetContainerReference(location);
        }

        /// <summary>
        /// Lists all containers that exists on the account
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public virtual async Task<ListResult<ContainerItem>> ListContainers(CancellationToken ct)
        {
            CloudStorageAccount storageAccount = GetCloudStorageAccount();
            var client = storageAccount.CreateCloudBlobClient();
            BlobContinuationToken blobContinuationToken = null;
            List<CloudBlobContainer> cloudBlobContainers = new List<CloudBlobContainer>();

            do
            {
                var result = await client.ListContainersSegmentedAsync(blobContinuationToken);
                blobContinuationToken = result.ContinuationToken;
                cloudBlobContainers.AddRange(result.Results);
            } while (blobContinuationToken != null);
            
            return new ListResult<ContainerItem>(cloudBlobContainers.Select((container) => new ContainerItem(container.Uri, container.Name)));
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
            CloudStorageAccount storageAccount = GetCloudStorageAccount();
            CloudBlobContainer container = GetBlobContainer(location, storageAccount);
            CloudBlockBlob blobCopy = container.GetBlockBlobReference(newFileName);

            if (!await blobCopy.ExistsAsync())
            {
                
                CloudBlockBlob blob = container.GetBlockBlobReference(currentFileName);
                if (await blob.ExistsAsync())
                {
                    await blobCopy.StartCopyAsync(blob);
                    await blob.DeleteIfExistsAsync();
                }
            }
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
            CloudStorageAccount storageAccount = GetCloudStorageAccount();
            CloudBlobContainer container = GetBlobContainer(currentlocation, storageAccount);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer newContainer = await GetOrCreateContainer(newLocation, blobClient);
            CloudBlockBlob blobCopy = newContainer.GetBlockBlobReference(fileName);

            if (!await blobCopy.ExistsAsync())
            {
                CloudBlockBlob blob = container.GetBlockBlobReference(fileName);

                if (await blob.ExistsAsync())
                {
                    await blobCopy.StartCopyAsync(blob);
                    await blob.DeleteIfExistsAsync();
                }
            }
        }

        /// <summary>
        /// Lists all blobs that exists on the location
        /// </summary>
        /// <param name="location">container name</param>
        /// <param name="ct">operation cancellation token</param>
        /// <returns></returns>
        public virtual async Task<ListResult<BlobDescription>> ListBlobs(string location,  CancellationToken ct)
        {
            CloudStorageAccount storageAccount = GetCloudStorageAccount();
            CloudBlobContainer container = GetBlobContainer(location, storageAccount);
            BlobContinuationToken blobContinuationToken = null;

            List<IListBlobItem> listResult = new List<IListBlobItem>();
            do
            {
                var result = await container.ListBlobsSegmentedAsync(blobContinuationToken);
                blobContinuationToken = result.ContinuationToken;
                listResult.AddRange(result.Results);
            } while (blobContinuationToken != null);

            return new ListResult<BlobDescription>(listResult.Select((listBlobItem)=> new BlobDescription(listBlobItem.Uri, listBlobItem.Container.Name, listBlobItem is CloudBlobDirectory)));
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
            CloudStorageAccount storageAccount = GetCloudStorageAccount();
            CloudBlobContainer container = GetBlobContainer(location, storageAccount);
            var blob = await container.GetBlobReferenceFromServerAsync(name);
            return blob.ConvertToBlobDescriptionDetails();

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
            CloudStorageAccount storageAccount = GetCloudStorageAccount();
            CloudBlobContainer container = GetBlobContainer(location, storageAccount);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            var result = await blockBlob.DeleteIfExistsAsync();
            return blockBlob.ConvertToBlobDescriptionDetails();
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

            CloudStorageAccount storageAccount = GetCloudStorageAccount();
            CloudBlobContainer container = GetBlobContainer(location, storageAccount);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            await blockBlob.DownloadToStreamAsync(destination);
            return blockBlob.ConvertToBlobDescriptionDetails();
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
            CloudStorageAccount storageAccount = GetCloudStorageAccount();
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = await GetOrCreateContainer(location, blobClient);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

            blockBlob.Properties.ContentType = contentType;


            await blockBlob.UploadFromStreamAsync(stream);

            
            return blockBlob.ConvertToBlobDescriptionDetails();
        }
    }
}
