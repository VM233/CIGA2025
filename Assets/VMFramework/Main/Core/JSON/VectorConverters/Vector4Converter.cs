using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace VMFramework.Core.JSON
{
    public sealed class Vector4Converter : JsonConverter<Vector4>
    {
        public override void WriteJson(JsonWriter writer, Vector4 value, JsonSerializer serializer)
        {
            var token = new JObject()
            {
                { "x", value.x },
                { "y", value.y },
                { "z", value.z },
                { "w", value.w }
            };
            
            token.WriteTo(writer);
        }

        public override Vector4 ReadJson(JsonReader reader, Type objectType, Vector4 existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            var value = hasExistingValue ? existingValue : default;
            
            var token = JObject.Load(reader);
            
            if (token.TryGetValue("x", out var xToken))
            {
                value.x = xToken.Value<float>();
            }

            if (token.TryGetValue("y", out var yToken))
            {
                value.y = yToken.Value<float>();
            }
            
            if (token.TryGetValue("z", out var zToken))
            {
                value.z = zToken.Value<float>();
            }
            
            if (token.TryGetValue("w", out var wToken))
            {
                value.w = wToken.Value<float>();    
            }
            
            return value;
        }
    }
}