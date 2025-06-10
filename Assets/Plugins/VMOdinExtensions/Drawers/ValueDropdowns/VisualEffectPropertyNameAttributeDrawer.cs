#if UNITY_EDITOR && ODIN_INSPECTOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;
using VMFramework.Core;
using VMFramework.Core.Linq;

namespace VMFramework.OdinExtensions
{
    public class VisualEffectPropertyNameAttributeDrawer
        : GeneralValueDropdownAttributeDrawer<VisualEffectPropertyNameAttribute>
    {
        protected readonly List<VFXExposedProperty> properties = new();

        protected override IEnumerable<ValueDropdownItem> GetValues()
        {
            foreach (var parent in Property.TraverseToRoot(false, property => property.Parent))
            {
                VisualEffect visualEffect = null;

                if (parent.ParentValues.IsNullOrEmpty() == false)
                {
                    var parentValue = parent.ParentValues.First();

                    if (parentValue is Component component)
                    {
                        visualEffect = component.transform.QueryFirstComponentInParents<VisualEffect>(true);
                    }
                }

                if (visualEffect == null)
                {
                    continue;
                }

                var visualEffectAsset = visualEffect.visualEffectAsset;

                if (visualEffectAsset == null)
                {
                    return Enumerable.Empty<ValueDropdownItem>();
                }

                properties.Clear();
                visualEffectAsset.GetExposedProperties(properties);

                if (properties.Count == 0)
                {
                    return Enumerable.Empty<ValueDropdownItem>();
                }

                return properties.Select(property => new ValueDropdownItem(property.name, property.name));
            }

            return Enumerable.Empty<ValueDropdownItem>();
        }
    }
}
#endif