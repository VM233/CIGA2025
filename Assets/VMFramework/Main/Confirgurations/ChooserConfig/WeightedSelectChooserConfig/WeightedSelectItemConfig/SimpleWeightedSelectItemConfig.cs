using System;
using Sirenix.OdinInspector;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    [Serializable]
    public class SimpleWeightedSelectItemConfig<T> : BaseConfig, IWeightedSelectItem<T>
    {
        [HideLabel]
        public T value;

        [MinValue(0)]
        public int ratio = 1;
        
        T IWeightedSelectItem<T>.Value => value;

        float IWeightedSelectItem<T>.Weight => ratio;
    }
}