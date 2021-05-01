using Azure.Storage.Abstractions;
using Azure.Storage.Abstractions.Files;
using Azure.Storage.Files;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VPFrameworks.Storage.Azure.Files.Extensions;

namespace VPFrameworks.Storage.Azure.Files
{
    /// <summary>
    /// Provider to Azure File Storage
    /// </summary>
    public class FileStorageProvider : StorageProvider, IFileStorageProvider
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
        /// Creates new instance of this type
        /// </summary>
        /// <param name="settings"></param>
        public FileStorageProvider(StorageSettings settings) : base(settings)
        {
            
        }

        private CloudFileShare GetFileShareClient(string root)
        {
            CloudStorageAccount storageAccount = GetCloudStorageAccount();
            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();
            return fileClient.GetShareReference(root);
        }

        private CloudFileDirectory CreateDirectoryIfNotExists(string directory, CloudFileShare fileShare)
        {
            var rootReference = fileShare.GetRootDirectoryReference();
            var fileDirectory = rootReference.GetDirectoryReference(directory);
            fileDirectory.CreateIfNotExists();
            return fileDirectory;
        }

        private async Task<FileDescription> UploadFile(string name, Stream file, CloudFileDirectory cloudDdirectory, CancellationToken ct)
        {
            var fileReference = cloudDdirectory.GetFileReference(name);
            await fileReference.UploadFromStreamAsync(file, ct);

            return fileReference.ConvertToFileDescription(cloudDdirectory.Name);
        }

        

        private async Task<FileDescription> DownloadFile(string fileName, Stream file, CloudFileDirectory cloudDdirectory, CancellationToken ct)
        {
            CloudFile cloudFile = cloudDdirectory.GetFileReference(fileName);
            await cloudFile.DownloadRangeToStreamAsync(file, 0, cloudFile.Properties.Length, ct);
            return cloudFile.ConvertToFileDescription(cloudDdirectory.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="directory"></param>
        /// <param name="name"></param>
        /// <param name="file"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<FileDescription> CreateFile(string root, string directory, string name, Stream file, CancellationToken ct)
        {
            var fileShare = GetFileShareClient(root);
            await fileShare.CreateIfNotExistsAsync(ct);
            var cloudDdirectory = CreateDirectoryIfNotExists(directory, fileShare);
            return await UploadFile(name, file, cloudDdirectory, ct);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="directory"></param>
        /// <param name="files"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<IEnumerable<FileDescription>> CreateFiles(string root, string directory, IEnumerable<Tuple<string, Stream>> files, CancellationToken ct)
        {
            var fileShare = GetFileShareClient(root);
            await fileShare.CreateIfNotExistsAsync(ct);
            var cloudDdirectory = CreateDirectoryIfNotExists(directory, fileShare);
            List<FileDescription> fileItems = new List<FileDescription>();
            foreach (var file in files)
            {
                var fileDescription =  await UploadFile(file.Item1, file.Item2, cloudDdirectory, ct);
                fileItems.Add(fileDescription);
            }
            return fileItems;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="directory"></param>
        /// <param name="fileName"></param>
        /// <param name="file"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<FileDescription> GetFile(string root, string directory, string fileName, Stream file, CancellationToken ct)
        {
            var fileShare = GetFileShareClient(root);
            await fileShare.CreateIfNotExistsAsync(ct);
            var cloudDdirectory = CreateDirectoryIfNotExists(directory, fileShare);
           return await DownloadFile(fileName, file, cloudDdirectory, ct);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="directory"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ListResult<ContentDescription>> ListAllContent(string root, string directory, CancellationToken ct)
        {
            var fileShare = GetFileShareClient(root);
            await fileShare.CreateIfNotExistsAsync(ct);
            var cloudDdirectory = CreateDirectoryIfNotExists(directory, fileShare);
            List<IListFileItem> listResult = new List<IListFileItem>();
            FileContinuationToken continuationToken = null;
            do
            {
                var result = await cloudDdirectory.ListFilesAndDirectoriesSegmentedAsync(continuationToken, ct);
                continuationToken = result.ContinuationToken;
                listResult.AddRange(result.Results);

            } while (continuationToken != null);

            return new ListResult<ContentDescription>(listResult.Select((listFileItem) => listFileItem.ConvertToContentDescription(root) ));
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="directory"></param>
        /// <param name="files"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ListResult<FileDescription>> GetFiles(string root, string directory, IEnumerable<Tuple<string, Stream>> files, CancellationToken ct)
        {
            var fileShare = GetFileShareClient(root);
            await fileShare.CreateIfNotExistsAsync(ct);
            var cloudDdirectory = CreateDirectoryIfNotExists(directory, fileShare);

            var results = new ListResult<FileDescription>();

            foreach (var file in files)
            {   
                await DownloadFile(file.Item1, file.Item2, cloudDdirectory, ct);
                var fileRefence = cloudDdirectory.GetFileReference(file.Item1);
                results.Add(fileRefence.ConvertToFileDescription(directory));
            }

            return results;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="shareName"></param>
        /// <param name="directory"></param>
        /// <param name="file"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<bool> RemoveFile(string shareName, string directory, string file, CancellationToken ct)
        {
            var fileShare = GetFileShareClient(shareName);
            await fileShare.CreateIfNotExistsAsync(ct);
            var cloudDdirectory = fileShare.GetRootDirectoryReference();
            var directoryRef = cloudDdirectory.GetDirectoryReference(directory);
            
            if (directoryRef == null)
                throw new ArgumentException("directory does not exist");
 
            var fileRef = directoryRef.GetFileReference(file);
            return await fileRef.DeleteIfExistsAsync(ct);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="currentDirectory"></param>
        /// <param name="newDirectory"></param>
        /// <param name="file"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<FileDescription> MoveFile(string root, string currentDirectory, string newDirectory, string file, CancellationToken ct)
        {
            var fileShare = GetFileShareClient(root);
            await fileShare.CreateIfNotExistsAsync(ct);
            var cloudDdirectory = fileShare.GetRootDirectoryReference();
            var directoryRef = cloudDdirectory.GetDirectoryReference(currentDirectory);

            if (directoryRef == null)
                throw new ArgumentException("directory does not exist");

            var fileRef = directoryRef.GetFileReference(file);
            var newDirectoryRef = cloudDdirectory.GetDirectoryReference(newDirectory);
            await newDirectoryRef.CreateIfNotExistsAsync(ct);
            var newFileRef = newDirectoryRef.GetFileReference(file);

            if (await fileRef.ExistsAsync(ct))
            {
                await newFileRef.StartCopyAsync(fileRef, ct);
                await fileRef.DeleteAsync(ct);
                return new FileDescription(root, newDirectory, file, newFileRef.Uri, newFileRef.Properties.Length, newFileRef.Properties.ETag, newFileRef.Properties.LastModified, newFileRef.Metadata);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="currentDirectory"></param>
        /// <param name="newDirectory"></param>
        /// <param name="file"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<FileDescription> CopyFile(string root, string currentDirectory, string newDirectory, string file, CancellationToken ct)
        {
            var fileShare = GetFileShareClient(root);
            await fileShare.CreateIfNotExistsAsync(ct);
            var cloudDdirectory = fileShare.GetRootDirectoryReference();
            var directoryRef = cloudDdirectory.GetDirectoryReference(currentDirectory);

            if (directoryRef == null)
                throw new ArgumentException("directory does not exist");

            var fileRef = directoryRef.GetFileReference(file);
            var newDirectoryRef = cloudDdirectory.GetDirectoryReference(newDirectory);
            await newDirectoryRef.CreateIfNotExistsAsync(ct);
            var newFileRef = newDirectoryRef.GetFileReference(file);

            if (await fileRef.ExistsAsync(ct))
            {
                await newFileRef.StartCopyAsync(fileRef, ct);
                return new FileDescription(root, newDirectory, file, newFileRef.Uri, newFileRef.Properties.Length, newFileRef.Properties.ETag, newFileRef.Properties.LastModified, newFileRef.Metadata);

            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="directory"></param>
        /// <param name="files"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<RemoveFilesResult> RemoveFiles(string root, string directory, IEnumerable<string> files, CancellationToken ct)
        {
            var fileShare = GetFileShareClient(root);
            await fileShare.CreateIfNotExistsAsync(ct);
            var cloudDdirectory = fileShare.GetRootDirectoryReference();
            var directoryRef = cloudDdirectory.GetDirectoryReference(directory);

            if (directoryRef == null)
                throw new ArgumentException("directory does not exist");

            RemoveFilesResult result = new RemoveFilesResult();
            foreach (var file in files)
            {
                var fileRef = directoryRef.GetFileReference(file);
                result.Add(file, await fileRef.DeleteIfExistsAsync(ct));
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="directory"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<DirectoryDescription> RemoveDirectory(string root, string directory, CancellationToken ct)
        {
            var fileShare = GetFileShareClient(root);
            await fileShare.CreateIfNotExistsAsync(ct);
            var cloudDdirectory = fileShare.GetRootDirectoryReference();
            var directoryRef = cloudDdirectory.GetDirectoryReference(directory);
            var cloudDirectory = cloudDdirectory.ConvertToDirectoryDescription(root);
            await directoryRef.DeleteIfExistsAsync(ct);
            return cloudDirectory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="directory"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<DirectoryDescription> CreateDirectory(string root, string directory, CancellationToken ct)
        {
            CloudFileShare fileShare = GetFileShareClient(root);
            await fileShare.CreateIfNotExistsAsync();
            var cloudDdirectory = CreateDirectoryIfNotExists(directory, fileShare);
            return cloudDdirectory.ConvertToDirectoryDescription(root);
        }

         
    }
}
