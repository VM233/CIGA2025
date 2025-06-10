#if UNITY_EDITOR
using System.Collections;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.OdinExtensions
{
    [DrawerPriority(DrawerPriorityLevel.WrapperPriority)]
    internal sealed class IsNotNullOrEmptyAttributeDrawer : SingleValidationAttributeDrawer<IsNotNullOrEmptyAttribute>
    {
        protected override bool Validate(object value)
        {
            bool isNullOrEmpty = value.IsUnityNull();

            if (isNullOrEmpty)
            {
                return false;
            }
            
            if (value is string stringValue)
            {
                if (Attribute.Trim)
                {
                    if (stringValue.IsEmptyOrWhiteSpace())
                    {
                        isNullOrEmpty = true;
                    }
                }
                else
                {
                    if (stringValue.IsEmpty())
                    {
                        isNullOrEmpty = true;
                    }
                }
            }
            else if (value is ICollection collectionValue)
            {
                if (collectionValue.Count == 0)
                {
                    isNullOrEmpty = true;
                }
            }
            else if (value is IEmptyCheckable emptyCheckable)
            {
                if (emptyCheckable.IsEmpty())
                {
                    isNullOrEmpty = true;
                }
            }

            return isNullOrEmpty == false;
        }

        protected override string GetDefaultMessage(GUIContent label, object value)
        {
            var propertyName = label == null ? Property.Name : label.text;

            return $"{propertyName} cannot be null or empty.";
        }
    }
}
#endif