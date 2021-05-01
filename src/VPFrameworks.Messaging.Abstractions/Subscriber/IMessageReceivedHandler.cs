using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VPFrameworks.Messaging.Abstractions.Subscriber
{

    /// <summary>
    /// Concrete implementations should handle the message to process. When they finish the process of the message in a way it is safe 
    /// to Callback the result of the process using an instance of <see cref="IMessageReceivedCallback{T}"/>.
    /// </summary>
    /// <typeparam name="T">Concrete type of the message</typeparam>
    public interface IMessageReceivedHandler<T>
    {
        /// <summary>
        /// handles the messages to the application
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<MessageReceivedHandleResult> HandleAsync(MessageReceived<T> payload, CancellationToken token = default(CancellationToken));
    }
}
