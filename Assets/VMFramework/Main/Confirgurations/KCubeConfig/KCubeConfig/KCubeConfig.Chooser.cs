using System;
using System.Collections.Generic;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    public partial class KCubeConfig<TPoint> : IRangeChooserConfig<TPoint>, IWrapperChooserConfig<TPoint, TPoint>
    {
        void IAvailableItemsProvider<TPoint>.GetAvailableItems(ICollection<TPoint> items)
        {
            items.Add(min);
            items.Add(max);
        }

        IEnumerable<TPoint> IWrapperChooserConfig<TPoint, TPoint>.GetAvailableWrappers()
        {
            yield return min;
            yield return max;
        }

        void IWrapperChooserConfig<TPoint, TPoint>.SetAvailableValues(Func<TPoint, TPoint> setter)
        {
            min = setter(min);
            max = setter(max);
        }

        IChooser<TPoint> IChooserGenerator<TPoint>.GenerateNewChooser() => this;
    }
}