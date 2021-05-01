using System;
using System.Collections.Generic;

namespace Azure.Storage.Abstractions.Files
{
    /// <summary>
    /// Remove file operation result
    /// </summary>
    public class RemoveFilesResult
    {
        private List<Tuple<string, bool>> filesDeleted = new List<Tuple<string, bool>>();

        /// <summary>
        /// Creates instance of RemoveFilesResult
        /// </summary>
        public RemoveFilesResult()
        {

        }

        /// <summary>
        /// add file deletion operation result
        /// </summary>
        /// <param name="file"></param>
        /// <param name="deleted"></param>
        public void Add(string file, bool deleted)
        {
            filesDeleted.Add(new Tuple<string, bool>(file, deleted));
        }
    }
}