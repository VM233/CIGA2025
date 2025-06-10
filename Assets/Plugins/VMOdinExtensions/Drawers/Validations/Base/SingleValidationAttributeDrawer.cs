#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Utilities.Editor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;

namespace VMFramework.OdinExtensions
{
    [DrawerPriority(DrawerPriorityLevel.WrapperPriority)]
    public abstract class SingleValidationAttributeDrawer<TAttribute> : OdinAttributeDrawer<TAttribute> 
        where TAttribute : SingleValidationAttribute
    {
        private ValueResolver<string> errorMessageGetter;

        protected override void Initialize()
        {
            if (Attribute.Message.IsNullOrEmpty() == false)
            {
                errorMessageGetter =
                    ValueResolver.GetForString(Property, Attribute.Message);
            }
        }
        
        protected bool IsValid { get; private set; }

        /// <returns>Is the value valid?</returns>
        protected abstract bool Validate(object value);

        protected abstract string GetDefaultMessage(GUIContent label, object value);

        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (errorMessageGetter is { HasError: true })
            {
                SirenixEditorGUI.ErrorMessageBox(errorMessageGetter
                    .ErrorMessage);
                CallNextDrawer(label);
                return;
            }

            var value = Property.ValueEntry.WeakSmartValue;

            IsValid = Validate(value);
            
            string errorMessage;
            if (errorMessageGetter != null)
            {
                errorMessage = errorMessageGetter.GetValue();
            }
            else
            {
                errorMessage = GetDefaultMessage(label, value);
            }

            if (IsValid == false)
            {
                SirenixEditorGUI.MessageBox(errorMessage, Attribute.ValidateType.ToMessageType());
            }

            OnAfterValidate(label, value);
            CallNextDrawer(label);

            if (IsValid == false)
            {
                Rect rect = GUILayoutUtility.GetLastRect();

                if (Attribute.DrawCurrentRect)
                {
                    rect = GUIHelper.GetCurrentLayoutRect();
                }

                if (Attribute.ValidateType == ValidateType.Error)
                {
                    rect.DrawErrorRect();
                }
                else if (Attribute.ValidateType == ValidateType.Warning)
                {
                    rect.DrawWarningRect();
                }
            }
        }

        protected virtual void OnAfterValidate(GUIContent label, object value)
        {
            
        }
    }
}
#endif