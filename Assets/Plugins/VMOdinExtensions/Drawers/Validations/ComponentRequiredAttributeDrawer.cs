#if UNITY_EDITOR
using System;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.OdinExtensions
{
    [DrawerPriority(DrawerPriorityLevel.WrapperPriority)]
    internal sealed class ComponentRequiredAttributeDrawer : 
        SingleValidationAttributeDrawer<ComponentRequiredAttribute>
    {
        private ValueResolver<Type> componentTypeGetter;
        private ValueResolver<string> errorMessageGetter;
        private bool isList;

        protected override void Initialize()
        {
            isList = Property.ChildResolver is ICollectionResolver;

            if (isList)
            {
                return;
            }
            
            if (Attribute.Message.IsNullOrEmpty() == false)
            {
                errorMessageGetter =
                    ValueResolver.GetForString(Property, Attribute.Message);
            }

            if (Attribute.ComponentTypeGetter.IsNullOrEmpty() == false)
            {
                componentTypeGetter =
                    ValueResolver.Get<Type>(Property, Attribute.ComponentTypeGetter);
            }
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (isList)
            {
                CallNextDrawer(label);
                return;
            }
            
            base.DrawPropertyLayout(label);
        }

        protected override string GetDefaultMessage(GUIContent label, object value)
        {
            var componentType =
                Attribute.ComponentType ?? componentTypeGetter?.GetValue();

            if (componentType == null)
            {
                return string.Empty;
            }

            var name = componentType.Name;

            return errorMessageGetter == null
                ? $"{name} is Required"
                : errorMessageGetter.ErrorMessage;
        }

        protected override bool Validate(object value)
        {
            var componentType =
                Attribute.ComponentType ?? componentTypeGetter?.GetValue();

            if (componentType == null)
            {
                return true;
            }

            if (value.IsUnityNull())
            {
                return false;
            }

            if (value is GameObject gameObject)
            {
                if (gameObject.HasComponent(componentType))
                {
                    return true;
                }

                return false;
            }

            if (value is Component component)
            {
                if (component.GetType().IsDerivedFrom(componentType, true))
                {
                    return true;
                }
                
                return false;
            }

            return value.GetType().IsDerivedFrom(componentType, true);
        }
    }
}
#endif