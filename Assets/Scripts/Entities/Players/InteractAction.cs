using EnumsNET;
using UnityEngine;
using UnityEngine.InputSystem;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace RoomPuzzle
{
    public class InteractAction : MonoBehaviour
    {
        [IsNotNullOrEmpty]
        public string inputName;

        protected InputAction inputAction;
        protected IStageElement stageElement;
        protected PlayerController playerController;

        protected virtual void Awake()
        {
            inputAction = InputSystem.actions.FindAction(inputName);
            inputAction.performed += OnInteract;

            stageElement = GetComponent<IStageElement>();
            playerController = GetComponent<PlayerController>();
        }

        protected virtual void OnInteract(InputAction.CallbackContext context)
        {
            var facingDirection = playerController.FacingDirection;

            Vector2Int? priorityPosition = null;

            if (facingDirection != FourTypesDirection.None)
            {
                priorityPosition = stageElement.Position + facingDirection.ToCardinalVector();
            }

            if (priorityPosition.HasValue)
            {
                if (stageElement.Stage.TryGetElements(priorityPosition.Value, out var elements))
                {
                    foreach (var element in elements)
                    {
                        if (element.Interact(stageElement, new InteractHint()
                            {
                                moveHint = new MoveHint()
                                {
                                    direction = facingDirection.ToCardinalVector(),
                                    duration = playerController.moveDuration
                                }
                            }, out var valid))
                        {
                            if (valid)
                            {
                                return;
                            }
                        }
                    }
                }
            }

            var otherDirections = facingDirection.Reversed();

            foreach (var otherDirection in otherDirections.GetFlags())
            {
                var position = stageElement.Position + otherDirection.ToCardinalVector();

                if (stageElement.Stage.TryGetElements(position, out var elements))
                {
                    foreach (var element in elements)
                    {
                        if (element.Interact(stageElement, new InteractHint()
                            {
                                moveHint = new MoveHint()
                                {
                                    direction = otherDirection.ToCardinalVector(),
                                    duration = playerController.moveDuration
                                }
                            }, out var valid))
                        {
                            if (valid)
                            {
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}