using System;
using System.Diagnostics;
using UnityEngine;

namespace VMFramework.OdinExtensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    [Conditional("UNITY_EDITOR")]
    public class AnimatorParameterAttribute : GeneralValueDropdownAttribute
    {
        public AnimatorControllerParameterType? ParameterType;

        public AnimatorParameterAttribute()
        {
            ParameterType = null;
        }

        public AnimatorParameterAttribute(AnimatorControllerParameterType parameterType)
        {
            ParameterType = parameterType;
        }
    }
}