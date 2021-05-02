using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace InfrastrutureClients.Http.Abstractions
{
    /// <summary>
    /// List the core methods to enable Http comunications
    /// </summary>
    public abstract class HttpClientHandler  
    {
        #region Private Fields

        /// <summary>
        /// URI of the base. 
        /// </summary>
        private Uri _baseUri;

        /// <summary>
        /// Gets the underlying Http instance
        /// </summary>
        public HttpClient HttpClient { get;  }

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets or sets the identifier of the application. 
        /// </summary>
        /// <value> The identifier of the application. </value>
        public string AppId
        {
            get;
        }

        private MediaTypeFormatter mediaTypeFormatter;

        private ILogger logger;


        /// <summary>
        /// Gets or sets URI of the base. 
        /// </summary>
        /// <value> The base URI. </value>
        public string BaseUri
        {
            get
            {
                return this._baseUri.AbsoluteUri;
            }
            set
            {
                var url = value;
                if (!Regex.IsMatch(value, @"(\/)\Z"))
                    url = url + "/";

                this._baseUri = new Uri(url);
            }
        }


        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="appId"></param>
        /// <param name="mediaTypeFormatter"></param>
        /// <param name="logger"></param>
        protected HttpClientHandler(HttpClient httpClient, string appId, MediaTypeFormatter mediaTypeFormatter, ILogger logger)
        {

            this.HttpClient = httpClient;
            this.AppId = appId;
            this.mediaTypeFormatter = mediaTypeFormatter;
            this.logger = logger;
        }


        #endregion Public Constructors

        #region Protected Methods

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseMessage"></param>
        /// <returns></returns>
        protected async Task<T> ReadResponseContent<T>(HttpResponseMessage responseMessage)
        {
            
            T responseContent = await responseMessage.Content.ReadAsAsync<T>(new List<MediaTypeFormatter>() { mediaTypeFormatter } );

            return responseContent;

        }

        /// <summary>
        /// Deletes an entity of a type T the asynchronous. 
        /// </summary>
        /// <remarks> Vpaulino, 6/29/2015. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="route"> The route. </param>
        /// <param name="responseHandler"> The response handler. </param>
        /// <param name="parameters"> Options for controlling the operation. </param>
        /// <returns> returns the deleted instance. </returns>
        public async Task<T> DeleteAsync<T>(string route, Func<HttpResponseMessage, Task<T>> responseHandler, params string[] parameters)
        {
            this.logger.LogDebug($"DELETE {route} Request starting ");
            var actionUri = string.Format(route, parameters);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, new Uri(this._baseUri, actionUri));
            HttpResponseMessage response = await this.HttpClient.SendAsync(request).ConfigureAwait(false);

            T responseContent = await responseHandler?.Invoke(response);

            this.logger.LogDebug($"DELETE {route} Request Finished ");
            return responseContent;
        }

        /// <summary>
        /// Executes patch http verb
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="route"></param>
        /// <param name="payload"></param>
        /// <param name="responseHandler"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<T> PatchAsync<T>(string route, T payload, Func<HttpResponseMessage, Task<T>> responseHandler, params string[] parameters)
        {
            this.logger.LogDebug($"Patch {route} Request Starting ");
            var actionUri = string.Format(route, parameters);
            var content = new ObjectContent(payload.GetType(), payload, this.mediaTypeFormatter);
            HttpResponseMessage response = await this.HttpClient.PatchAsync(actionUri, content).ConfigureAwait(false);

            T responseContent = await responseHandler?.Invoke(response);

            this.logger.LogDebug($"Patch {route} Request Finished ");
            return responseContent;
        }

        /// <summary>
        /// Generates a query string parameters. 
        /// </summary>
        /// <remarks> Vpaulino, 6/29/2015. </remarks>
        /// <param name="startParametersChar"> Starting parameter character </param>
        /// <param name="parameters"> Options for controlling the operation. </param>
        /// <returns> The query string parameters. </returns>
        protected string GenerateQueryStringParameters(string startParametersChar = "", params KeyValuePair<string, string>[] parameters)
        {
            StringBuilder builder = new StringBuilder();

            var formatedParameters = parameters
                .Where(p => !string.IsNullOrWhiteSpace(p.Value))
                .Select(p => p.Key + "=" + Uri.EscapeDataString(p.Value));

            string parametersConcat = string.Join("&", formatedParameters);

            if (parameters.Any(p => !string.IsNullOrWhiteSpace(p.Value)))
            {
                parametersConcat = startParametersChar + parametersConcat;
            }

            return parametersConcat;
        } 

        /// <summary>
        /// Gets the asynchronous. 
        /// </summary>
        /// <remarks> Vpaulino, 6/29/2015. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="route"> The route. </param>
        /// <param name="responseHandler"> The response handler. </param>
        /// <param name="parameters"> Options for controlling the operation. </param>
        /// <returns> The async&lt; t&gt; </returns>
        public async Task<T> GetAsync<T>(string route, Func<HttpResponseMessage, Task<T>> responseHandler, params string[] parameters)
        {

            this.logger.LogDebug($"GET {route} Request starting ");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, new Uri(this._baseUri, string.Format(route, parameters)));


            HttpResponseMessage response = await this.HttpClient.SendAsync(request);

            T responseContent = await responseHandler(response);

            this.logger.LogDebug($"Path {route} Request Finished ");

            return responseContent;
        }

        /// <summary>
        /// Bulk delete operation to run asynchronous. 
        /// </summary>
        /// <remarks> Vpaulino, 6/29/2015. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="route"> The route. </param>
        /// <param name="content"> The content. </param>
        /// <param name="responseHandler"> The response handler. </param>
        /// <param name="parameters"> Options for controlling the operation. </param>
        /// <returns> returns the deleted item. </returns>
        protected async Task<T> LoadedDeleteAsync<T>(string route, object content, Func<HttpResponseMessage, Task<T>> responseHandler, params string[] parameters)
        {
            //Guard.ThrowIfNull(route, "route");
            //Guard.ThrowIfNull(content, "content");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, new Uri(this._baseUri, string.Format(route, parameters)))
            {
                Content = new ObjectContent(content.GetType(), content, this.mediaTypeFormatter)
            };
            HttpResponseMessage response = await this.HttpClient.SendAsync(request).ConfigureAwait(false);

            T responseContent = await responseHandler?.Invoke(response);

            return responseContent;
        }

        /// <summary>
        /// Posts the asynchronous request and return the response object . 
        /// </summary>
        /// <remarks> Vpaulino, 6/29/2015. </remarks>
        /// <typeparam name="T"> Generic output parameter type. </typeparam>
        /// <typeparam name="U"> Generic input parameter type. </typeparam>
        /// <param name="route"> The route. </param>
        /// <param name="content"> The content. </param>
        /// <param name="responseHandler"> The response handler. </param>
        /// <param name="parameters"> Options for controlling the operation. </param>
        /// <returns> Returns the result instance of the operation. </returns>
        public async Task<T> PostJsonAsync<T, U>(string route, U content, Func<HttpResponseMessage, Task<T>> responseHandler, params string[] parameters)
        {
            //Guard.ThrowIfNull(route, "route");
            //Guard.ThrowIfNull(content, "content");

            this.logger.LogDebug($"POST {route} Request starting ");

            HttpResponseMessage response = await this.HttpClient.PostAsJsonAsync<U>(new Uri(this._baseUri, string.Format(route, parameters)), content).ConfigureAwait(false);

            T responseContent = await responseHandler?.Invoke(response);


            this.logger.LogDebug($"POST {route} Request Finished ");

            return responseContent;
        }

        /// <summary>
        /// Posts the asynchronous request and return void or throws an exception if the status code
        /// does not represent success.
        /// </summary>
        /// <remarks> Vpaulino, 6/29/2015. </remarks>
        /// <typeparam name="U"> Generic input parameter type. </typeparam>
        /// <param name="route"> The route. </param>
        /// <param name="content"> The content. </param>
        /// <param name="responseHandler"> The response handler. </param>
        /// <param name="parameters"> Options for controlling the operation. </param>
        /// <returns> Returns void. If the process is successful it does not throw any exceptions. </returns>
        public async Task PostJsonAsync<U>(string route, U content, Func<HttpResponseMessage, Task> responseHandler, params string[] parameters)
        {
            //Guard.ThrowIfNull(route, "route");
            //Guard.ThrowIfNull(content, "content");

            this.logger.LogDebug($"POST {route} Request starting ");


            HttpResponseMessage response = await this.HttpClient.PostAsJsonAsync<U>(new Uri(this._baseUri, string.Format(route, parameters)), content).ConfigureAwait(false);

            await responseHandler?.Invoke(response);

            this.logger.LogDebug($"POST {route} Request Finished ");

            return;
        }

        /// <summary>
        /// Puts the asynchronous. 
        /// </summary>
        /// <remarks> Vpaulino, 6/29/2015. </remarks>
        /// <typeparam name="T"> Generic output parameter type. </typeparam>
        /// <typeparam name="U"> Generic input parameter type. </typeparam>
        /// <param name="route"> The route. </param>
        /// <param name="content"> The content. </param>
        /// <param name="responseHandler"> The response handler. </param>
        /// <param name="parameters"> Options for controlling the operation. </param>
        /// <returns> Returns the result instance of the operation. </returns>
        public async Task<T> PutAsync<T, U>(string route, U content, Func<HttpResponseMessage, Task<T>> responseHandler, params string[] parameters)
        {

            this.logger.LogDebug($"PUT {route} Request starting ");

            HttpResponseMessage response = await this.HttpClient.PutAsJsonAsync<U>(new Uri(this._baseUri, string.Format(route, parameters)), content).ConfigureAwait(false);

            T responseContent = await responseHandler(response);


            this.logger.LogDebug($"PUT {route} Request Finished ");

            return responseContent;
        }

        /// <summary>
        /// Request asynchronous. 
        /// </summary>
        /// <remarks> Vpaulino, 6/29/2015. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="method"> The method. </param>
        /// <param name="route"> The route. </param>
        /// <param name="content"> The content. </param>
        /// <param name="parameters"> Options for controlling the operation. </param>
        /// <returns> A Task&lt;T&gt; </returns>
        public async Task<T> RequestAsync<T>(HttpMethod method, string route, object content, params string[] parameters)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, new Uri(this._baseUri, string.Format(route, parameters)));

            if (content != null)
                request.Content = new ObjectContent(content.GetType(), content, this.mediaTypeFormatter);

            HttpResponseMessage response = await this.HttpClient.SendAsync(request).ConfigureAwait(false);

            return await response.Content.ReadAsAsync<T>();
        }

        /// <summary>
        /// Throw exception 
        /// </summary>
        /// <param name="httpResponseMessage"></param>
        /// <param name="defaultMessage"></param>
        /// <returns></returns>
        protected async Task ThrowIfCodedException(HttpResponseMessage httpResponseMessage, string defaultMessage)
        {
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                Dictionary<string, object> errorInformation = await httpResponseMessage.Content.ReadAsAsync<Dictionary<string, object>>();
                string message = defaultMessage;

                if (errorInformation != null)
                {
                    object value = null;
                    errorInformation.TryGetValue("ErrorId", out value);
                    message = value.ToString();
                }
                else
                {
                    message = httpResponseMessage.ReasonPhrase;
                }

                throw new HttpClientException((int)httpResponseMessage.StatusCode, message, errorInformation);
            }
        }

        /// <summary>
        /// ThrowIfCodedException
        /// </summary>
        /// <param name="httpResponseMessage"></param>
        /// <param name="defaultMessage"></param>
        /// <param name="codesTobypass"></param>
        /// <returns></returns>
        protected async Task ThrowIfCodedException(HttpResponseMessage httpResponseMessage, string defaultMessage, params HttpStatusCode[] codesTobypass)
        {
            if (codesTobypass != null && codesTobypass.Contains(httpResponseMessage.StatusCode))
            {
                return;
            }

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                Dictionary<string, object> errorInformation =  await RetrieveErrorResponseData(httpResponseMessage);

                throw new HttpClientException((int)httpResponseMessage.StatusCode, errorInformation);
            }
        }
        /// <summary>
        /// Retrieves a dictionary from the response that represents the infor needed to characterize the error
        /// </summary>
        /// <param name="httpResponseMessage"></param>
        /// <returns></returns>
        protected abstract Task<Dictionary<string, object>> RetrieveErrorResponseData(HttpResponseMessage httpResponseMessage);

        /// <summary>
        /// Throw if not success. 
        /// </summary>
        /// <remarks> Vpaulino, 6/29/2015. </remarks>
        /// <exception cref="HttpClientException"> Thrown when an AA error condition occurs. </exception>
        /// <param name="response"> The response. </param>
        protected void ThrowIfNotSuccess(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                throw new HttpClientException((int)response.StatusCode, string.Format("Authorization platform returned: {0} {1}", (int)response.StatusCode, response.StatusCode.ToString()
             ));
        }

        #endregion Protected Methods
    }
}