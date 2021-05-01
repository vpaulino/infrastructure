namespace VPFrameworks.Http.Abstractions
{
    /// <summary>
    /// Contract to get one valid Authenticator to the scheme
    /// </summary>
    public interface IHttpAuthenticationProvider
    {
        /// <summary>
        /// Gets the Authenticator to be used to execute the authentication
        /// </summary>
        /// <param name="scheme">scheme name. see  https://developer.mozilla.org/en-US/docs/Web/HTTP/Authentication#Authentication_schemes </param>
        /// <returns></returns>
        IHttpAuthenticator GetAuthenticator(string scheme);
    }
}