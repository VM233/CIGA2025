using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace VMFramework.Core.JSON
{
    public sealed class ColorConverter : JsonConverter<Color>
    {
        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            var token = new JObject()
            {
                {"r", value.r},
                {"g", value.g},
                {"b", value.b},
                {"a", value.a}
            };
            
            token.WriteTo(writer);
        }

        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            var value = hasExistingValue ? existingValue : default;
            
            var token = JObject.Load(reader);
            
            if (token.TryGetValue("r", out var rToken))
            {
                value.r = rToken.Value<float>();
            }
            
            if (token.TryGetValue("g", out var gToken))
            {
                value.g = gToken.Value<float>();
            }
            
            if (token.TryGetValue("b", out var bToken))
            {
                value.b = bToken.Value<float>();
            }
            
            if (token.TryGetValue("a", out var aToken))
            {
                value.a = aToken.Value<float>();
            }
            
            return value;
        }
    }
}