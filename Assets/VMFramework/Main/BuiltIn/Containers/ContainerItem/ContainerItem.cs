using System;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.Core.JSON;
using VMFramework.GameLogicArchitecture;
using VMFramework.Properties;

namespace VMFramework.Containers
{
    public delegate void ContainerItemSourceChangedHandler(IContainer container, int slotIndex, IContainerItem item);

    public abstract partial class ContainerItem : ControllerGameItem, IContainerItem, IStateCloneable
    {
        #region Fields and Properties

        [ShowInInspector]
        public IContainer SourceContainer { get; private set; }

        [ShowInInspector]
        [DisableInEditorMode]
        public readonly SimpleProperty<int> count = new();

        public abstract int MaxStackCount { get; }

        public int SlotIndex { get; private set; }

        public ReadOnlySpan<IJSONSerializationReceiver> JSONSerializationReceivers => jsonSerializationReceivers;

        public event IDirtyable.DirtyHandler OnDirty;

        [ShowInInspector]
        private IJSONSerializationReceiver[] jsonSerializationReceivers;

        public event PropertyChangedHandler<int> OnCountChangedEvent
        {
            add => count.OnChanged += value;
            remove => count.OnChanged -= value;
        }

        IContainer IContainerItem.SourceContainer
        {
            get => SourceContainer;
            set => SourceContainer = value;
        }

        int IContainerItem.Count
        {
            get => count.Value;
            set => count.Value = value;
        }

        #endregion

        #region Add and Remove Events

        public event ContainerItemSourceChangedHandler OnAddedToContainerEvent;
        public event ContainerItemSourceChangedHandler OnRemovedFromContainerEvent;

        protected virtual void OnAddedToContainer(IContainer container, int slotIndex)
        {
            transform.SetParent(container.transform);
        }

        protected virtual void OnRemovedFromContainer(IContainer container)
        {

        }

        void IContainerItem.OnAddedToContainer(IContainer container, int slotIndex)
        {
            SlotIndex = slotIndex;
            OnAddedToContainer(container, slotIndex);
            OnAddedToContainerEvent?.Invoke(container, slotIndex, this);
        }

        void IContainerItem.OnRemovedFromContainer(IContainer container)
        {
            OnRemovedFromContainer(container);
            OnRemovedFromContainerEvent?.Invoke(container, SlotIndex, this);
        }

        #endregion

        #region Pool Events

        protected override void OnCreate()
        {
            base.OnCreate();

            count.SetOwner(this);

            count.OnChanged += OnCountChanged;

            foreach (var dirtyable in GetComponents<IDirtyable>())
            {
                if (ReferenceEquals(dirtyable, this))
                {
                    continue;
                }

                dirtyable.OnDirty += OnComponentDirty;
            }

            jsonSerializationReceivers = GetComponentsInChildren<IJSONSerializationReceiver>(includeInactive: true);
        }

        protected override void OnGet()
        {
            base.OnGet();

            count.SetValue(0, initial: true);
        }

        #endregion

        public event IContainerItem.MergeHandler OnMergeEvent;

        private void OnComponentDirty(IDirtyable dirtyable, bool local)
        {
            OnDirty?.Invoke(this, local);
        }

        private void OnCountChanged(object owner, int previous, int current, bool initial)
        {
            if (initial)
            {
                return;
            }

            if (current == previous)
            {
                return;
            }

            OnDirty?.Invoke(this, local: false);
        }

        public virtual bool IsMergeableWith(IContainerItem other)
        {
            if (other == null) return false;
            if (count.Value >= MaxStackCount) return false;

            return other.id == id;
        }

        public int MergeWith(IContainerItem other, int preferredCount = int.MaxValue)
        {
            if (OnMergeWith(other, preferredCount, out var mergedCount, out var thisItemFinalCount,
                    out var otherItemFinalCount) == false)
            {
                return 0;
            }

            OnMergeEvent?.Invoke(this, other, mergedCount);
            count.Value = thisItemFinalCount;
            other.Count = otherItemFinalCount;
            return mergedCount;
        }

        protected virtual bool OnMergeWith(IContainerItem other, int preferredCount, out int mergedCount,
            out int thisItemFinalCount, out int otherItemFinalCount)
        {
            if (other.Count == 0)
            {
                mergedCount = 0;
                thisItemFinalCount = 0;
                otherItemFinalCount = 0;
                return false;
            }

            int maxIncrease = MaxStackCount - count.Value;
            maxIncrease = maxIncrease.Min(preferredCount);

            if (maxIncrease > other.Count)
            {
                var otherCount = other.Count;
                thisItemFinalCount = count.Value + otherCount;
                otherItemFinalCount = 0;
                mergedCount = otherCount;
                return true;
            }

            thisItemFinalCount = count.Value + maxIncrease;
            otherItemFinalCount = other.Count - maxIncrease;
            mergedCount = maxIncrease;
            return true;
        }

        public virtual void CloneFrom(IStateCloneable stateCloneable)
        {
            var containerItem = (ContainerItem)stateCloneable;
            count.SetValue(containerItem.count.GetValue(), initial: true);
        }
    }
}