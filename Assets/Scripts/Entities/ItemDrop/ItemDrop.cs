using Sirenix.OdinInspector;
using UnityEngine;

namespace RoomPuzzle
{
    public class ItemDrop : MonoBehaviour
    {
        public Item defaultItemPrefab;

        [MinValue(0)]
        public int count = 1;

        [ShowInInspector, HideInEditorMode]
        public Item CurrentItem { get; protected set; } = null;

        protected virtual void Start()
        {
            if (defaultItemPrefab != null)
            {
                var item = Instantiate(defaultItemPrefab);
                item.Count = count;
                SetItem(item);
            }
        }

        public void SetItem(Item item)
        {
            if (CurrentItem != null)
            {
                Destroy(CurrentItem.gameObject);
            }
            
            item.transform.SetParent(transform);
            
            CurrentItem = item;
        }
    }
}