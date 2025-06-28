using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using VMFramework.Configuration;
using VMFramework.Containers;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.Properties;

namespace VMFramework.UI
{
    public class UIToolkitContainerModifierBase
        : SingleBinderModifier<IContainer>, IContainerSlotsModifier, IReadOnlyContainerPropertyProvider, IRefreshable
    {
        [BoxGroup(CONFIGS_CATEGORY)]
        public List<ContainerSlotDistributorConfig> slotDistributorConfigs = new();

        public IReadOnlyProperty<IContainer> Container => bindTargetProperty;

        public override IFuncTargetsProcessor<object, object> DefaultProcessor => new ContainerBindProcessor();

        public event IRefreshable.RefreshHandler OnRefreshed;

        public event ISlotsPanelModifier.OnSlotAddedHandler OnSlotAdded;

        public event ISlotsPanelModifier.OnSlotRemovedHandler OnSlotRemoved;
        
        public event ISlotsPanelModifier.OnSlotSourceChangedHandler OnSlotSourceChanged;

        private bool refreshTag = false;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnPostCloseEvent += OnPostClose;
        }

        protected virtual void OnPostClose(IUIPanel panel)
        {
            ClearSlots();
        }

        protected virtual void Update()
        {
            if (refreshTag)
            {
                refreshTag = false;
                Refresh();
            }
        }

        protected virtual void OnRefresh()
        {
            foreach (var slot in Slots)
            {
                OnSlotSourceChanged?.Invoke(this, slot);
            }
        }

        public void Refresh()
        {
            OnRefresh();
            OnRefreshed?.Invoke(this);
        }

        #region Bind Container

        protected virtual void OnItemChanged(IContainer container, int slotIndex, IContainerItem item)
        {
            refreshTag = true;

            if (TryGetSlotsSet(slotIndex, out var slots))
            {
                foreach (var slot in slots)
                {
                    slot.Source = item;
                }
            }

            SetTooltip(slotIndex, item);
        }
        
        protected virtual void OnItemDirty(IContainer container, int index, IContainerItem item, bool local)
        {
            refreshTag = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateAllSlots()
        {
            if (BindTarget == null)
            {
                return;
            }

            foreach (var (slotIndex, item) in BindTarget.GetAllItems().Enumerate())
            {
                OnItemChanged(BindTarget, slotIndex, item);
            }
        }

        protected virtual void OnContainerSizeChanged(IContainer container, int currentSize)
        {
            if (BindTarget != null && Panel.IsOpened)
            {
                BuildSlots();
            }
        }

        protected override void ProcessTargetBind(IContainer container)
        {
            base.ProcessTargetBind(container);

            if (container != null)
            {
                container.OnAfterItemChangedEvent += OnItemChanged;
                container.OnItemDirtyEvent += OnItemDirty;
                container.OnSizeChangedEvent += OnContainerSizeChanged;

                BuildSlots();
            }
        }

        protected override void ProcessTargetUnbind(IContainer container)
        {
            base.ProcessTargetUnbind(container);

            if (container != null)
            {
                container.OnAfterItemChangedEvent -= OnItemChanged;
                container.OnItemDirtyEvent -= OnItemDirty;
                container.OnSizeChangedEvent -= OnContainerSizeChanged;
                
                ClearSlots();
            }
        }

        #endregion

        #region Slots

        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        private readonly Dictionary<SlotVisualElement, int> slotIndicesLookup = new();

        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        private readonly Dictionary<int, HashSet<SlotVisualElement>> slotsLookup = new();

        public IReadOnlyCollection<SlotVisualElement> Slots => slotIndicesLookup.Keys;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void BuildSlots()
        {
            ClearSlots();

            foreach (var distributorConfig in slotDistributorConfigs)
            {
                BuildSlots(distributorConfig);
            }

            UpdateAllSlots();

            Refresh();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ClearSlots()
        {
            foreach (var (slot, slotIndex) in slotIndicesLookup)
            {
                OnClearSlot(slotIndex, slot);
                OnSlotRemoved?.Invoke(this, slot);
            }

            slotIndicesLookup.Clear();
            slotsLookup.Clear();
        }

        protected virtual void OnClearSlot(int slotIndex, SlotVisualElement slot)
        {
            
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void BuildSlots(ContainerSlotDistributorConfig distributorConfig)
        {
            var parent = this.RootVisualElement()
                .QueryStrictly(distributorConfig.parentName, nameof(distributorConfig.parentName));

            int index = distributorConfig.StartIndex;
            int count = distributorConfig.Count;

            IEnumerable<SlotVisualElement> slots;

            if (distributorConfig.findMethod == SlotFindMethod.Default)
            {
                slots = parent.QueryAll<SlotVisualElement>();
            }
            else if (distributorConfig.findMethod == SlotFindMethod.ByName)
            {
                slots = parent.QueryAllByName<SlotVisualElement>(distributorConfig.slotName);
            }
            else
            {
                Debugger.LogError($"Unsupported find method: {distributorConfig.findMethod}!");
                return;
            }

            foreach (var slot in slots)
            {
                if (count <= 0)
                {
                    break;
                }

                SetSlot(index, slot);

                index++;
                count--;
            }

            if (BindTarget == null || distributorConfig.autoFill == false)
            {
                return;
            }

            var container = this.RootVisualElement()
                .QueryStrictly(distributorConfig.ContainerName, nameof(distributorConfig.ContainerName));

            for (int slotIndex = index; slotIndex < BindTarget.Count; slotIndex++)
            {
                var newSlot = new SlotVisualElement();

                container.Add(newSlot);

                SetSlot(slotIndex, newSlot);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void SetSlot(int slotIndex, SlotVisualElement slot)
        {
            if (slotIndicesLookup.TryAdd(slot, slotIndex) == false)
            {
                Debugger.LogWarning($"Slot with index: {slotIndex} already exists in slotIndicesLookup!");
                return;
            }

            var set = slotsLookup.GetValueOrAddNew(slotIndex);

            if (set.Add(slot) == false)
            {
                Debugger.LogWarning($"Slot with index: {slotIndex} already exists in slotsLookup!");
                return;
            }

            OnSetSlot(slotIndex, slot);

            OnSlotAdded?.Invoke(this, slot);
        }

        protected virtual void OnSetSlot(int slotIndex, SlotVisualElement slot)
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool TryGetSlotsSet(int slotIndex, out HashSet<SlotVisualElement> slots)
        {
            if (slotsLookup.TryGetValue(slotIndex, out slots))
            {
                return slots.Count > 0;
            }

            // Debugger.LogWarning($"Failed to find slots with index: {slotIndex} in {nameof(slotsLookup)}!");

            return false;
        }

        public bool TryGetSlots(int slotIndex, out IReadOnlyCollection<SlotVisualElement> slots)
        {
            if (slotsLookup.TryGetValue(slotIndex, out var slotsSet))
            {
                slots = slotsSet;
                return slotsSet.Count > 0;
            }

            slots = null;
            return false;
        }

        public bool TryGetContainerAndIndex(SlotVisualElement slot, out IContainer container, out int slotIndex)
        {
            container = BindTarget;
            return slotIndicesLookup.TryGetValue(slot, out slotIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool TryGetSlotIndexWithWarning(SlotVisualElement slot, out int slotIndex)
        {
            if (slotIndicesLookup.TryGetValue(slot, out slotIndex))
            {
                return true;
            }

            Debugger.LogWarning($"Failed to find slot index for slot: {slot} in {nameof(slotIndicesLookup)}!");

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void SetTooltip(int slotIndex, object tooltipData)
        {
            if (TryGetSlotsSet(slotIndex, out var slots))
            {
                foreach (var slot in slots)
                {
                    slot.tooltipManager.SetTooltip(tooltipData);
                }
            }
        }

        #endregion
    }
}