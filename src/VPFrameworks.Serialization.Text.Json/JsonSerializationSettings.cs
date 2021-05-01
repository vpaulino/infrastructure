using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using VPFrameworks.Serialization.Abstractions;

namespace Serialization.Text.Json
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonSerializationSettings : SerializationSettings
    { 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        public JsonSerializationSettings(JsonSerializerSettings settings) : base("application/json", Encoding.UTF8)
        {
            this.NewtonSoftSettings = settings;
        }

        /// <summary>
        /// 
        /// </summary>
        public JsonSerializerSettings NewtonSoftSettings { get; set; }

    }
}