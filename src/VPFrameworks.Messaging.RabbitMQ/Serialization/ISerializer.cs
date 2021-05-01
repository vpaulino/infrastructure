using System.Text;
using RabbitMQ.Client;

namespace VPFrameworks.Messaging.RabbitMQ.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        byte[] Serialize<T>(T message);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <param name="messageProperties"></param>
        /// <returns></returns>
        T  DeSerialize<T>(byte[] bytes, IBasicProperties messageProperties);

        /// <summary>
        /// 
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// 
        /// </summary>
        string ContentEncoding { get; }


    }
}