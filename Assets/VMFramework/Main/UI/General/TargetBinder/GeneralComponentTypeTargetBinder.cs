using System;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class GeneralComponentTypeTargetBinder<TTarget> : BaseConfig, ITargetBinder<TTarget>
    {
        [IsNotNullOrEmpty]
        public Type componentType;

        [DropdownLink]
        [IsNotNullOrEmpty]
        public TTarget target;

        public bool TryGetTarget(object source, out TTarget target)
        {
            target = default;

            if (source.TryGetComponent(componentType, out _) == false)
            {
                return false;
            }
            
            target = this.target;
            return true;
        }
    }
}