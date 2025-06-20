using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoomPuzzle
{
    public class ItemConsumeInteraction : MonoBehaviour
    {
        [Required]
        public Item item;

        [MinValue(0)]
        public int count;

        public bool consumeOnUse = true;

        [MinValue(0)]
        public int consumeTimes = 1;

        public int LeftTimes { get; protected set; } = 0;

        protected Interactable interactable;

        protected virtual void Awake()
        {
            interactable = GetComponent<Interactable>();
            interactable.OnCheckInteractable += OnCheckInteractable;
            interactable.OnInteract += OnInteract;
        }

        protected virtual void Start()
        {
            LeftTimes = consumeTimes;
        }

        protected virtual void OnCheckInteractable(PlayerController player, ref bool canInteract,
            ICollection<string> invalidReasons)
        {
            if (LeftTimes > 0)
            {
                if (player.TryGetComponent(out Inventory inventory) == false)
                {
                    canInteract = false;
                    return;
                }

                if (inventory.HasItem(item.itemID, count) == false)
                {
                    canInteract = false;
                }
            }
        }

        protected virtual void OnInteract(PlayerController player)
        {
            if (LeftTimes > 0)
            {
                if (player.TryGetComponent(out Inventory inventory) == false)
                {
                    return;
                }

                if (consumeOnUse)
                {
                    inventory.RemoveItem(item.itemID, count);
                    LeftTimes--;
                }
            }
        }
    }
}