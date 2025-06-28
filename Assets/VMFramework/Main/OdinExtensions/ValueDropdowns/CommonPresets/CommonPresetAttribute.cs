using System;
using System.Diagnostics;
using VMFramework.Core;

namespace VMFramework.OdinExtensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    [Conditional("UNITY_EDITOR")]
    public sealed class CommonPresetAttribute : GeneralValueDropdownAttribute
    {
        public string Key;

        public CommonPresetAttribute()
        {
            
        }
        
        public CommonPresetAttribute(string key)
        {
            Key = key;

            if (key.IsNullOrEmpty())
            {
                throw new ArgumentException("Key cannot be null or empty.");
            }
        }
    }
}