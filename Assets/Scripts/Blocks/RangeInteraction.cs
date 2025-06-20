using Sirenix.OdinInspector;
using UnityEngine;

namespace RoomPuzzle
{
    public class RangeInteraction : MonoBehaviour
    {
        [Required]
        public Interactable interactable;

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerInteraction playerInteraction) == false)
            {
                return;
            }

            playerInteraction.interactables.Add(interactable);
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerInteraction playerInteraction) == false)
            {
                return;
            }

            playerInteraction.interactables.Remove(interactable);
        }
    }
}