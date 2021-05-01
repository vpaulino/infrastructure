
using System.Threading;
using System.Threading.Tasks;

namespace VPFrameworks.Messaging.Abstractions.Publisher
{
    /// <summary>
    /// 
    /// </summary>
    public  interface IPublisher
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="payload"></param>
        /// <param name="path"></param>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<PublishResult> PublishAsync<T>(T payload, string path, MessageOptions options, CancellationToken token);

        
    }
}
