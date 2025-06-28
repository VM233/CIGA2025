using System;
using System.Collections.Generic;
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
        
        [IsNotNullOrEmpty]
        public string continuousMoveInputName;

        [MinValue(0)]
        public float moveDuration = 0.4f;
        
        [ShowInInspector]
        public FourTypesDirection FacingDirection { get; protected set; }

        public IStageElement StageElement { get; protected set; }
        
        public event Action<FourTypesDirection> OnFacingDirectionChanged;

        protected InputAction moveAction;
        protected InputAction continuousMoveAction;
        
        protected bool isContinuousMove;

        protected virtual void Awake()
        {
            StageElement = GetComponent<IStageElement>();

            moveAction = InputSystem.actions.FindAction(moveInputName);
            moveAction.performed += OnMove;
            
            continuousMoveAction = InputSystem.actions.FindAction(continuousMoveInputName);
            continuousMoveAction.performed += OnContinuousMove;
            continuousMoveAction.canceled += OnCancelContinuousMove;
        }

        protected virtual void OnMove(InputAction.CallbackContext context)
        {
            if (StageElement.Stage == null)
            {
                return;
            }

            var moveValue = context.ReadValue<Vector2>();
            TryMove(moveValue);
        }

        protected virtual void Update()
        {
            if (isContinuousMove == false)
            {
                return;
            }
            
            var moveValue = continuousMoveAction.ReadValue<Vector2>();
            TryMove(moveValue);
        }
        
        protected virtual void OnContinuousMove(InputAction.CallbackContext context)
        {
            isContinuousMove = true;
        }
        
        protected virtual void OnCancelContinuousMove(InputAction.CallbackContext context)
        {
            isContinuousMove = false;
        }

        protected virtual void TryMove(Vector2 moveValue)
        {
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
            }, out var shouldStop);

            if (shouldStop)
            {
                return;
            }

            FacingDirection = moveDirection.ToFourTypesDirection();
            OnFacingDirectionChanged?.Invoke(FacingDirection);
        }
    }
}