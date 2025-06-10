#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Utilities.Editor;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.OdinExtensions
{
    internal sealed class MaximumAttributeDrawer : SingleValidationAttributeDrawer<MaximumAttribute>
    {
        private ValueResolver<double> maxValueGetter;

        private bool isProvider;
        private bool canClamp;

        protected override void Initialize()
        {
            maxValueGetter = ValueResolver.Get(Property, Attribute.MaxExpression, Attribute.MaxValue);
        }

        protected override bool Validate(object value)
        {
            if (value is null)
            {
                return true;
            }
            
            if (value is not IMaximumValueProvider provider)
            {
                isProvider = false;
                return false;
            }
            
            isProvider = true;
            
            canClamp = provider.CanClampByMaximum;

            return canClamp;
        }

        protected override string GetDefaultMessage(GUIContent label, object value)
        {
            if (isProvider == false)
            {
                return $"{value.GetType().GetNiceName()} does not implement {typeof(IMaximumValueProvider)}";
            }

            if (canClamp == false)
            {
                return $"Cannot clamp {value.GetType().GetNiceName()} by maximum value.";
            }

            return string.Empty;
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (maxValueGetter.HasError)
            {
                SirenixEditorGUI.ErrorMessageBox(maxValueGetter.ErrorMessage);
                CallNextDrawer(label);
                return;
            }
            
            base.DrawPropertyLayout(label);
        }

        protected override void OnAfterValidate(GUIContent label, object value)
        {
            base.OnAfterValidate(label, value);
            
            if (IsValid)
            {
                var maximumValueProvider = (IMaximumValueProvider)value;
                
                double max = maxValueGetter.GetValue();

                maximumValueProvider.ClampByMaximum(max);
            }
        }
    }
}
#endif