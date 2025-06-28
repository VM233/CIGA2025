using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using VMFramework.Core.Pools;

namespace VMFramework.Containers
{
    public static class ContainerPopUtility
    {
        #region Try Pop Item By Slot Index

        /// <summary>
        /// 尝试弹出槽位上的物品，如果槽位无效或者物品为null，则返回false，其他情况返回true
        /// </summary>
        /// <param name="container"></param>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool TryPopItemBySlotIndex<TContainer, TItem>([NotNull] this TContainer container, int index,
            out TItem item)
            where TContainer : IContainer
            where TItem : IContainerItem
        {
            container.CheckIndex(index);
            item = (TItem)container.GetItem(index);

            if (item == null)
            {
                return false;
            }

            container.SetItem(index, null);

            return true;
        }

        /// <summary>
        /// 尝试弹出槽位上的物品到指定的容器，如果槽位无效或者物品为null，返回True
        /// 如果目标容器无法完全接收物品，则返回false，其他情况返回True
        /// </summary>
        /// <param name="container"></param>
        /// <param name="slotIndex"></param>
        /// <param name="targetContainer"></param>
        /// <param name="poppedCount"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPopItemBySlotIndexTo<TFromContainer, TToContainer>(
            [NotNull] this TFromContainer container, int slotIndex, [NotNull] TToContainer targetContainer,
            out int poppedCount)
            where TFromContainer : IContainer
            where TToContainer : IContainer
        {
            var item = container.GetItem(slotIndex);
            var arguments = new ContainerAddArguments(item);
            return targetContainer.TryAddItem(arguments, out poppedCount);
        }

        /// <summary>
        /// 尝试弹出槽位上的物品到指定的容器的指定槽位，如果槽位无效或者物品为null，返回True
        /// 如果目标容器无法完全接收物品，则返回false，其他情况返回True
        /// </summary>
        /// <param name="container"></param>
        /// <param name="slotIndex"></param>
        /// <param name="targetContainer"></param>
        /// <param name="targetIndex"></param>
        /// <returns></returns>
        public static bool TryPopItemBySlotIndexTo(this IContainer container, int slotIndex, IContainer targetContainer,
            int targetIndex)
        {
            var item = container.GetItem(slotIndex);

            if (item == null)
            {
                return true;
            }

            targetContainer.TryMergeItem(targetIndex, item, out var completelyMerged);

            return completelyMerged;
        }

        #endregion

        #region Try Pop First Valid Item

        /// <summary>
        /// 尝试弹出第一个有效的物品，如果没有有效物品，则返回false，其他情况返回true
        /// </summary>
        /// <param name="container"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPopFirstValidItem<TContainer, TItem>(this TContainer container, out TItem item)
            where TContainer : IContainer
            where TItem : IContainerItem
        {
            if (container.ValidSlotIndices.Count == 0)
            {
                item = default;
                return false;
            }

            var slotIndex = container.ValidSlotIndices.First();

            return container.TryPopItemBySlotIndex(slotIndex, out item);
        }

        /// <summary>
        /// 尝试弹出第一个有效的物品到指定的容器，如果没有有效物品，则返回false，
        /// 若目标容器无法完全接收物品，则返回false，其他情况返回true
        /// </summary>
        /// <param name="container"></param>
        /// <param name="targetContainer"></param>
        /// <param name="poppedCount"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPopFirstValidItemTo<TFromContainer, TToContainer>(this TFromContainer container,
            TToContainer targetContainer, out int poppedCount)
            where TFromContainer : IContainer
            where TToContainer : IContainer
        {
            if (container.ValidSlotIndices.Count == 0)
            {
                poppedCount = 0;
                return false;
            }

            var slotIndex = container.ValidSlotIndices.First();

            return container.TryPopItemBySlotIndexTo(slotIndex, targetContainer, out poppedCount);
        }

        #endregion

        #region Try Pop Item By Preferred Count

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPopItemByPreferredCountTo<TFromContainer, TToContainer>(
            [NotNull] this TFromContainer container, int slotIndex, int preferredCount,
            [NotNull] TToContainer targetContainer, int targetSlotIndex, out int poppedCount)
            where TFromContainer : IContainer
            where TToContainer : IContainer
        {
            container.CheckIndex(slotIndex);
            targetContainer.CheckIndex(targetSlotIndex);

            var currentItem = container.GetItem(slotIndex);

            if (currentItem == null)
            {
                poppedCount = 0;
                return false;
            }

            return targetContainer.TryMergeItem(targetSlotIndex, currentItem, preferredCount, out poppedCount,
                new ContainerMergeHint(forceMergeWhenNonSplittable: false), out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPopItemByPreferredCountTo<TFromContainer, TToContainer>(
            [NotNull] this TFromContainer container, int slotIndex, int preferredCount,
            [NotNull] TToContainer targetContainer, out int poppedCount)
            where TFromContainer : IContainer
            where TToContainer : IContainer
        {
            container.CheckIndex(slotIndex);
            var currentItem = container.GetItem(slotIndex);

            if (currentItem == null)
            {
                poppedCount = 0;
                return false;
            }

            var arguments = new ContainerAddArguments(currentItem, preferredCount);
            targetContainer.TryAddItem(arguments, out poppedCount);

            return poppedCount > 0;
        }

        /// <summary>
        /// 尝试弹出指定数量的物品，如果没有弹出任何物品，则会返回false，
        /// 如果有部分或全部物品被弹出，会返回true。
        /// </summary>
        /// <param name="container"></param>
        /// <param name="slotIndex"></param>
        /// <param name="preferredCount"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPopItemByPreferredCount<TContainer>([NotNull] this TContainer container,
            int slotIndex, int preferredCount, out IContainerItem item)
            where TContainer : IContainer
        {
            container.CheckIndex(slotIndex);
            var currentItem = container.GetItem(slotIndex);

            if (currentItem == null)
            {
                item = null;
                return false;
            }

            if (currentItem.IsSplittable(preferredCount, out _, out var shouldSplitToSelf) == false)
            {
                item = null;
                return false;
            }

            if (shouldSplitToSelf)
            {
                container.SetItem(slotIndex, null);
                item = currentItem;
                return true;
            }

            var splitResult = currentItem.Split(preferredCount);

            item = splitResult;
            return true;
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PopItemsTo(this IContainer container, int count, IContainer targetContainer,
            out int remainingCount)
        {
            remainingCount = count;

            foreach (var slotIndex in container.ValidSlotIndices)
            {
                if (container.TryPopItemByPreferredCountTo(slotIndex, remainingCount, targetContainer,
                        out var poppedCount) == false)
                {
                    continue;
                }

                remainingCount -= poppedCount;

                if (remainingCount <= 0)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 尝试将容器中的所有物品弹出到指定的容器中。
        /// 如果全部物品都弹出到目标容器中，则返回true，否则返回false。
        /// </summary>
        /// <param name="container"></param>
        /// <param name="targetContainer"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPopAllItemsTo(this IContainer container, IContainer targetContainer)
        {
            bool success = true;

            var slotIndices = ListPool<int>.Default.Get();
            slotIndices.Clear();

            slotIndices.AddRange(container.ValidSlotIndices);
            foreach (var slotIndex in slotIndices)
            {
                success &= container.TryPopItemBySlotIndexTo(slotIndex, targetContainer, out _);
            }

            slotIndices.ReturnToDefaultPool();

            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IContainerItem> PopAllItems<TContainer>(this TContainer container)
            where TContainer : IContainer
        {
            int count = container.Count;
            for (int i = 0; i < count; i++)
            {
                var item = container.GetItem(i);
                if (item == null)
                {
                    continue;
                }

                container.SetItem(i, null);
                yield return item;
            }
        }
    }
}