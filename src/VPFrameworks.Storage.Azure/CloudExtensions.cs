
using Azure.Storage.Abstractions.Files;
using Azure.Storage.Files;
using Microsoft.Azure.Storage.File;

namespace InfrastrutureClients.Storage.Azure.Files.Extensions   
{
    /// <summary>
    /// Extensions of File storage types
    /// </summary>
    public static class CloudExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileReference"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static FileDescription ConvertToFileDescription(this CloudFile fileReference, string directory)
        {
           return new FileDescription(directory, fileReference.Parent.Name, fileReference.Name, fileReference.Uri, fileReference.Properties.Length, fileReference.Properties.ETag, fileReference.Properties.LastModified, fileReference.Metadata);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirReference"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        public static DirectoryDescription ConvertToDirectoryDescription(this CloudFileDirectory dirReference, string root)
        {
            return new DirectoryDescription(root, dirReference.Name, dirReference.Uri, dirReference.Properties.ETag, dirReference.Properties.LastModified, dirReference.Metadata);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listFileItem"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        public static ContentDescription ConvertToContentDescription(this IListFileItem listFileItem, string root)
        {
            if (listFileItem is CloudFileDirectory)
            {
                var cloudFileDirectory = listFileItem as CloudFileDirectory;
                return new DirectoryDescription(root, cloudFileDirectory.Name, listFileItem.Uri, cloudFileDirectory.Properties.ETag, cloudFileDirectory.Properties.LastModified, cloudFileDirectory.Metadata);
            }

            if (listFileItem is CloudFile)
            {
                var cloudFile = listFileItem as CloudFile;
                return new DirectoryDescription(root, cloudFile.Name, listFileItem.Uri, cloudFile.Properties.ETag, cloudFile.Properties.LastModified, cloudFile.Metadata);
            }

            return null;

        }

    }
}
