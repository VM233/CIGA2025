using System;
using System.Collections.Generic;
using VMFramework.GameEvents;
using VMFramework.GameLogicArchitecture;
using VMFramework.Properties;

namespace VMFramework.Containers
{
    public delegate void ItemChangedHandler(IContainer container, int index, IContainerItem item);
    
    public delegate void ItemDirtyHandler(IContainer container, int index, IContainerItem item, bool local);

    public delegate void SizeChangedHandler(IContainer container, int currentSize);

    public partial interface IContainer : IJSONSerializableControllerGameItem, IReadOnlyCollection<IContainerItem>
    {
        public delegate void ItemAddedHandler(IContainer container, int index, IContainerItem item);

        public delegate void ItemRemovedHandler(IContainer container, int index, IContainerItem item);
        
        public IProperty<IContainerOwner> Owner { get; }

        public int? Capacity { get; }

        public int ValidCount { get; }

        public bool IsFull { get; }

        public IReadOnlyCollection<int> ValidSlotIndices { get; }

        public IReadOnlyCollection<IContainerItem> ValidItems { get; }
        
        public IContainerRangeManager RangeManager { get; }

        public event ItemChangedHandler OnBeforeItemChangedEvent;
        public event ItemChangedHandler OnAfterItemChangedEvent;
        public event ItemDirtyHandler OnItemDirtyEvent;
        public event SizeChangedHandler OnSizeChangedEvent;

        public event ItemAddedHandler OnItemAddedEvent;

        public event ItemRemovedHandler OnItemRemovedEvent;

        public void CheckIndex(int index);

        /// <summary>
        /// 获取指定索引处的<see cref="IContainerItem"/>。
        /// 如果索引越界，则会报错。
        /// 可以使用<see cref="CheckIndex"/>来检查索引是否越界。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IContainerItem GetItem(int index);

        public IReadOnlyList<IContainerItem> GetAllItems();

        public bool CanMergeItem(int slotIndex, IContainerItem newItem);

        public bool IsMatchFilters(int slotIndex, IContainerItem item, out bool hasFilters)
        {
            hasFilters = false;
            return true;
        }

        /// <summary>
        /// 尝试合并物品到容器中
        /// </summary>
        /// <param name="slotIndex"></param>
        /// <param name="newItem"></param>
        /// <param name="preferredCount">期望合并的数量</param>
        /// <param name="mergedCount">实际合并的数量</param>
        /// <param name="hint"></param>
        /// <param name="completelyMerged">是否完全合并</param>
        /// <returns>是否发生合并</returns>
        public bool TryMergeItem(int slotIndex, IContainerItem newItem, int preferredCount, out int mergedCount,
            ContainerMergeHint hint, out bool completelyMerged);

        public void SetItem(int index, IContainerItem item);

        /// <summary>
        /// 尝试添加物品到容器中, 如果物品完全添加到容器中，则返回true，否则返回false
        /// </summary>
        public bool TryAddItem(ContainerAddArguments arguments, out int addedCount);

        public void Sort(Comparison<IContainerItem> comparison);

        public void Sort(Comparison<IContainerItem> comparison, int startIndex, int endIndex);

        /// <summary>
        /// 堆叠物品，将相同物品的数量合并到一起
        /// </summary>
        public void StackItems();

        /// <summary>
        /// 压缩容器，去除物品间的Null
        /// </summary>
        public void Compress();

        /// <summary>
        /// 压缩容器，去除物品间的Null
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        public void Compress(int startIndex, int endIndex);

        public void ExpandTo(int newCount);

        public void ClearAllItems();

        public void CopyAllItemsToArray(IContainerItem[] itemsArray);

        public void LoadFromItemsList(IReadOnlyList<IContainerItem> itemsList, bool autoReturn, int count);

        public void Shuffle();
    }
}