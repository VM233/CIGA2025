using System;
using System.Diagnostics;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ActionResolvers;
using Sirenix.Utilities.Editor;
using UnityEditor;
#endif

namespace VMFramework.OdinExtensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class ListItemSelectorAttribute : Attribute
    {
        public string OnSelectMethod;
        public Color ColorOnSelect => new(R, G, B, A);
        public float R, G, B, A;

        public ListItemSelectorAttribute(string OnSelectMethod, float R = 0.3f, float G = 0.5f, float B = 1f,
            float A = 0.5f)
        {
            this.OnSelectMethod = OnSelectMethod;
            this.R = R;
            this.G = G;
            this.B = B;
            this.A = A;
        }
    }
}