using System;
using System.Diagnostics;
using Sirenix.OdinInspector;

namespace VMFramework.OdinExtensions
{
    [DontApplyToListElements]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    [Conditional("UNITY_EDITOR")]
    public sealed class EnabledIfHasFlagAttribute : Attribute
    {
        public string Condition;
        
        public Enum Flag;

        public bool Animate;
        
        public EnabledIfHasFlagAttribute(string condition, object flag, bool animate = false)
        {
            Condition = condition;
            Flag = (Enum)flag;
            Animate = animate;
        }
    }
}