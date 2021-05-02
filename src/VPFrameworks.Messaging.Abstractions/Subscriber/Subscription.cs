namespace InfrastrutureClients.Messaging.Abstractions.Subscriber
{
    /// <summary>
    /// result of a susbscribeAsyn operations
    /// </summary>
    public class Subscription
    {
        /// <summary>
        /// creates a new instance of subscription
        /// </summary>
        /// <param name="id"></param>
        public Subscription(object id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Gets the id of the subscription created
        /// </summary>
        public object Id { get; }


    }
}