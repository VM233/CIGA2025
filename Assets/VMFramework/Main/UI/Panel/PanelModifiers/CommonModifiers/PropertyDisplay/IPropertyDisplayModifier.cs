using System.Collections.Generic;
using UnityEngine.UIElements;

namespace VMFramework.UI
{
    public interface IPropertyDisplayModifier : IPanelModifier
    {
        public IEnumerable<object> PropertySources { get; }
        
        public IReadOnlyList<(PropertyDisplayInfo info, VisualElement element)> AllPropertyElements { get; }
        
        public bool TryGetElementsByPropertySource(object propertySource, out IReadOnlyList<VisualElement> elements);
        
        public void AddProperty(PropertyDisplayInfo propertyConfig);

        public void Clear();
    }
}