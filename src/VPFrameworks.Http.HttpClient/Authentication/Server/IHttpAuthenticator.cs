using System.Security.Principal;
using System.Threading.Tasks;

namespace VPFrameworks.Http.Abstractions
{
    /// <summary>
    /// Authenticator contract
    /// </summary>
    public interface IHttpAuthenticator
    {
        /// <summary>
        /// Authenticate method
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        Task<IPrincipal> Authenticate(string parameter);
    }
}