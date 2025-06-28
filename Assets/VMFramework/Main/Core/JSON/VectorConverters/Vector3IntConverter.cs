using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace VMFramework.Core.JSON
{
    public sealed class Vector3IntConverter : JsonConverter<Vector3Int>
    {
        public override void WriteJson(JsonWriter writer, Vector3Int value, JsonSerializer serializer)
        {
            var token = new JObject()
            {
                { "x", value.x },
                { "y", value.y },
                { "z", value.z }
            };
            
            token.WriteTo(writer);
        }

        public override Vector3Int ReadJson(JsonReader reader, Type objectType, Vector3Int existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            var value = hasExistingValue ? existingValue : default;
            
            var token = JObject.Load(reader);
            
            if (token.TryGetValue("x", out var xToken))
            {
                value.x = xToken.Value<int>();
            }

            if (token.TryGetValue("y", out var yToken))
            {
                value.y = yToken.Value<int>();
            }
            
            if (token.TryGetValue("z", out var zToken))
            {
                value.z = zToken.Value<int>();
            }
            
            return value;
        }
    }
}