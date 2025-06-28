using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VMFramework.Core.JSON
{
    public interface IJSONSerializationReceiver : IController
    {
        public void SerializeTo(JObject o, JsonSerializer serializer);
        
        public void DeserializeFrom(JObject o, JsonSerializer serializer);
    }
}