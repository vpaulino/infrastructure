using System;

namespace InfrastrutureClients.Messaging.Abstractions.Publisher
{
    /// <summary>
    /// Represents the result of a publish operation
    /// </summary>
    public class PublishResult
    {
        private string id;
        private DateTimeOffset? expirationTime;

        
        /// <summary>
        /// Creates a new instance of PublishOperation
        /// </summary>
        /// <param name="id"></param>
        /// <param name="expirationTime"></param>
        /// <param name="InsertionTime"></param>
        /// <param name="popReceipt"></param>
        public PublishResult(string id, DateTimeOffset? expirationTime, DateTimeOffset? InsertionTime, string popReceipt)
        {
            this.id = id;
            this.expirationTime = expirationTime;
            PopReceipt = popReceipt;
        }

        /// <summary>
        /// Gets or sets the receipt of a message published but not yet visibible to be consumed.
        /// </summary>
        /// <remarks>
        /// This property is represents a receipt to the process that consumes the message. 
        /// </remarks>
        public string PopReceipt { get;  }

        /// <summary>
        /// Gets the id of the message published
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the datetime that the message will expire
        /// </summary>
        public DateTimeOffset? ExpirationTime { get;  }

        /// <summary>
        /// Gets a datetime when was the message published
        /// </summary>
        public DateTimeOffset? InsertionTime { get;  }



    }
}