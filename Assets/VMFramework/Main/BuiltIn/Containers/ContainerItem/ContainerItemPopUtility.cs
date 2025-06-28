using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.Containers
{
    public static class ContainerItemPopUtility
    {
        /// <summary>
        /// 如果分割的数量达到了目标数量，则返回true
        /// </summary>
        /// <param name="items"></param>
        /// <param name="targetCount"></param>
        /// <param name="results"></param>
        /// <param name="splitCount"></param>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TCollection"></typeparam>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool PopItems<TItem, TCollection>(this IEnumerable<TItem> items, int targetCount,
            TCollection results, out int splitCount)
            where TItem : IContainerItem
            where TCollection : ICollection<IContainerItem>
        {
            splitCount = 0;
            if (targetCount <= 0)
            {
                return true;
            }
            
            int leftToConsume = targetCount;

            foreach (var item in items)
            {
                if (item.IsSplittable(leftToConsume, out _, out _) == false)
                {
                    continue;
                }
                
                var result = item.Split(leftToConsume);
                var count = result.Count;
                leftToConsume -= count;
                splitCount += count;
                results.Add(result);

                if (leftToConsume <= 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 尝试移除希望数量的物品，如果完全移除，则返回true，否则返回false
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool RemoveItems<TItem>(this IEnumerable<TItem> items, int targetCount, out int removedCount)
            where TItem : IContainerItem
        {
            removedCount = 0;

            if (targetCount <= 0)
            {
                return true;
            }

            foreach (var item in items)
            {
                var fullyRemoved = item.Remove(targetCount, out var thisRemovedCount);
                
                removedCount += thisRemovedCount;
                
                if (fullyRemoved)
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}