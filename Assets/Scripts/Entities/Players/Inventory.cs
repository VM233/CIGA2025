using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoomPuzzle
{
    public class Inventory : MonoBehaviour
    {
        [ShowInInspector]
        protected readonly Dictionary<string, Item> items = new();

        public void AddItem(Item item)
        {
            if (items.TryGetValue(item.itemID, out var existingItem))
            {
                existingItem.Count += item.Count;
                Destroy(item.gameObject);
            }
            else
            {
                items.Add(item.itemID, item);
                item.transform.SetParent(transform);
            }
        }

        public void RemoveItem(string itemID, int count)
        {
            if (items.TryGetValue(itemID, out var item))
            {
                item.Count -= count;

                if (item.Count <= 0)
                {
                    items.Remove(itemID);
                    Destroy(item.gameObject);
                }
            }
        }

        public bool HasItem(string itemID, int count)
        {
            if (items.TryGetValue(itemID, out var item))
            {
                return item.Count >= count;
            }

            return false;
        }
    }
}