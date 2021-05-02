namespace InfrastrutureClients.Http.Abstractions
{
    /// <summary>
    /// Represents the data needed to send authorization information
    /// </summary>
    public class AuthenticationData
    {
        /// <summary>
        /// The Schema to be used
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// The 
        /// </summary>
        public string Parameter { get; set; }
    }
}