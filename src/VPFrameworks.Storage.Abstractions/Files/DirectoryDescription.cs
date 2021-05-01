using System;
using System.Collections.Generic;

namespace Azure.Storage.Abstractions.Files
{
    /// <summary>
    /// Gets the description of a directory
    /// </summary>
    public class DirectoryDescription : ContentDescription
    {
        
       /// <summary>
       /// Create instances of a directory 
       /// </summary>
       /// <param name="shareName"></param>
       /// <param name="name"></param>
       /// <param name="uri"></param>
       /// <param name="eTag"></param>
       /// <param name="lastModified"></param>
       /// <param name="metadata"></param>
        public DirectoryDescription(string shareName, string name, Uri uri, string eTag, DateTimeOffset? lastModified, IDictionary<string, string> metadata) : base(shareName, uri, eTag, lastModified, metadata, true)
        {
          
            this.Name = name;
           
        }
         
        /// <summary>
        /// Gets the name of a directory
        /// </summary>
        public string Name { get;   }
      
    }
}