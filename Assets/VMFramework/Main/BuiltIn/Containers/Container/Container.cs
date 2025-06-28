using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core;
using Sirenix.OdinInspector;
using VMFramework.Core.JSON;
using VMFramework.Core.Linq;
using VMFramework.Core.Pools;
using VMFramework.GameEvents;
using VMFramework.GameLogicArchitecture;
using VMFramework.Properties;
using VMFramework.Tools;

namespace VMFramework.Containers
{
    public partial class Container : ControllerGameItem, IContainer
    {
        protected IContainerConfig ContainerConfig => (IContainerConfig)GamePrefab;

        public IProperty<IContainerOwner> Owner => ownerProperty;

        public int Count => items.Count;

        public int? Capacity { get; private set; }

        public int ValidCount => validItemsLookup.Count;

        [ShowInInspector]
        public virtual bool IsFull => Capacity.HasValue && ValidCount >= Capacity;

        [ShowInInspector]
        public virtual bool IsEmpty => ValidCount == 0;

        public IReadOnlyCollection<int> ValidSlotIndices => validItemsLookup.Keys;

        public IReadOnlyCollection<IContainerItem> ValidItems => validItemsLookup.Values;

        public event ItemChangedHandler OnBeforeItemChangedEvent;
        public event ItemChangedHandler OnAfterItemChangedEvent;

        public event IContainer.ItemAddedHandler OnItemAddedEvent;
        public event IContainer.ItemRemovedHandler OnItemRemovedEvent;

        public event ItemDirtyHandler OnItemDirtyEvent;

        public event SizeChangedHandler OnSizeChangedEvent;

        [ShowInInspector]
        protected readonly SimpleProperty<IContainerOwner> ownerProperty = new();

        protected PropertyChangedHandler<int> itemCountChangedFunc;
        protected IDirtyable.DirtyHandler itemDirtyFunc;

        private readonly SortedDictionary<int, IContainerItem> validItemsLookup = new();

        [ShowInInspector]
        private List<IContainerItem> items = new();

        [ShowInInspector]
        protected ISlotFiltersManager FiltersManager { get; set; }

        public IContainerRangeManager RangeManager { get; protected set; }

        public ReadOnlySpan<IJSONSerializationReceiver> JSONSerializationReceivers => jsonSerializationReceivers;

        private IJSONSerializationReceiver[] jsonSerializationReceivers;

        #region Pool Events

        protected override void OnCreate()
        {
            base.OnCreate();

            ownerProperty.SetOwner(this);

            FiltersManager = GetComponent<ISlotFiltersManager>();
            RangeManager = GetComponent<IContainerRangeManager>();

            itemCountChangedFunc = OnItemCountChanged;
            itemDirtyFunc = OnItemDirty;

            Capacity = ContainerConfig.Capacity;

            if (Capacity.HasValue)
            {
                for (var i = items.Count; i < Capacity.Value; i++)
                {
                    items.Add(null);
                }
            }

            jsonSerializationReceivers = GetComponentsInChildren<IJSONSerializationReceiver>(includeInactive: true);
        }

        protected override void OnGet()
        {
            base.OnGet();

            using var containerCreateEvent = ContainerCreateEvent.Get();
            containerCreateEvent.SetContainer(this);
            containerCreateEvent.Propagate();
        }

        protected override void OnReturn()
        {
            base.OnReturn();

            if (IsDebugging)
            {
                Debugger.LogWarning($"{this} is Destroyed");
            }

            ClearAllItems();

            using var containerDestroyEvent = ContainerDestroyEvent.Get();
            containerDestroyEvent.SetContainer(this);
            containerDestroyEvent.Propagate();

            var transformParent =
                ContainerTransform.Get(BuiltInModulesSetting.ContainerGeneralSetting.transformContainerName);
            transform.SetParent(transformParent);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ReturnItems()
        {
            foreach (var item in items)
            {
                if (item == null)
                {
                    continue;
                }

                GameItemManager.Instance.Return(item);
            }
        }

        #endregion

        #region IContainerItem Event

        private void OnItemCountChanged(object owner, int previousCount, int currentCount, bool isInitial)
        {
            var item = (IContainerItem)owner;
            if (currentCount <= 0)
            {
                SetItem(item.SlotIndex, null);

                GameItemManager.Instance.Return(item);
            }
        }

        private void OnItemDirty(IDirtyable dirtyable, bool local)
        {
            var containerItem = (IContainerItem)dirtyable;
            OnItemDirtyEvent?.Invoke(this, containerItem.SlotIndex, containerItem, local);
        }

        protected virtual void OnItemAdded(int slotIndex, IContainerItem item)
        {
            item.SourceContainer = this;

            validItemsLookup.Add(slotIndex, item);

            item.OnCountChangedEvent += itemCountChangedFunc;
            item.OnDirty += itemDirtyFunc;

            item.OnAddedToContainer(this, slotIndex);
            
            OnItemAddedEvent?.Invoke(this, slotIndex, item);
        }

        protected virtual void OnItemRemoved(int slotIndex, IContainerItem item)
        {
            item.OnCountChangedEvent -= itemCountChangedFunc;
            item.OnDirty -= itemDirtyFunc;

            validItemsLookup.Remove(slotIndex);

            if (ReferenceEquals(item.SourceContainer, this))
            {
                item.SourceContainer = null;
            }

            item.OnRemovedFromContainer(this);

            OnItemRemovedEvent?.Invoke(this, slotIndex, item);
        }

        protected void OnBeforeItemChanged(int slotIndex, IContainerItem item)
        {
            OnBeforeItemChangedEvent?.Invoke(this, slotIndex, item);
        }

        protected void OnAfterItemChanged(int slotIndex, IContainerItem item)
        {
            OnAfterItemChangedEvent?.Invoke(this, slotIndex, item);
        }

        #endregion

        #region Size Event

        protected void OnCountChanged()
        {
            OnSizeChangedEvent?.Invoke(this, Count);

            if (IsDebugging)
            {
                Debugger.LogWarning($"This size of {this} has changed to {Count}");
            }
        }

        #endregion

        #region Query Items

        public void CheckIndex(int index)
        {
            if (index < 0 || index >= items.Count)
            {
                throw new IndexOutOfRangeException(
                    $"Index {index} is out of range [{0}, {items.Count - 1}] for container {this}.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IContainerItem GetItem(int index) => items[index];

        public IReadOnlyList<IContainerItem> GetAllItems() => items;

        #endregion

        #region Merge

        public virtual bool CanMergeItem(int slotIndex, IContainerItem newItem)
        {
            if (newItem == null)
            {
                return true;
            }

            if (newItem.Count <= 0)
            {
                return true;
            }

            var itemInContainer = GetItem(slotIndex);

            if (itemInContainer == null)
            {
                return IsMatchFilters(slotIndex, newItem, out _);
            }

            return itemInContainer.IsMergeableWith(newItem);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool IsMatchFilters(int slotIndex, IContainerItem item, out bool hasFilters)
        {
            if (FiltersManager == null)
            {
                hasFilters = false;
                return true;
            }

            return FiltersManager.IsMatch(slotIndex, item, out hasFilters);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool TryMergeItem(int slotIndex, IContainerItem newItem, int preferredCount, out int mergedCount,
            ContainerMergeHint hint, out bool completelyMerged)
        {
            if (newItem == null)
            {
                mergedCount = 0;
                completelyMerged = true;
                return true;
            }

            if (newItem.Count <= 0)
            {
                mergedCount = 0;
                completelyMerged = true;
                return true;
            }

            if (preferredCount <= 0)
            {
                mergedCount = 0;
                completelyMerged = false;
                return true;
            }

            var itemInContainer = GetItem(slotIndex);

            if (itemInContainer == null)
            {
                if (IsMatchFilters(slotIndex, newItem, out _) == false)
                {
                    mergedCount = 0;
                    completelyMerged = false;
                    return false;
                }

                if (newItem.IsSplittable(preferredCount, out _, out var shouldSplitToSelf) == false)
                {
                    if (hint.forceMergeWhenNonSplittable)
                    {
                        SetItem(slotIndex, newItem);
                        mergedCount = newItem.Count;
                        completelyMerged = true;
                        return true;
                    }

                    mergedCount = 0;
                    completelyMerged = false;
                    return false;
                }

                if (shouldSplitToSelf)
                {
                    SetItem(slotIndex, newItem);
                    mergedCount = newItem.Count;
                    completelyMerged = true;
                    return true;
                }

                var cloneItem = newItem.Split(preferredCount);
                SetItem(slotIndex, cloneItem);
                mergedCount = cloneItem.Count;
                completelyMerged = false;
                return true;
            }

            if (itemInContainer.IsMergeableWith(newItem) == false)
            {
                mergedCount = 0;
                completelyMerged = false;
                return false;
            }

            OnBeforeItemChanged(slotIndex, itemInContainer);

            mergedCount = itemInContainer.MergeWith(newItem, preferredCount);

            OnAfterItemChanged(slotIndex, itemInContainer);

            completelyMerged = mergedCount >= newItem.Count || mergedCount >= preferredCount;
            return true;
        }

        #endregion

        #region Set Item

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetNullAndReturn(int index)
        {
            var item = items[index];

            if (item == null)
            {
                return;
            }

            SetItem(index, null);
            GameItemManager.Instance.Return(item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetItem(int index, IContainerItem item)
        {
            var targetItem = item;

            if (targetItem != null)
            {
                if (targetItem.IsDestroyed)
                {
                    targetItem = null;
                }
                else if (targetItem.Count <= 0)
                {
                    GameItemManager.Instance.Return(targetItem);
                    targetItem = null;
                }
                else
                {
                    targetItem.SourceContainer?.SetItem(item.SlotIndex, null);
                }
            }

            var oldItem = items[index];

            items[index] = targetItem;

            if (oldItem != null)
            {
                OnItemRemoved(index, oldItem);
            }

            OnBeforeItemChanged(index, oldItem);

            if (targetItem != null)
            {
                OnItemAdded(index, targetItem);
            }

            OnAfterItemChanged(index, targetItem);
        }

        #endregion

        #region Add Item

        protected virtual bool TryAddItemSingleInternal(ContainerAddArguments arguments, out int addedCount)
        {
            var item = arguments.item;
            var (startIndex, endIndex) = arguments.slotRange ?? RangeInteger.Infinite;
            var preferredCount = arguments.preferredCount;
            var mergeHint = arguments.mergeHint;

            if (IsDebugging)
            {
                Debugger.LogWarning($"Trying to add {item} to {this} in slots range: [{startIndex}, {endIndex}]" +
                                    $"with preferred count: {preferredCount}");
            }

            addedCount = 0;

            if (startIndex > endIndex)
            {
                return false;
            }

            var clampedStartIndex = startIndex.ClampMin(0);
            var clampedEndIndex = endIndex.ClampMax(items.Count - 1);

            var addAllowedRange = ContainerConfig.AddAllowedRange;

            if (addAllowedRange.HasValue)
            {
                clampedStartIndex = clampedStartIndex.ClampMin(addAllowedRange.Value.min);
                clampedEndIndex = clampedEndIndex.ClampMax(addAllowedRange.Value.max);

                if (clampedStartIndex > clampedEndIndex)
                {
                    return false;
                }
            }

            int leftCount = preferredCount.Min(item.Count);

            if (FiltersManager is { AddToFilteredSlotFirst: true })
            {
                var filtersStartIndex = clampedStartIndex.Max(FiltersManager.MinIndex);
                var filtersEndIndex = clampedEndIndex.Min(FiltersManager.MaxIndex);

                for (var slotIndex = filtersStartIndex; slotIndex <= filtersEndIndex; slotIndex++)
                {
                    bool isMatch = IsMatchFilters(slotIndex, item, out var hasFilters);

                    if (hasFilters == false)
                    {
                        continue;
                    }

                    if (isMatch)
                    {
                        if (TryMergeItem(slotIndex, item, leftCount, out var mergedCount, mergeHint,
                                out var completelyMerged))
                        {
                            addedCount += mergedCount;

                            if (completelyMerged)
                            {
                                return true;
                            }

                            leftCount -= mergedCount;
                        }
                    }
                }
            }

            for (var slotIndex = clampedStartIndex; slotIndex <= clampedEndIndex; slotIndex++)
            {
                var itemInContainer = items[slotIndex];

                if (itemInContainer == null)
                {
                    continue;
                }

                if (TryMergeItem(slotIndex, item, leftCount, out var mergedCount, mergeHint, out var completelyMerged))
                {
                    addedCount += mergedCount;

                    if (completelyMerged)
                    {
                        return true;
                    }

                    leftCount -= mergedCount;
                }
            }

            for (var slotIndex = clampedStartIndex; slotIndex <= clampedEndIndex; slotIndex++)
            {
                var itemInContainer = items[slotIndex];

                if (itemInContainer != null)
                {
                    continue;
                }

                if (TryMergeItem(slotIndex, item, leftCount, out var mergedCount, mergeHint, out var completelyMerged))
                {
                    addedCount += mergedCount;

                    if (completelyMerged)
                    {
                        return true;
                    }

                    leftCount -= mergedCount;
                }
            }

            if (Capacity.HasValue)
            {
                return false;
            }

            var addTimes = 0;
            while (true)
            {
                if (items.Count >= endIndex)
                {
                    return false;
                }

                AddNull(count: 1);

                if (TryMergeItem(slotIndex: items.Count - 1, item, leftCount, out var mergedCount, mergeHint,
                        out var completelyMerged))
                {
                    addedCount += mergedCount;

                    if (completelyMerged)
                    {
                        return true;
                    }

                    leftCount -= mergedCount;
                }
                else
                {
                    return false;
                }

                addTimes++;

                if (addTimes >= 1000)
                {
                    throw new NotSupportedException(
                        $"Failed to add item {item} to {this}, exceeded maximum add times: {1000}");
                }
            }
        }

        protected virtual bool TryAddItemRangesInternal(ContainerAddArguments arguments, out int addedCount)
        {
            addedCount = 0;
            var preferredCount = arguments.preferredCount;

            foreach (var range in arguments.slotRanges)
            {
                var newArguments = arguments;
                newArguments.slotRange = range;
                newArguments.preferredCount = preferredCount;
                var fullyAdded = TryAddItemSingleInternal(newArguments, out var currentAddedCount);

                addedCount += currentAddedCount;
                preferredCount -= currentAddedCount;

                if (fullyAdded)
                {
                    return true;
                }
            }

            return false;
        }

        public virtual bool TryAddItem(ContainerAddArguments arguments, out int addedCount)
        {
            var item = arguments.item;

            if (item == null)
            {
                addedCount = 0;
                return true;
            }

            if (item.IsDestroyed)
            {
                throw new InvalidOperationException($"Cannot add destroyed item {item} to {this}.");
            }

            if (item.Count <= 0)
            {
                addedCount = 0;
                return true;
            }

            if (arguments.preferredCount <= 0)
            {
                addedCount = 0;
                return true;
            }

            if (arguments.slotRange == null && arguments.slotRanges == null)
            {
                if (RangeManager != null && RangeManager.TryGetAddableRanges(out var ranges))
                {
                    arguments.slotRanges = ranges;
                }
            }

            if (arguments.slotRanges != null)
            {
                if (arguments is { limitSlotRanges: true, slotRange: not null })
                {
                    var slotRange = arguments.slotRange.Value;

                    var validRanges = ListPool<RangeInteger>.Default.Get();
                    validRanges.Clear();

                    arguments.slotRanges.LimitTo(slotRange.min, slotRange.max, validRanges);
                    arguments.slotRanges = validRanges;

                    var fullyAdded = TryAddItemRangesInternal(arguments, out addedCount);

                    validRanges.ReturnToDefaultPool();

                    return fullyAdded;
                }

                return TryAddItemRangesInternal(arguments, out addedCount);
            }

            return TryAddItemSingleInternal(arguments, out addedCount);
        }

        #endregion

        #region Stack Items

        public virtual void StackItems()
        {
            if (RangeManager != null && RangeManager.TryGetSortableRanges(out var ranges))
            {
                foreach (var range in ranges)
                {
                    this.StackItems(range.min, range.max);
                }
            }
            else
            {
                this.StackItems(int.MinValue, int.MaxValue);
            }
        }

        #endregion

        #region Sort

        public virtual void Sort(Comparison<IContainerItem> comparison)
        {
            if (RangeManager != null && RangeManager.TryGetSortableRanges(out var ranges))
            {
                foreach (var range in ranges)
                {
                    Sort(comparison, range.min, range.max);
                }
            }
            else
            {
                Sort(comparison, int.MinValue, int.MaxValue);
            }
        }

        public virtual void Sort(Comparison<IContainerItem> comparison, int startIndex, int endIndex)
        {
            startIndex = startIndex.ClampMin(0);
            endIndex = endIndex.ClampMax(Count - 1);

            this.StackItems(startIndex, endIndex);

            var itemList = ListPool<IContainerItem>.Default.Get();
            itemList.Clear();
            itemList.AddRange(this.GetRangeItems(startIndex, endIndex));

            itemList.RemoveAllNull();

            itemList.Sort(comparison);

            for (var i = 0; i < itemList.Count; i++)
            {
                SetItem(startIndex + i, itemList[i]);
            }

            for (var i = endIndex; i >= startIndex + itemList.Count; i--)
            {
                SetItem(i, null);
            }

            itemList.ReturnToDefaultPool();
        }

        #endregion

        #region Compress Items

        public virtual void Compress()
        {
            Compress(int.MinValue, int.MaxValue);
        }

        /// <summary>
        /// 压缩容器，去除物品间的Null
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        public virtual void Compress(int startIndex, int endIndex)
        {
            startIndex = startIndex.ClampMin(0);
            endIndex = endIndex.ClampMax(Count - 1);

            var itemList = ListPool<IContainerItem>.Default.Get();
            itemList.Clear();
            itemList.AddRange(this.GetRangeItems(startIndex, endIndex));

            itemList.RemoveAllNull();

            for (var i = 0; i < itemList.Count; i++)
            {
                SetItem(startIndex + i, itemList[i]);
            }

            for (var i = endIndex; i >= startIndex + itemList.Count; i--)
            {
                items.RemoveAt(i);
            }

            itemList.ReturnToDefaultPool();
            OnCountChanged();
        }

        #endregion

        #region String

        protected override void OnGetStringProperties(
            ICollection<(string propertyID, string propertyContent)> collection)
        {
            base.OnGetStringProperties(collection);

            collection.Add((nameof(ValidCount), ValidCount.ToString()));
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void AddNull(int count)
        {
            for (var i = 0; i < count; i++)
            {
                items.Add(null);
            }

            OnCountChanged();
        }

        public void ExpandTo(int newCount)
        {
            if (newCount > Capacity)
            {
                Debugger.LogError(
                    $"Cannot expand container {this} to {newCount} because it has a capacity of {Capacity}");
                newCount = Capacity.Value;
            }

            if (newCount <= items.Count)
            {
                return;
            }

            for (var i = items.Count; i < newCount; i++)
            {
                items.Add(null);
            }

            OnCountChanged();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearAllItems()
        {
            for (var i = 0; i < items.Count; i++)
            {
                SetNullAndReturn(i);
            }

            if (Capacity.HasValue == false)
            {
                items.Clear();
                OnCountChanged();
            }
        }

        #region Array Operations

        public void LoadFromItemsList(IReadOnlyList<IContainerItem> itemsList, bool autoReturn, int count)
        {
            ClearAllItems();

            var sourceCount = itemsList.Count.Min(count);
            if (Capacity.HasValue)
            {
                var minLength = items.Count.Min(sourceCount);

                if (items.Count != sourceCount)
                {
                    Debugger.LogWarning($"Container {this} has a capacity of {Capacity}, " +
                                        $"but the source list has {sourceCount} items.");
                }

                for (int i = 0; i < minLength; i++)
                {
                    var otherItem = itemsList[i];
                    if (otherItem == null)
                    {
                        continue;
                    }

                    SetItem(i, itemsList[i]);
                }

                if (autoReturn)
                {
                    for (int i = minLength; i < sourceCount; i++)
                    {
                        var otherItem = itemsList[i];

                        if (otherItem == null)
                        {
                            continue;
                        }

                        GameItemManager.Instance.Return(otherItem);
                    }
                }
            }
            else
            {
                ExpandTo(sourceCount);
                OnCountChanged();

                for (var i = 0; i < sourceCount; i++)
                {
                    SetItem(i, itemsList[i]);
                }
            }
        }

        public void CopyAllItemsToArray(IContainerItem[] itemsArray)
        {
            for (var i = 0; i < items.Count; i++)
            {
                itemsArray[i] = items[i];
            }
        }

        #endregion

        public virtual void Shuffle()
        {
            var itemList = items.ToListDefaultPooled();

            itemList.Shuffle();

            foreach (var (slotIndex, item) in itemList.Enumerate())
            {
                SetItem(slotIndex, item);
            }

            itemList.ReturnToDefaultPool();
        }
    }
}