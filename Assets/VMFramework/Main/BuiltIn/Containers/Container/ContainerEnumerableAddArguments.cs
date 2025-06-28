using System.Collections.Generic;
using VMFramework.Core;

namespace VMFramework.Containers
{
    public struct ContainerEnumerableAddArguments
    {
        public IEnumerable<IContainerItem> items;
        public RangeInteger slotRange;
        public int? preferredCount;
        public ContainerMergeHint mergeHint;

        public ContainerEnumerableAddArguments(IEnumerable<IContainerItem> items, RangeInteger slotRange,
            int? preferredCount, ContainerMergeHint mergeHint)
        {
            this.items = items;
            this.slotRange = slotRange;
            this.preferredCount = preferredCount;
            this.mergeHint = mergeHint;
        }

        public ContainerEnumerableAddArguments(IEnumerable<IContainerItem> items)
        {
            this.items = items;
            this.slotRange = RangeInteger.Infinite;
            this.preferredCount = null;
            this.mergeHint = ContainerMergeHint.Default;
        }

        public ContainerEnumerableAddArguments(IEnumerable<IContainerItem> items, RangeInteger slotRange)
        {
            this.items = items;
            this.slotRange = slotRange;
            this.preferredCount = null;
            this.mergeHint = ContainerMergeHint.Default;
        }

        public ContainerAddArguments ToContainerAddArguments(IContainerItem item)
        {
            var preferredCount = this.preferredCount ?? int.MaxValue;
            return new ContainerAddArguments(item, slotRange, preferredCount, mergeHint);
        }
    }
}