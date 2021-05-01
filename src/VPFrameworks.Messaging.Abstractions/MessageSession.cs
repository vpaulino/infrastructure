namespace VPFrameworks.Messaging.Abstractions
{
    /// <summary>
    /// Concrete implementations support session information when exchanging messages. 
    /// </summary>
    public class MessageSession
    {
        /// <summary>
        /// Creates new instance of MessageSession
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="clientId"></param>
        /// <param name="isClosed"></param>
        public MessageSession(string sessionId, string clientId, bool isClosed)
        {
            this.SessionId = sessionId;
            this.ClientId = clientId;
            this.IsClosed = isClosed;
        }

        /// <summary>
        /// Gets the id of the session. one messages can belong one session. one session can have multiple messages
        /// </summary>
        public virtual string SessionId { get; }

        /// <summary>
        /// Gets the client identification that is involved in message session
        /// </summary>
        public virtual string ClientId { get;  }

        /// <summary>
        /// Gets the info if the session is closed
        /// </summary>
        public virtual bool IsClosed { get; }

    }
}