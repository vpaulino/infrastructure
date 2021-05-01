namespace VPFrameworks.Messaging.Abstractions.Subscriber
{
    /// <summary>
    /// Type of handling that messages handler  execute
    /// </summary>
    /// <remarks>
    /// This information is useful to the concrete ISubscriber implementation
    /// </remarks>
    public enum HandleMode
    {
        /// <summary>
        /// If the <see cref="IMessageReceivedHandler{T}"/> execution represents the exact process of the message
        /// </summary>
        Sync = 0,

        /// <summary>
        /// If the <see cref="IMessageReceivedHandler{T}"/> execution was dispatched to another execution offloaded process
        /// </summary>
        Async = 1
    }
}