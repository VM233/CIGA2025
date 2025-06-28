using System.Collections.Generic;
using VMFramework.Core;

namespace VMFramework.Containers
{
    public struct ContainerAddArguments
    {
        public IContainerItem item;
        
        /// <summary>
        /// 添加的槽位范围。
        /// 当<see cref="slotRanges"/>不为空时，此参数将被忽略。
        /// 当<see cref="slotRange"/>和<see cref="slotRanges"/>都为空时，
        /// 会被视为添加到<see cref="IContainer.RangeManager"/>
        /// 的<see cref="IContainerRangeManager.TryGetAddableRanges"/>所给的范围内（如果有的话）
        /// </summary>
        public RangeInteger? slotRange;
        
        /// <summary>
        /// 添加的槽位范围集合，当此参数不为空时，<see cref="slotRange"/>参数将被忽略。
        /// 当<see cref="slotRange"/>和<see cref="slotRanges"/>都为空时，
        /// 会被视为添加到<see cref="IContainer.RangeManager"/>
        /// 的<see cref="IContainerRangeManager.TryGetAddableRanges"/>所给的范围内（如果有的话）
        /// </summary>
        public IEnumerable<RangeInteger> slotRanges;
        
        /// <summary>
        /// 是否限制<see cref="slotRanges"/>在<see cref="slotRange"/>范围内
        /// </summary>
        public bool limitSlotRanges;

        /// <summary>
        /// 添加的最大数量，或者说预期的数量
        /// </summary>
        public int preferredCount;

        public ContainerMergeHint mergeHint;

        public ContainerAddArguments(IContainerItem item, RangeInteger slotRange, int preferredCount,
            ContainerMergeHint mergeHint)
        {
            this.item = item;
            this.slotRange = slotRange;
            this.preferredCount = preferredCount;
            this.mergeHint = mergeHint;
            this.slotRanges = null;
            this.limitSlotRanges = false;
        }

        public ContainerAddArguments(IContainerItem item)
        {
            this.item = item;
            this.slotRange = null;
            this.preferredCount = int.MaxValue;
            this.mergeHint = ContainerMergeHint.Default;
            this.slotRanges = null;
            this.limitSlotRanges = false;
        }

        public ContainerAddArguments(IContainerItem item, int preferredCount, RangeInteger? range = null)
        {
            this.item = item;
            this.slotRange = null;
            this.preferredCount = preferredCount;
            this.mergeHint = ContainerMergeHint.Default;
            this.slotRanges = null;
            this.limitSlotRanges = false;
        }

        public ContainerAddArguments(IContainerItem item, RangeInteger? range)
        {
            this.item = item;
            this.slotRange = range;
            this.preferredCount = int.MaxValue;
            this.mergeHint = ContainerMergeHint.Default;
            this.slotRanges = null;
            this.limitSlotRanges = false;
        }
    }
}