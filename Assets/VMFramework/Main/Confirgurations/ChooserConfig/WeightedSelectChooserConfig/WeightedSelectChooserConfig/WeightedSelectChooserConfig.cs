﻿using System.Collections.Generic;

namespace VMFramework.Configuration
{
    public class WeightedSelectChooserConfig<TItem> : GeneralWeightedSelectChooserConfig<TItem, TItem>, 
        IWeightedSelectChooserConfig<TItem>
        where TItem : new()
    {
        public WeightedSelectChooserConfig() : base()
        {
            
        }
        
        public WeightedSelectChooserConfig(IEnumerable<TItem> items) : base(items)
        {
            
        }

        protected override TItem UnboxWrapper(TItem wrapper)
        {
            return wrapper;
        }
    }

    public class WeightedSelectChooserConfig<TWrapper, TItem> : GeneralWeightedSelectChooserConfig<TWrapper, TItem>
        where TWrapper : IChooserWrapper<TItem>
    {
        protected override TItem UnboxWrapper(TWrapper wrapper)
        {
            return wrapper.UnboxWrapper();
        }
    }
}