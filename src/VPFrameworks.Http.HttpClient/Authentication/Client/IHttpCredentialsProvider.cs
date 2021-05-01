using System;
using System.Threading.Tasks;

namespace VPFrameworks.Http.Abstractions
{
    /// <summary>
    /// This an abstraction layer to fetch credentials from some specific place of the application
    /// </summary>
    public interface IHttpCredentialsProvider
    {
        /// <summary>
        /// Gets the credentials considering the Destination to where the request is happening
        /// </summary>
        /// <param name="destinationUrl"></param>
        /// <returns></returns>
        Task<AuthenticationData> GetCredentials(Uri destinationUrl);
    }
}