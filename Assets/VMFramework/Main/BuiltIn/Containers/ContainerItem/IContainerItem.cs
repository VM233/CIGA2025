using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.Properties;

namespace VMFramework.Containers
{
    public interface IContainerItem : IJSONSerializableControllerGameItem, IDirtyable
    {
        public delegate void MergeHandler(IContainerItem item, IContainerItem other, int count);
        
        public IContainer SourceContainer { get; set; }

        public int MaxStackCount { get; }

        public int Count { get; set; }
        
        public int SlotIndex { get; }

        public event PropertyChangedHandler<int> OnCountChangedEvent;
        public event ContainerItemSourceChangedHandler OnAddedToContainerEvent;
        public event ContainerItemSourceChangedHandler OnRemovedFromContainerEvent;
        
        public event MergeHandler OnMergeEvent;

        public bool IsMergeableWith(IContainerItem other);

        /// <summary>
        /// 将此物品与另一个物品合并。
        /// </summary>
        /// <param name="other"></param>
        /// <param name="preferredCount">希望合并的最大数量</param>
        /// <returns>实际合并的数量</returns>
        public int MergeWith(IContainerItem other, int preferredCount = int.MaxValue);

        /// <summary>
        /// 如果能拆分出物品，则返回true，即使拆分出来的数量小于目标数量。
        /// </summary>
        /// <param name="targetCount"></param>
        /// <param name="actualSplitCount"></param>
        /// <param name="shouldSplitToSelf">在分离时是否应该将自身分离出来而不是创建新的物品。</param>
        /// <returns></returns>
        public bool IsSplittable(int targetCount, out int actualSplitCount, out bool shouldSplitToSelf)
        {
            if (targetCount <= 0)
            {
                actualSplitCount = 0;
                shouldSplitToSelf = false;
                return false;
            }

            targetCount = targetCount.Min(MaxStackCount);

            if (targetCount >= Count)
            {
                actualSplitCount = Count;
                shouldSplitToSelf = true;
            }
            else
            {
                actualSplitCount = targetCount;
                shouldSplitToSelf = false;
            }
            
            return actualSplitCount > 0;
        }
        
        public IContainerItem Split(int targetCount)
        {
            var clone = this.GetClone();

            targetCount = targetCount.Min(Count).Min(MaxStackCount);

            clone.Count = targetCount;

            Count -= targetCount;

            return clone;
        }

        /// <summary>
        /// 如果能完全移除指定的数量，则返回true。
        /// </summary>
        public bool IsRemovable(int targetRemoveCount, out int actualRemoveCount)
        {
            var count = Count;

            if (targetRemoveCount > count)
            {
                actualRemoveCount = count;
                
                return false;
            }
            
            actualRemoveCount = targetRemoveCount;
            
            return true;
        }

        /// <summary>
        /// 如果能完全移除指定的数量，则返回true。
        /// </summary>
        public bool Remove(int targetRemoveCount, out int actualRemoveCount)
        {
            var count = Count;

            if (targetRemoveCount > count)
            {
                actualRemoveCount = count;
                Count = 0;
                
                return false;
            }
            
            actualRemoveCount = targetRemoveCount;
            Count -= actualRemoveCount;
            
            return true;
        }

        public void OnAddedToContainer(IContainer container, int slotIndex);

        public void OnRemovedFromContainer(IContainer container);
    }
}
