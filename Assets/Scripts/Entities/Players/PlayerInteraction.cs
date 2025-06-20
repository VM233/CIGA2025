using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;

namespace RoomPuzzle
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Required]
        public PlayerController player;

        public KeyCode interactKey = KeyCode.E;

        public HashSet<Interactable> interactables = new();

        protected readonly List<Interactable> canInteract = new();

        protected virtual void Start()
        {
            interactables.Clear();
        }

        protected virtual void Update()
        {
            if (Input.GetKeyDown(interactKey))
            {
                if (interactables.Count > 0)
                {
                    canInteract.Clear();
                    canInteract.AddRange(interactables.Where(interactable =>
                        interactable.CanInteract(player, invalidReasons: null)));

                    if (canInteract.Count > 0)
                    {
                        var interactable = canInteract.SelectMin(interactable =>
                            Vector2.Distance(transform.position.XY(), interactable.transform.position.XY()));
                        interactable.Interact(player);
                    }
                }
            }
        }
    }
}