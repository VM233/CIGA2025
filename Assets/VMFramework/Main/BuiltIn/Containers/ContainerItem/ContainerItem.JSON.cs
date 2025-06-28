using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VMFramework.Core.JSON;

namespace VMFramework.Containers
{
    public partial class ContainerItem : IJSONSerializationReceiver
    {
        public virtual void SerializeTo(JObject o, JsonSerializer serializer)
        {
            o.Add(nameof(count), count.Value);
        }

        public virtual void DeserializeFrom(JObject o, JsonSerializer serializer)
        {
            if (o.TryGetValue(nameof(count), out JToken token))
            {
                count.Value = token.ToObject<int>();
            }
        }
    }
}