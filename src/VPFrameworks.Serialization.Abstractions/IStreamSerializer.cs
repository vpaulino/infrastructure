using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace VPFrameworks.Serialization.Abstractions
{
    /// <summary>
    /// Contract to serializer and deserialze data to and from some Stream
    /// </summary>
    public interface IStreamSerializer 
    {
        /// <summary>
        /// Deserializes from a stream an instance of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
           Task<T> Deserialize<T>(Stream stream, SerializationSettings settings);

        /// <summary>
        /// Serializes an instance of T to a stream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="stream"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
            Task Serialize<T>(T entity, Stream stream, SerializationSettings settings);
    }
}
