using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VPFrameworks.Http.Abstractions
{
    /// <summary>
    /// Creates an instance of LoggingHandler
    /// </summary>
    public class HttpLoggingHandler : DelegatingHandler
    {
        private ILogger logger;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="innerHandler"></param>
        public HttpLoggingHandler(ILogger logger, HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
            this.logger = logger;
            
        }

        /// <summary>
        /// Sends the message
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            
            this.logger.LogInformation($"request: {request} {Environment.NewLine} Body: {await request.Content.ReadAsStringAsync()}");

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            this.logger.LogInformation($"response: {response} {Environment.NewLine} Body: {await response.Content.ReadAsStringAsync()}");

            return response;
        }
    }
}
