using System.Runtime.CompilerServices;

namespace VMFramework.Containers
{
    public static class ContainerAddUtility
    {
        /// <inheritdoc cref="IContainer.TryMergeItem(int,IContainerItem,int,out int,ContainerMergeHint,out bool)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryMergeItem<TContainer, TItem>(this TContainer container, int slotIndex, TItem item,
            out bool completelyMerged, int preferredCount = int.MaxValue)
            where TContainer : IContainer
            where TItem : IContainerItem
        {
            return container.TryMergeItem(slotIndex, item, preferredCount, mergedCount: out _,
                new ContainerMergeHint(forceMergeWhenNonSplittable: true), out completelyMerged);
        }
    }
}