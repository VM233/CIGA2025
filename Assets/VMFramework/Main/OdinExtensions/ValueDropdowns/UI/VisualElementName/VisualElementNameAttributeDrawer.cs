#if UNITY_EDITOR && ODIN_INSPECTOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.UI;

namespace VMFramework.OdinExtensions
{
    internal sealed class VisualElementNameAttributeDrawer
        : GeneralValueDropdownAttributeDrawer<VisualElementNameAttribute>
    {
        protected override void Validate()
        {
            base.Validate();

            foreach (var parent in Property.TraverseToRoot(false, property => property.Parent))
            {
                var value = parent?.ValueEntry?.WeakSmartValue;

                if (value is IVisualTreeAssetProvider)
                {
                    return;
                }

                if (parent.ParentValues.IsNullOrEmpty() == false)
                {
                    var parentValue = parent.ParentValues.First();

                    if (parentValue is Component component)
                    {
                        if (component.transform.QueryFirstComponentInParents<UIDocument>(true) != null)
                        {
                            return;
                        }
                    }
                }
            }

            SirenixEditorGUI.ErrorMessageBox(
                $"The property {Property.Name} is not a child of a {nameof(IVisualTreeAssetProvider)}.");
        }

        protected override IEnumerable<ValueDropdownItem> GetValues()
        {
            foreach (var parent in Property.TraverseToRoot(false, property => property.Parent))
            {
                var value = parent?.ValueEntry?.WeakSmartValue;
                
                VisualTreeAsset visualTree = null;

                if (value is IVisualTreeAssetProvider provider)
                {
                    visualTree = provider.VisualTree;
                }
                else if (parent.ParentValues.IsNullOrEmpty() == false)
                {
                    var parentValue = parent.ParentValues.First();

                    if (parentValue is Component component)
                    {
                        var uiDocument = component.transform.QueryFirstComponentInParents<UIDocument>(true);

                        if (uiDocument)
                        {
                            visualTree = uiDocument.visualTreeAsset;
                        }
                    }
                }

                if (visualTree == null)
                {
                    continue;
                }

                return visualTree.GetAllNamesByTypes(Attribute.VisualElementTypes)
                    .Where(name => name.IsLowercaseAndHyphenOnly() == false).ToValueDropdownItems();
            }

            return Enumerable.Empty<ValueDropdownItem>();
        }
    }
}
#endif