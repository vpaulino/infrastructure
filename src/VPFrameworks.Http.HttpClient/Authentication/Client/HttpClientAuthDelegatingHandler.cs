using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VPFrameworks.Http.Abstractions
{
    /// <summary>
    /// Provides a delegate handler that supports authentication on the Server Side
    /// </summary>
    public class HttpClientAuthDelegatingHandler : DelegatingHandler
    {
        private IHttpCredentialsProvider credentialsProvider;

        /// <summary>
        /// Creates a new instance of 
        /// </summary>
        /// <param name="credentialsProvider"></param>
        public HttpClientAuthDelegatingHandler(IHttpCredentialsProvider credentialsProvider)
        {
            this.credentialsProvider = credentialsProvider;
        }

        private async Task<AuthenticationData> GetCredentials(Uri destinationUrl)
        {
            AuthenticationData  authData = await credentialsProvider.GetCredentials(destinationUrl);
            return authData;
        }
        /// <summary>
        /// When overriden enables adding some custom behaviour arround the http request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            
            AuthenticationData authData = await this.GetCredentials(request.RequestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue(authData.Scheme, authData.Parameter);
           
            var response = await base.SendAsync(request, cancellationToken);
            if (response.StatusCode == HttpStatusCode.Unauthorized && !response.Headers.Contains("WwwAuthenticate"))
            {
                response.Headers.Add("WwwAuthenticate", request.Headers.Authorization.Scheme);
            }

            return response;
        }

        
        
    }
}
