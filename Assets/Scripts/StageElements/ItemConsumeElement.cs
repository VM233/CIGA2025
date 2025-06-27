using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoomPuzzle
{
    public class ItemConsumeElement : MonoBehaviour
    {
        [AssetsOnly]
        [Required]
        public Item item;

        [MinValue(1)]
        public int count = 1;
        
        public bool consumeItem = true;

        protected IStageElement stageElement;

        protected virtual void Awake()
        {
            stageElement = GetComponent<IStageElement>();
            stageElement.OnCheckInteractable += OnCheckInteractable;
            stageElement.OnInteract += OnInteract;
        }

        protected virtual void OnCheckInteractable(IStageElement element, IStageElement from, InteractHint hint,
            ref bool canInteract)
        {
            if (from.TryGetComponent(out Inventory inventory) == false)
            {
                canInteract = false;
                return;
            }
            
            if (inventory.HasItem(item.itemID, count) == false)
            {
                canInteract = false;
            }
        }

        protected virtual void OnInteract(IStageElement element, IStageElement from, InteractHint hint)
        {
            if (from.TryGetComponent(out Inventory inventory) == false)
            {
                return;
            }

            if (consumeItem)
            {
                inventory.RemoveItem(item.itemID, count);
            }
        }
    }
}