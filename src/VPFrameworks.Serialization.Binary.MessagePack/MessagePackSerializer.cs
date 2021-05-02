
using MessagePack;
using System;
using System.IO;
using System.Threading.Tasks;
using VPFrameworks.Serialization.Abstractions;
using VPFrameworks.Serialization.Binary.MessagePack;

namespace InfrastrutureClients.Serialization.Binary.MessagePack
{
    /// <summary>
    /// 
    /// </summary>
    public class MessagePackSerializer : IBinarySerializer
    {
         

        IFormatterResolver formatterResolver;

        /// <summary>
        /// 
        /// </summary>
        public string ContentType { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formatterResolver"></param>
        public MessagePackSerializer(IFormatterResolver formatterResolver)
        {
            this.formatterResolver = formatterResolver;
            this.ContentType = "application/message-pack";
        }

        /// <summary>
        /// 
        /// </summary>
        public MessagePackSerializer()
        {
            this.formatterResolver =  DefaultFormatterResolver.Instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="body"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public async Task<T> Deserialize<T>(byte[] body, SerializationSettings settings)
        {
            T result = default(T);
            using (MemoryStream stream = new MemoryStream())
            {
                result = await global::MessagePack.MessagePackSerializer.DeserializeAsync<T>(stream, this.formatterResolver);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public async Task<byte[]> Serialize<T>(T message, SerializationSettings settings)
        {
            byte[] result;
            using (MemoryStream stream = new MemoryStream())
            {
                await global::MessagePack.MessagePackSerializer.SerializeAsync(stream, message, this.formatterResolver);

                using (BinaryReader reader = new BinaryReader(stream))
                {
                    result = reader.ReadBytes((int)stream.Length);
                }
            }

            return result;
        }
    }
}
