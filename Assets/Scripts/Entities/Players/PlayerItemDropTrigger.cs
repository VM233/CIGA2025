using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoomPuzzle
{
    public class PlayerItemDropTrigger : MonoBehaviour
    {
        [Required]
        public Inventory inventory;

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out ItemDrop itemDrop) == false)
            {
                return;
            }

            if (itemDrop.CurrentItem == null)
            {
                return;
            }

            inventory.AddItem(itemDrop.CurrentItem);
            Destroy(itemDrop.gameObject);
        }
    }
}