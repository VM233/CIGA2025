#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace VMFramework.OdinExtensions
{
    [DrawerPriority(DrawerPriorityLevel.WrapperPriority)]
    public sealed class
        RangeSliderAttributeDrawer : OdinAttributeDrawer<RangeSliderAttribute>
    {
        private ValueResolver<float> minGetter;

        private ValueResolver<float> maxGetter;

        protected override void Initialize()
        {
            minGetter = ValueResolver.Get(Property, Attribute.MinValueGetter,
                Attribute.MinValue);
            maxGetter = ValueResolver.Get(Property, Attribute.MaxValueGetter,
                Attribute.MaxValue);
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (Property.ValueEntry.WeakSmartValue is not IRangeSliderValueProvider
                valueProvider)
            {
                SirenixEditorGUI.ErrorMessageBox(
                    $"{Property.Name} must implement {nameof(IRangeSliderValueProvider)}");

                CallNextDrawer(label);
                return;
            }

            Vector2 limits = new(minGetter.GetValue(), maxGetter.GetValue());
            EditorGUI.BeginChangeCheck();
            Vector2 smartValue = SirenixEditorFields.MinMaxSlider(label,
                new Vector2(valueProvider.Min, valueProvider.Max), limits,
                Attribute.ShowFields);
            if (EditorGUI.EndChangeCheck())
            {
                valueProvider.Min = smartValue.x;
                valueProvider.Max = smartValue.y;
            }
        }
    }
}
#endif