using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using VMFramework.Core;

namespace RoomPuzzle
{
    public class Door : MonoBehaviour
    {
        [Required]
        public Tilemap tilemap;
        
        [Required]
        public TileBase openedTile;
        
        [Required]
        public TileBase closedTile;

        public bool defaultIsOpen = false;
        
        public bool canClose = true;
        
        public bool canOpen = true;
        
        public bool IsOpen { get; protected set; }
        
        protected Interactable interactable;

        protected virtual void Awake()
        {
            interactable = GetComponent<Interactable>();
            interactable.OnInteract += OnInteract;
        }

        protected virtual void Start()
        {
            SetOpen(defaultIsOpen);
        }

        protected virtual void OnInteract(PlayerController player)
        {
            if (IsOpen)
            {
                if (canClose)
                {
                    SetOpen(false);
                }
            }
            else
            {
                if (canOpen)
                {
                    SetOpen(true);
                }
            }
        }

        public void SetOpen(bool isOpen)
        {
            IsOpen = isOpen;
            
            var position = tilemap.WorldToCell(transform.position).ReplaceZ(0);
            tilemap.SetTile(position, IsOpen? openedTile : closedTile);
        }
    }
}