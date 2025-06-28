using System;
using System.Collections.Generic;
using UnityEngine;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace RoomPuzzle
{
    public class PlayerGraphicController : MonoBehaviour
    {
        [AnimatorParameter(AnimatorControllerParameterType.Float)]
        [IsNotNullOrEmpty]
        public string directionXParamName;

        [AnimatorParameter(AnimatorControllerParameterType.Float)]
        [IsNotNullOrEmpty]
        public string directionYParamName;

        [AnimatorParameter(AnimatorControllerParameterType.Bool)]
        [IsNotNullOrEmpty]
        public string isWalkingParamName;

        protected Animator animator;
        protected PlayerController playerController;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            playerController = GetComponentInParent<PlayerController>();
            playerController.OnFacingDirectionChanged += OnFacingDirectionChanged;
        }

        protected virtual void OnFacingDirectionChanged(FourTypesDirection direction)
        {
            var vector = direction.ToCardinalVector();
            animator.SetFloat(directionXParamName, vector.x);
            animator.SetFloat(directionYParamName, vector.y);
        }

        protected virtual void Update()
        {
            animator.SetBool(isWalkingParamName, playerController.StageElement.IsMoving());
        }
    }
}