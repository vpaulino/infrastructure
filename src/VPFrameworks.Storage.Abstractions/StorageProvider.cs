using System;
using System.Threading;

namespace Azure.Storage.Abstractions
{
    /// <summary>
    /// Base classe to represent comunication to any Azure Storage provider 
    /// </summary>
    public abstract class StorageProvider
    {
        /// <summary>
        /// Settings to comunicate with file storage
        /// </summary>
        protected readonly StorageSettings settings;

        /// <summary>
        /// Creates new instance of StorageProvider
        /// </summary>
        /// <param name="settings"></param>
        protected StorageProvider(StorageSettings settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ct"></param>
        protected void IsCancelled(CancellationToken ct)
        {
            if (ct.IsCancellationRequested)
                throw new OperationCanceledException();

        }





    }
}