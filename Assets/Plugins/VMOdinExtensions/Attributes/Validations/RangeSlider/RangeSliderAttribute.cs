using System;
using System.Diagnostics;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Utilities.Editor;
using UnityEditor;
#endif

namespace VMFramework.OdinExtensions
{
    [AttributeUsage(AttributeTargets.All)]
    [Conditional("UNITY_EDITOR")]
    public class RangeSliderAttribute : Attribute
    {
        public float MinValue;

        public float MaxValue;

        public string MinValueGetter;

        public string MaxValueGetter;

        public bool ShowFields;

        #region Constructor

        public RangeSliderAttribute(float minValue, float maxValue,
            bool showFields = true)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            ShowFields = showFields;
        }

        public RangeSliderAttribute(string minValueGetter, string maxValueGetter,
            bool showFields = true)
        {
            MinValueGetter = minValueGetter;
            MaxValueGetter = maxValueGetter;
            ShowFields = showFields;
        }

        public RangeSliderAttribute(float minValue, string maxValueGetter,
            bool showFields = true)
        {
            MinValue = minValue;
            MaxValueGetter = maxValueGetter;
            ShowFields = showFields;
        }

        public RangeSliderAttribute(string minValueGetter, float maxValue,
            bool showFields = true)
        {
            MinValueGetter = minValueGetter;
            MaxValue = maxValue;
            ShowFields = showFields;
        }

        #endregion
    }
}
