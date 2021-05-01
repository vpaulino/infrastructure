namespace VPFrameworks.Messaging.Abstractions.Subscriber
{
    /// <summary>
    /// Contains all the data that represents the result of handling the message
    /// </summary>
    public class MessageReceivedHandleResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="success"></param>
        public MessageReceivedHandleResult(HandleMode mode, bool success)
        {
            this.Mode = mode;
            this.Success = success;
        }
        /// <summary>
        /// Gets the method how the message was handled
        /// </summary>
        public HandleMode Mode { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool Success { get; }
    }
}