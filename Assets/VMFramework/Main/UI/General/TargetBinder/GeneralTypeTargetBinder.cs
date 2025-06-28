using System;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class GeneralTypeTargetBinder<TTarget> : BaseConfig, ITargetBinder<TTarget>
    {
        [IsNotNullOrEmpty]
        public Type type;
        
        [DropdownLink]
        [IsNotNullOrEmpty]
        public TTarget target;

        public bool TryGetTarget(object source, out TTarget target)
        {
            target = default;
            
            if (source is null)
            {
                return false;
            }
            
            if (type.IsValueType)
            {
                if (source.GetType() != type)
                {
                    return false;
                }
            }
            else if (source.GetType().IsDerivedFrom(type, true) == false)
            {
                return false;
            }
            
            target = this.target;
            return true;
        }
    }
}