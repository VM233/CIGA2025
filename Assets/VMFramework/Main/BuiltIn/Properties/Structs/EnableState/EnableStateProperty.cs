using System;
using Sirenix.OdinInspector;

namespace VMFramework.Properties
{
    public class EnableStateProperty : IEnableStateProperty
    {
        [ShowInInspector]
        public bool IsEnabled { get; protected set; }

        protected Action<bool> enabledChangedFunc;

        public EnableStateProperty(Action<bool> enabledChangedFunc)
        {
            this.enabledChangedFunc = enabledChangedFunc;
        }

        [Button]
        public virtual void SetEnabled(bool isEnabled)
        {
            if (IsEnabled == isEnabled)
            {
                return;
            }
            
            enabledChangedFunc?.Invoke(isEnabled);
            
            IsEnabled = isEnabled;
        }
    }
}