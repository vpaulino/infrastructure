using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Abstractions;
using Azure.Storage.Abstractions.Files;

namespace Azure.Storage.Files
{
    /// <summary>
    /// API to provide comunication to a File Storage
    /// </summary>
    public interface IFileStorageProvider
    {
        /// <summary>
        /// Copy files between directories
        /// </summary>
        /// <param name="root"></param>
        /// <param name="currentDirectory"></param>
        /// <param name="newDirectory"></param>
        /// <param name="file"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<FileDescription> CopyFile(string root, string currentDirectory, string newDirectory, string file, CancellationToken ct);
        
        /// <summary>
        /// Creates a new directory
        /// </summary>
        /// <param name="root"></param>
        /// <param name="directory"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DirectoryDescription> CreateDirectory(string root, string directory, CancellationToken ct);
        
        /// <summary>
        /// Creates one new file
        /// </summary>
        /// <param name="root"></param>
        /// <param name="directory"></param>
        /// <param name="name"></param>
        /// <param name="file"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<FileDescription> CreateFile(string root, string directory, string name, Stream file, CancellationToken ct);
        
        /// <summary>
        /// Creates a set of files
        /// </summary>
        /// <param name="root"></param>
        /// <param name="directory"></param>
        /// <param name="files"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IEnumerable<FileDescription>> CreateFiles(string root, string directory, IEnumerable<Tuple<string, Stream>> files, CancellationToken ct);
        
        /// <summary>
        /// Gets a set of files written to their own streams
        /// </summary>
        /// <param name="root"></param>
        /// <param name="directory"></param>
        /// <param name="files"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ListResult<FileDescription>> GetFiles(string root, string directory, IEnumerable<Tuple<string, Stream>> files, CancellationToken ct);
        
        /// <summary>
        /// List directory contents
        /// </summary>
        /// <param name="root"></param>
        /// <param name="directory"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ListResult<ContentDescription>> ListAllContent(string root, string directory, CancellationToken ct);
        
        /// <summary>
        /// Move Files between locations
        /// </summary>
        /// <param name="root"></param>
        /// <param name="currentDirectory"></param>
        /// <param name="newDirectory"></param>
        /// <param name="file"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<FileDescription> MoveFile(string root, string currentDirectory, string newDirectory, string file, CancellationToken ct);
        
        /// <summary>
        /// Removes some directory
        /// </summary>
        /// <param name="root"></param>
        /// <param name="directory"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DirectoryDescription> RemoveDirectory(string root, string directory, CancellationToken ct);
        
        /// <summary>
        /// Removes file from a specific directory
        /// </summary>
        /// <param name="root"></param>
        /// <param name="directory"></param>
        /// <param name="file"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> RemoveFile(string root, string directory, string file, CancellationToken ct);
        
        /// <summary>
        /// Removes a set of files
        /// </summary>
        /// <param name="root"></param>
        /// <param name="directory"></param>
        /// <param name="files"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<RemoveFilesResult> RemoveFiles(string root, string directory, IEnumerable<string> files, CancellationToken ct);
    }
}