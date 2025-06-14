﻿using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ActionResolvers;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace VMFramework.OdinExtensions
{
    [DrawerPriority(0.01, 0, 0)]
    internal sealed class ListItemSelectorAttributeDrawer : OdinAttributeDrawer<ListItemSelectorAttribute>
    {
        private static readonly NamedValue[] onSelectMethodArgs =
        {
            new("index", typeof(int))
        };

        private bool isListElement;
        private InspectorProperty baseMemberProperty;
        private PropertyContext<InspectorProperty> globalSelectedProperty;
        private InspectorProperty selectedProperty;
        private ActionResolver onSelect;

        protected override void Initialize()
        {
            isListElement = Property.Parent is { ChildResolver: IOrderedCollectionResolver };
            var isList = !isListElement;
            var listProperty = isList ? Property : Property.Parent;
            baseMemberProperty = listProperty.FindParent(x => x.Info.PropertyType == PropertyType.Value, true);
            globalSelectedProperty = baseMemberProperty.Context.GetGlobal(
                "selectedIndex" + baseMemberProperty.GetHashCode(), (InspectorProperty)null);

            onSelect = ActionResolver.Get(Property, Attribute.OnSelectMethod, onSelectMethodArgs);
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var t = Event.current.type;

            if (onSelect == null)
            {
                CallNextDrawer(label);
                return;
            }

            if (onSelect.HasError)
            {
                onSelect.DrawError();
                CallNextDrawer(label);
                return;
            }

            if (isListElement)
            {
                if (t == EventType.Layout)
                {
                    CallNextDrawer(label);
                }
                else
                {
                    var rect = GUIHelper.GetCurrentLayoutRect();
                    var isSelected = globalSelectedProperty.Value == Property;

                    if (t == EventType.Repaint && isSelected)
                    {
                        EditorGUI.DrawRect(rect, Attribute.ColorOnSelect);
                    }
                    else if (t == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
                    {
                        globalSelectedProperty.Value = Property;
                    }

                    CallNextDrawer(label);
                }
            }
            else
            {
                CallNextDrawer(label);

                if (Event.current.type != EventType.Layout)
                {
                    var sel = globalSelectedProperty.Value;

                    // Select
                    if (sel != null && sel != selectedProperty)
                    {
                        selectedProperty = sel;
                        Select(selectedProperty.Index);
                    }
                    // Deselect when destroyed
                    else if (selectedProperty != null && selectedProperty.Index < Property.Children.Count &&
                             selectedProperty != Property.Children[selectedProperty.Index])
                    {
                        var index = -1;
                        Select(index);
                        selectedProperty = null;
                        globalSelectedProperty.Value = null;
                    }
                }
            }
        }

        private void Select(int index)
        {
            GUIHelper.RequestRepaint();

            onSelect.Context.NamedValues.Set(nameof(index), index);
            onSelect.DoAction();
        }
    }
}