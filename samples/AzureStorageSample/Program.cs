using System;
using Azure.Storage.Queues;
namespace AzureStorageSample
{
    class Program
    {
        private static string connectionString;
        private static string queueName;
        private static string blobContainerName;
        private static string blobName;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            QueueClient qclient = new(connectionString, queueName, new QueueClientOptions() );


            Azure.Storage.Blobs.BlobClient bloblClient = new(connectionString, blobContainerName, blobName);

            Azure.Storage.Blobs.BlobContainerClient containerClient = new(connectionString, blobContainerName);

        }
    }
}
