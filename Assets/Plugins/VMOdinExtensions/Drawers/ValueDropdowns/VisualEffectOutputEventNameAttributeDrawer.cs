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
    public class VisualEffectOutputEventNameAttributeDrawer
        : GeneralValueDropdownAttributeDrawer<VisualEffectOutputEventNameAttribute>
    {
        protected readonly List<string> names = new();
        
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
                
                names.Clear();
                visualEffect.GetOutputEventNames(names);

                if (names.Count == 0)
                {
                    return Enumerable.Empty<ValueDropdownItem>();
                }

                return names.Select(name => new ValueDropdownItem(name, name));
            }

            return Enumerable.Empty<ValueDropdownItem>();
        }
    }
}
#endif