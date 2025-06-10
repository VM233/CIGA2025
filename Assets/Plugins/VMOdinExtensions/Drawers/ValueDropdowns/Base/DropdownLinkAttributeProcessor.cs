#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using VMFramework.Core;
using VMFramework.Core.Linq;

namespace VMFramework.OdinExtensions
{
    internal sealed class DropdownLinkAttributeProcessor : OdinAttributeProcessor
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            base.ProcessSelfAttributes(property, attributes);

            if (attributes.AnyIs(out DropdownLinkAttribute _) == false)
            {
                return;
            }

            foreach (var inspectorProperty in property.TraverseToRoot(false, property => property.Parent))
            {
                var generalValueDropdown = inspectorProperty.GetAttribute<GeneralValueDropdownAttribute>();

                if (generalValueDropdown == null)
                {
                    continue;
                }

                var clone = generalValueDropdown.Clone();
                clone.DisableDraw = false;
                
                attributes.Add(clone);
                break;
            }
        }
    }
}
#endif