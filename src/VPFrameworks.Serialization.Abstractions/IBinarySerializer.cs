using System.Threading.Tasks;

namespace VPFrameworks.Serialization.Abstractions
{
    /// <summary>
    /// Contract to serializer and deserialize data from binary fromat
    /// </summary>
    public interface IBinarySerializer 
    { 
        /// <summary>
        /// Serializes an instance of T to an array of Bytes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        Task<byte[]> Serialize<T>(T message, SerializationSettings settings);

        /// <summary>
        /// Deserializes an array of bytes to an instance of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="body"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        Task<T> Deserialize<T>(byte[] body, SerializationSettings settings);
    }
}