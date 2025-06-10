#if UNITY_EDITOR && ODIN_INSPECTOR
using VMFramework.Core;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using VMFramework.Core.Editor;

namespace VMFramework.OdinExtensions
{
    [DrawerPriority(DrawerPriorityLevel.SuperPriority)]
    internal sealed class StringContextMenuDrawer : OdinValueDrawer<string>, IDefinesGenericMenuItems
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            CallNextDrawer(label);
        }

        void IDefinesGenericMenuItems.PopulateGenericMenu(InspectorProperty property,
            GenericMenu genericMenu)
        {
            if (property.ValueEntry.WeakSmartValue is not string str)
            {
                return;
            }

            if (property.GetAttribute<ValueDropdownAttribute>() != null)
            {
                return;
            }

            if (str.IsEmptyOrWhiteSpace())
            {
                return;
            }
            
            if (property.Info.IsEditable)
            {
                genericMenu.AddSeparator();
                
                if (str.IsNullOrEmpty() == false)
                {
                    genericMenu.AddItem("Clear String", () =>
                    {
                        property.ValueEntry.WeakSmartValue = string.Empty;
                    });
                }

                genericMenu.AddItem("Pascal Case", () =>
                {
                    property.ValueEntry.WeakSmartValue = str.ToPascalCase(" ");
                });

                genericMenu.AddItem("Snake Case", () =>
                {
                    property.ValueEntry.WeakSmartValue = str.ToSnakeCase();
                });
            
                genericMenu.AddItem("Camel Case", () =>
                {
                    property.ValueEntry.WeakSmartValue = str.ToCamelCase();
                });

                if (str.Contains(' '))
                {
                    genericMenu.AddItem("Remove Spaces", () =>
                    {
                        property.ValueEntry.WeakSmartValue = str.Replace(" ", "");
                    });
                }
            }
        }
    }
}

#endif