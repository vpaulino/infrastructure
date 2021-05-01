namespace VPFrameworks.Messaging.Abstractions.Subscriber
{
    /// <summary>
    /// If the message received is associated to a session this will be the result of that message received
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SessionMessageReceived<T> : MessageReceived<T>
    {
        /// <summary>
        /// Creates a new instance of <see cref="SessionMessageReceived{T}"></see>
        /// </summary>
        /// <param name="messageId">unique message id</param>
        /// <param name="popReceipt"> receipt delivered</param>
        /// /// <param name="clientId">client id</param>
        /// <param name="payload">content of the message sent by the publisher</param>
        /// <param name="sessionParameters">session  parameters</param>
        public SessionMessageReceived(string messageId, string popReceipt, T payload,string clientId, MessageSession sessionParameters) : base(messageId, popReceipt, clientId, payload)
        {
            this.Session = sessionParameters;
        }
        /// <summary>
        /// Gets or sets the session parameters
        /// </summary>
        public MessageSession Session { get;  }
    }
}
