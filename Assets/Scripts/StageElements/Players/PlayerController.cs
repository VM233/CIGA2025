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

        [MinValue(0)]
        public float moveDuration = 0.4f;
        
        [ShowInInspector]
        public FourTypesDirection FacingDirection { get; protected set; }

        public IStageElement StageElement { get; protected set; }
        
        public event Action<FourTypesDirection> OnFacingDirectionChanged;

        protected InputAction moveAction;
        protected readonly Queue<Vector2Int> moveCommandQueue = new();

        protected virtual void Awake()
        {
            StageElement = GetComponent<IStageElement>();

            moveAction = InputSystem.actions.FindAction(moveInputName);
            moveAction.performed += OnMove;
        }

        protected virtual void Update()
        {
            if (moveCommandQueue.Count <= 0)
            {
                return;
            }

            var direction = moveCommandQueue.Peek();
            
            StageElement.Stage.MoveAndInteract(StageElement, new InteractHint()
            {
                moveHint = new MoveHint()
                {
                    duration = moveDuration,
                    direction = direction
                }
            }, out var shouldStop);

            if (shouldStop)
            {
                return;
            }

            FacingDirection = direction.ToFourTypesDirection();
            OnFacingDirectionChanged?.Invoke(FacingDirection);
            
            moveCommandQueue.Dequeue();
        }

        protected virtual void OnMove(InputAction.CallbackContext context)
        {
            if (StageElement.Stage == null)
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
            
            moveCommandQueue.Enqueue(moveDirection);
        }
    }
}