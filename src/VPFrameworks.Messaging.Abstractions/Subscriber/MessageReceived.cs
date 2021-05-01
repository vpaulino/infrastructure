using System;
using System.Collections.Generic;
using System.Text;

namespace VPFrameworks.Messaging.Abstractions.Subscriber
{
    /// <summary>
    /// Message Received details
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MessageReceived<T>
    {
        /// <summary>
        /// Create new instance of MEssage REceived and all the data to reference that message in the broker if needed
        /// </summary>
        /// <param name="messageId">unique message id</param>
        /// <param name="popReceipt">receipt of the message that was delivered</param>
        /// /// <param name="clientId">client id</param>
        /// <param name="payload">content sent by the publisher client</param>
        public MessageReceived(string messageId, string popReceipt, string clientId, T payload)
        {
            this.MessageId = messageId;
            this.PopReceipt = popReceipt;
            this.ClientId = clientId;
            this.Payload = payload;
        }

        /// <summary>
        /// Gets or sets the message Id
        /// </summary>
        public string MessageId { get; }

        /// <summary>
        /// Gets or sets the message received receipt
        /// </summary>
        public string PopReceipt { get; }

        /// <summary>
        /// Gets the client id that sent this message
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the Content sent by the publisher
        /// </summary>
        public T Payload { get;  }
    }
}
