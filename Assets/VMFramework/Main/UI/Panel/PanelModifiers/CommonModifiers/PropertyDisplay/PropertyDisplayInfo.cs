using UnityEngine;
using VMFramework.Core;

namespace VMFramework.UI
{
    public struct PropertyDisplayInfo
    {
        public string attributeValue;
        public SingleOrReadOnlyList<Sprite> icon;
        public string groupName;
        public object source;

        public PropertyDisplayInfo(string attributeValue, SingleOrReadOnlyList<Sprite> icon, string groupName = null,
            object source = null)
        {
            this.attributeValue = attributeValue;
            this.icon = icon;
            this.groupName = groupName;
            this.source = source;
        }
    }
}