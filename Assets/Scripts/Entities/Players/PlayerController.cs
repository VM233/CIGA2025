using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace RoomPuzzle
{
    public class PlayerController : MonoBehaviour, IStageElementProvider
    {
        [IsNotNullOrEmpty]
        public string moveInputName;

        [MinValue(0)]
        public float moveDuration = 0.4f;
        
        public FourTypesDirection FacingDirection { get; protected set; }

        public IStageElement StageElement { get; protected set; }
        
        public event Action<FourTypesDirection> OnFacingDirectionChanged;

        protected InputAction moveAction;

        protected virtual void Awake()
        {
            StageElement = GetComponent<IStageElement>();

            moveAction = InputSystem.actions.FindAction(moveInputName);
            moveAction.performed += OnMove;
        }

        protected virtual void OnMove(InputAction.CallbackContext context)
        {
            if (StageElement.Stage == null)
            {
                return;
            }

            if (StageElement.IsMoving())
            {
                return;
            }

            var moveValue = context.ReadValue<Vector2>();

            if (moveValue.x != 0 && moveValue.y != 0)
            {
                return;
            }

            var moveDirection = Vector2Int.zero;

            moveDirection.x = moveValue.x.Sign();
            moveDirection.y = moveValue.y.Sign();

            StageElement.Stage.MoveAndInteract(StageElement, new InteractHint()
            {
                moveHint = new MoveHint()
                {
                    duration = moveDuration,
                    direction = moveDirection
                }
            });

            FacingDirection = moveValue.ToFourTypesDirection2D();
            OnFacingDirectionChanged?.Invoke(FacingDirection);
        }
    }
}