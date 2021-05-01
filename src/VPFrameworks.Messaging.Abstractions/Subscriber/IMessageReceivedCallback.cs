using System.Threading;
using System.Threading.Tasks;

namespace VPFrameworks.Messaging.Abstractions.Subscriber
{
    /// <summary>
    /// After messages are received, some brokers allow explicit the Acknowledge so the message can be removed from the messaging broker 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMessageReceivedCallback<T>
    {
        /// <summary>
        /// Message was processed with sucess and now can be removed
        /// </summary>
        /// <param name="message"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task SuccessAckAsync(MessageReceived<T> message, CancellationToken token = default);

        /// <summary>
        /// Message was not processed with success. It should be handled according to the broker implementation
        /// </summary>
        /// <param name="message">message to work with</param>
        /// <param name="token">cancellatio ntoken </param>
        /// <returns></returns>
        Task FailedAckAsync(MessageReceived<T> message, CancellationToken token = default);

    }
}
