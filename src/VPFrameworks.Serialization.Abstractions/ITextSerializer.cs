using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VPFrameworks.Serialization.Abstractions
{
    /// <summary>
    /// Serialize and Deserialize data from the text representation 
    /// </summary>
    public interface ITextSerializer
    {
        /// <summary>
        /// Deserializes a string to a instance of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        Task<T> Deserialize<T>(string text, SerializationSettings settings);

        /// <summary>
        /// Serializes an instance of T to a string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        Task<string> Serialize<T>(T entity, SerializationSettings settings);
    }
}
