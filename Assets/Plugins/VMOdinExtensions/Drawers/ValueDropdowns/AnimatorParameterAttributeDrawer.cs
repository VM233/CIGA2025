#if UNITY_EDITOR && ODIN_INSPECTOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Linq;

namespace VMFramework.OdinExtensions
{
    public class AnimatorParameterAttributeDrawer : GeneralValueDropdownAttributeDrawer<AnimatorParameterAttribute>
    {
        protected override IEnumerable<ValueDropdownItem> GetValues()
        {
            foreach (var parent in Property.TraverseToRoot(false, property => property.Parent))
            {
                Animator animator = null;

                if (parent.ParentValues.IsNullOrEmpty() == false)
                {
                    var parentValue = parent.ParentValues.First();

                    if (parentValue is Component component)
                    {
                        animator = component.transform.QueryFirstComponentInParents<Animator>(true);
                    }
                }

                if (animator == null)
                {
                    continue;
                }

                if (Attribute.ParameterType.HasValue)
                {
                    return animator.parameters.Where(p => p.type == Attribute.ParameterType.Value)
                        .Select(p => new ValueDropdownItem(p.name, p.name));
                }

                return animator.parameters.Select(p => new ValueDropdownItem(p.name, p.name));
            }

            return Enumerable.Empty<ValueDropdownItem>();
        }
    }
}
#endif