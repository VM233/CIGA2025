using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VMFramework.Core.JSON;

namespace VMFramework.Containers
{
    public partial class Container : IJSONSerializationReceiver
    {
        public virtual void SerializeTo(JObject o, JsonSerializer serializer)
        {
            if (ValidCount > 0)
            {
                o.Add("items", JArray.FromObject(items, serializer));
            }
        }

        public virtual void DeserializeFrom(JObject o, JsonSerializer serializer)
        {
            if (o.TryGetValue("items", out JToken itemsToken))
            {
                var savedItems = itemsToken.ToObject<List<IContainerItem>>(serializer);
                LoadFromItemsList(savedItems, autoReturn: true, count: savedItems.Count);
            }
        }
    }
}