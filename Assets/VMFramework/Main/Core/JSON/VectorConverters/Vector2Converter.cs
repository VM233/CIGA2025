using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace VMFramework.Core.JSON
{
    public sealed class Vector2Converter : JsonConverter<Vector2>
    {
        public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
        {
            var token = new JObject()
            {
                { "x", value.x },
                { "y", value.y }
            };
            
            token.WriteTo(writer);
        }

        public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue,
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
            
            return value;
        }
    }
}