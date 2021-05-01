using Azure.Storage.Abstractions.Files;
using System;
using System.Collections.Generic;


namespace Azure.Storage.Files
{
    /// <summary>
    /// Represents a File description
    /// </summary>
    public class FileDescription : ContentDescription
    {
         
        /// <summary>
        /// Create instances of a File Description
        /// </summary>
        /// <param name="shareName"></param>
        /// <param name="directoryName"></param>
        /// <param name="fileName"></param>
        /// <param name="uri"></param>
        /// <param name="length"></param>
        /// <param name="eTag"></param>
        /// <param name="lastModified"></param>
        /// <param name="metadata"></param>
        public FileDescription(string shareName, string directoryName, string fileName, Uri uri, long length, string eTag, DateTimeOffset? lastModified, IDictionary<string, string> metadata)
            : base(shareName, uri, eTag, lastModified, metadata, false)
        {
           
            DirectoryName = directoryName;
            FileName = fileName;
            Length = length;
            
        }



      /// <summary>
      /// Gets the name of the direcotory where it is located
      /// </summary>
        public string DirectoryName { get; }

        /// <summary>
        /// Gets the FileName
        /// </summary>
        public string FileName { get; }
        
        /// <summary>
        /// Gets the File Length
        /// </summary>
        public long Length { get; }
       
       
      
    }
}