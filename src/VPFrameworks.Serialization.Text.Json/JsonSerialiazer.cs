using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using VPFrameworks.Serialization.Abstractions;

namespace Serialization.Text.Json
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonSerialiazer : ITextSerializer
    {

        Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public Task<T> Deserialize<T>(string text, SerializationSettings settings)
        {
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();

            try
            {                
                var clientSettings = settings as JsonSerializationSettings;
                T result = JsonConvert.DeserializeObject<T>(text, clientSettings.NewtonSoftSettings);

                tcs.SetResult(result);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        
            return tcs.Task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public Task<string> Serialize<T>(T entity, SerializationSettings settings)
        {
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

            try
            {
                var clientSettings = settings as JsonSerializationSettings;
                var result =JsonConvert.SerializeObject(entity, clientSettings.NewtonSoftSettings.Formatting);
                tcs.SetResult(result);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }

            return tcs.Task;
        }
    }
}
