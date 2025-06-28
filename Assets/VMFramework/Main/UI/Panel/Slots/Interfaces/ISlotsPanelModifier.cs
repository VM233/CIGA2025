using System.Collections.Generic;

namespace VMFramework.UI
{
    public interface ISlotsPanelModifier
    {
        public delegate void OnSlotSourceChangedHandler(ISlotsPanelModifier modifier, SlotVisualElement slot);
        
        public delegate void OnSlotAddedHandler(ISlotsPanelModifier modifier, SlotVisualElement slot);
        
        public delegate void OnSlotRemovedHandler(ISlotsPanelModifier modifier, SlotVisualElement slot);
        
        public IReadOnlyCollection<SlotVisualElement> Slots { get; }
        
        public event OnSlotSourceChangedHandler OnSlotSourceChanged;
        
        public event OnSlotAddedHandler OnSlotAdded;
        
        public event OnSlotRemovedHandler OnSlotRemoved;
    }
}