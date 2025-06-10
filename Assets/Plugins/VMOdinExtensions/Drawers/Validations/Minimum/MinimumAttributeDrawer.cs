#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Utilities.Editor;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.OdinExtensions
{
    internal sealed class MinimumAttributeDrawer : SingleValidationAttributeDrawer<MinimumAttribute>
    {
        private ValueResolver<double> minValueGetter;
        
        private bool isProvider;
        private bool canClamp;

        protected override void Initialize() =>
            minValueGetter = ValueResolver.Get(Property, Attribute.MinExpression,
                Attribute.MinValue);
        
        protected override bool Validate(object value)
        {
            if (value is null)
            {
                return true;
            }
            
            if (value is not IMinimumValueProvider provider)
            {
                isProvider = false;
                return false;
            }
            
            isProvider = true;
            
            canClamp = provider.CanClampByMinimum;

            return canClamp;
        }
        
        protected override string GetDefaultMessage(GUIContent label, object value)
        {
            if (isProvider == false)
            {
                return $"{value.GetType().GetNiceName()} does not implement {typeof(IMinimumValueProvider)}";
            }

            if (canClamp == false)
            {
                return $"Cannot clamp {value.GetType().GetNiceName()} by minimum value.";
            }

            return string.Empty;
        }
        
        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (minValueGetter.HasError)
            {
                SirenixEditorGUI.ErrorMessageBox(minValueGetter.ErrorMessage);
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
                var minimumValueProvider = (IMinimumValueProvider)value;
                
                double min = minValueGetter.GetValue();

                minimumValueProvider.ClampByMinimum(min);
            }
        }
    }
}
#endif