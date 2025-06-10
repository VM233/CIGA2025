using System;
using System.Diagnostics;

namespace VMFramework.OdinExtensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
                    AttributeTargets.Parameter, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public sealed class ComponentRequiredAttribute : SingleValidationAttribute
    {
        public Type ComponentType;
        public string ComponentTypeGetter;

        public ComponentRequiredAttribute(string componentTypeGetter) : base()
        {
            ComponentTypeGetter = componentTypeGetter;
        }

        public ComponentRequiredAttribute(Type componentType) : base()
        {
            ComponentType = componentType;
        }

        public ComponentRequiredAttribute(Type componentType, string errorMessage) :
            base(errorMessage)
        {
            ComponentType = componentType;
        }
    }
}
