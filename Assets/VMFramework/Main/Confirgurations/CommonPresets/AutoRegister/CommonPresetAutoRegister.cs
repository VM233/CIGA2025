using System;
using System.Diagnostics;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    [Conditional("UNITY_EDITOR")]
    public sealed class CommonPresetAutoRegister : Attribute
    {
        public string Key;
        public Type ValueType;
        
        public CommonPresetAutoRegister(string key, Type valueType)
        {
            Key = key;
            ValueType = valueType;
            
            key.AssertIsNotNullOrEmpty(nameof(key));
            valueType.AssertIsNotNull(nameof(valueType));
        }
    }
}