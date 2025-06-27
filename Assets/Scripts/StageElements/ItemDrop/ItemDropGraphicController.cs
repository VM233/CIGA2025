using Sirenix.OdinInspector;
using UnityEngine;

namespace RoomPuzzle
{
    [ExecuteAlways]
    public class ItemDropGraphicController : MonoBehaviour
    {
        [Required]
        public SpriteRenderer spriteRenderer;

        [Required]
        public ItemDrop itemDrop;
        
        protected virtual void Update()
        {
            RefreshIcon();
        }

        protected virtual void RefreshIcon()
        {
            if (itemDrop.CurrentItem != null)
            {
                spriteRenderer.sprite = itemDrop.CurrentItem.icon;
                return;
            }

            if (itemDrop.defaultItemPrefab != null)
            {
                spriteRenderer.sprite = itemDrop.defaultItemPrefab.icon;
            }
        }
    }
}