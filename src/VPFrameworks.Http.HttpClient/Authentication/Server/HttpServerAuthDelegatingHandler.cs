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
    public abstract class HttpServerAuthDelegatingHandler : DelegatingHandler
    {
        private IHttpAuthenticationProvider authenticationProvider;

        /// <summary>
        /// Creates a new instance of 
        /// </summary>
        /// <param name="authenticationProvider"></param>
        public HttpServerAuthDelegatingHandler(IHttpAuthenticationProvider authenticationProvider)
        {
            this.authenticationProvider = authenticationProvider;
        }

        private async Task<IPrincipal> ValidateCredentials(AuthenticationHeaderValue authenticationHeaderVal)
        {
            try
            {
                if (authenticationHeaderVal != null && !String.IsNullOrEmpty(authenticationHeaderVal.Parameter))
                {
                    IHttpAuthenticator authenticator = this.authenticationProvider.GetAuthenticator(authenticationHeaderVal.Scheme);
                    return  await authenticator.Authenticate(authenticationHeaderVal.Parameter);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// When overriden enables adding some custom behaviour arround the http request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            IPrincipal principal = null;
            if ((principal = await ValidateCredentials(request.Headers.Authorization)) != null)
            {
                SetHttpContext(principal);   
            }

            var response = await base.SendAsync(request, cancellationToken);
            if (response.StatusCode == HttpStatusCode.Unauthorized && !response.Headers.Contains("WwwAuthenticate"))
            {
                response.Headers.Add("WwwAuthenticate", request.Headers.Authorization.Scheme);
            }

            return response;
        }

        /// <summary>
        /// Responsable to Set the IPrincipal in the HttpContext
        /// </summary>
        /// <param name="principal"></param>
        protected abstract void SetHttpContext(IPrincipal principal);
        
    }
}
