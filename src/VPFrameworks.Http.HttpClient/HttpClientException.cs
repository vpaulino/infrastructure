using System;
using System.Collections.Generic;
using System.Text;

namespace VPFrameworks.Http.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpClientException  : Exception
    {
        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, object> ErrorInformation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ErrorCode { get; set; }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Creates a instance of <see cref="HttpClientException"/>
        /// </summary>
        /// <param name="message"></param>
        public HttpClientException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates a instance of <see cref="HttpClientException"/>
        /// </summary>
        /// <param name="statusCode"></param>
        public HttpClientException(int statusCode)
        {
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// Creates a instance of <see cref="HttpClientException"/>
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        public HttpClientException(int statusCode, string message) : base(message)
        {
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// Creates a instance of <see cref="HttpClientException"/>
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="message"></param>
        public HttpClientException(string errorCode, string message) : base(message)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// Creates a instance of <see cref="HttpClientException"/>
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <param name="errorInformation"></param>
        public HttpClientException(int statusCode, string message, Dictionary<string, object> errorInformation) 
        {
            this.StatusCode = statusCode;
            this.ErrorInformation = errorInformation;
        }
        /// <summary>
        /// Creates a instance of <see cref="HttpClientException"/>
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="errorInformation"></param>
        public HttpClientException(int statusCode, Dictionary<string, object> errorInformation) : this(statusCode)
        {
            ErrorInformation = errorInformation;
        }

        #endregion Public Constructors




    }
}
