using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfrastrutureClients.Messaging.Abstractions.Subscriber
{

    /// <summary>
    /// The concrete implementations should provide the funcionality to subscribe to a broker to receive messages to be delivered to the <see cref="IMessageReceivedHandler{T}"></see>
    /// </summary>
    /// <typeparam name="T">concrete client payload message type</typeparam>
    public interface ISubscriber<T>
    {
        /// <summary>
        /// Concrete implementations should enable the reception of messages from the broker calling this method only once to start receiving messages
        /// API Clients  
        /// </summary>
        /// <param name="messageHandler">messages handler to process the message</param>
        /// <param name="options">message options to submit with the payload to the broker</param>
        /// <param name="token">token to cancel the operation</param>
        /// <returns>subscription information</returns>
        Task<Subscription> SubscribeAsync(IMessageReceivedHandler<T> messageHandler, MessageOptions options, CancellationToken token);

        /// <summary>
        /// Concrete implementations should pro
        /// </summary>
        /// <param name="subscription">subscription that was return from the SubscribeAsync</param>
        /// <returns>Task representing the async operation executed</returns>
        Task CancelSubscriptionAsync(Subscription subscription);
    }
}
