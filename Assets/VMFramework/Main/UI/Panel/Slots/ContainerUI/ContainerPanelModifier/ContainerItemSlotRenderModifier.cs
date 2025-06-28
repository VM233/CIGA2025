using VMFramework.Containers;

namespace VMFramework.UI
{
    public class ContainerItemSlotRenderModifier : PanelModifier
    {
        protected ISlotsPanelModifier slotsModifier;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            slotsModifier = GetComponent<ISlotsPanelModifier>();
            slotsModifier.OnSlotSourceChanged += OnSlotSourceChanged;
        }

        protected virtual void OnSlotSourceChanged(ISlotsPanelModifier modifier, SlotVisualElement slot)
        {
            if (slot.Source == null)
            {
                SetNull(slot);
                return;
            }

            if (slot.Source is IContainerItem containerItem)
            {
                SetItem(slot, containerItem);
            }
        }

        protected virtual void SetItem(SlotVisualElement slot, IContainerItem containerItem)
        {
            if (containerItem.Count == 1)
            {
                slot.Description = string.Empty;
            }
            else
            {
                slot.Description = containerItem.Count.ToString();
            }

            if (containerItem is IIconOwner iconOwner)
            {
                slot.Icon = iconOwner.Icon;
            }
        }

        protected virtual void SetNull(SlotVisualElement slot)
        {
            slot.Icon = null;
            slot.Description = string.Empty;
        }
    }
}