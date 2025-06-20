using System.Collections.Generic;
using UnityEngine;

namespace RoomPuzzle
{
    public class Interactable : MonoBehaviour
    {
        public delegate void InteractableCheckHandler(PlayerController player, ref bool canInteract,
            ICollection<string> invalidReasons);

        public delegate void InteractHandler(PlayerController player);

        public event InteractableCheckHandler OnCheckInteractable;

        public event InteractHandler OnInteract;

        public bool CanInteract(PlayerController player, ICollection<string> invalidReasons)
        {
            bool canInteract = true;
            OnCheckInteractable?.Invoke(player, ref canInteract, invalidReasons);
            return canInteract;
        }

        public void Interact(PlayerController player)
        {
            if (CanInteract(player, null) == false)
            {
                return;
            }

            OnInteract?.Invoke(player);
        }
    }
}