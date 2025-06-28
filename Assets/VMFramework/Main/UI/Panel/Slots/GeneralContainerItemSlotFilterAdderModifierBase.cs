using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Containers;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class GeneralContainerItemSlotFilterAdderModifierBase : PanelModifier
    {
        [BoxGroup(CONFIGS_CATEGORY)]
        public bool autoAddSlotFilter = true;

        [BoxGroup(CONFIGS_CATEGORY)]
        [GamePrefabID(typeof(SlotFilterConfig))]
        [IsNotNullOrEmpty]
        public string filterID;

        private IContainerSlotsModifier slotsModifier;

        private EventCallback<PointerEnterEvent> onPointerEnterFunc;
        private EventCallback<PointerLeaveEvent> onPointerLeaveFunc;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            slotsModifier = GetComponent<IContainerSlotsModifier>();

            slotsModifier.OnSlotAdded+= OnSlotAdded;
            slotsModifier.OnSlotRemoved += OnSlotRemoved;

            onPointerEnterFunc = OnPointerEnter;
            onPointerLeaveFunc = OnPointerLeave;
        }

        protected virtual void OnSlotAdded(ISlotsPanelModifier modifier, SlotVisualElement slot)
        {
            slot.RegisterCallback(onPointerEnterFunc);
            slot.RegisterCallback(onPointerLeaveFunc);
        }

        protected virtual void OnSlotRemoved(ISlotsPanelModifier modifier, SlotVisualElement slot)
        {
            SlotGlobalFiltersManager.Instance.RemoveFilter(filterID, slot);
            slot.UnregisterCallback(onPointerEnterFunc);
            slot.UnregisterCallback(onPointerLeaveFunc);
        }

        protected virtual bool CanAddGeneralSlotFilter(SlotVisualElement slot, IContainer container, int slotIndex)
        {
            return true;
        }

        protected virtual void OnPointerEnter(PointerEnterEvent evt)
        {
            if (autoAddSlotFilter == false)
            {
                return;
            }

            var slot = (SlotVisualElement)evt.target;

            if (slotsModifier.TryGetContainerAndIndex(slot, out var container, out var slotIndex) == false)
            {
                return;
            }

            if (container == null)
            {
                return;
            }

            if (CanAddGeneralSlotFilter(slot, container, slotIndex) == false)
            {
                return;
            }

            var filter = new GeneralContainerItemSlotFilter(container, slotIndex);
            SlotGlobalFiltersManager.Instance.AddFilter(filterID, slot, filter);
        }

        protected virtual void OnPointerLeave(PointerLeaveEvent evt)
        {
            var slot = (SlotVisualElement)evt.target;
            SlotGlobalFiltersManager.Instance.RemoveFilter(filterID, slot);
        }
    }
}