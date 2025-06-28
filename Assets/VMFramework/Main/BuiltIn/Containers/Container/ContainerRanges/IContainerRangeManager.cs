using System.Collections.Generic;
using VMFramework.Core;

namespace VMFramework.Containers
{
    public interface IContainerRangeManager
    {
        public bool TryGetAddableRanges(out IEnumerable<RangeInteger> ranges);

        public bool TryGetSortableRanges(out IEnumerable<RangeInteger> ranges);
    }
}