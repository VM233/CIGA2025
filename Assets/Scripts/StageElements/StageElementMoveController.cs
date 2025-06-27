using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoomPuzzle
{
    public class StageElementMoveController : MonoBehaviour
    {
        public Ease ease = Ease.InOutSine;

        protected IStageElement element;

        [ShowInInspector]
        protected bool isMoving;

        protected virtual void Awake()
        {
            element = GetComponent<IStageElement>();
            element.OnMove += OnMove;
            element.OnCheckMoving += OnCheckMoving;
            element.OnStageChanged += OnStateChanged;
        }

        protected virtual void OnStateChanged(IStageElement stageElement, bool isAdd)
        {
            transform.DOKill();
            isMoving = false;
        }

        protected virtual void OnCheckMoving(IStageElement stageElement, ref bool isMoving)
        {
            if (this.isMoving)
            {
                isMoving = true;
            }
        }

        protected virtual void OnMove(IStageElement stageElement, IReadOnlyList<IStageElement> others,
            Vector2Int previous, Vector2Int current, MoveHint hint)
        {
            var startPosition = element.Stage.GetRealPosition(previous);
            var endPosition = element.Stage.GetRealPosition(current);

            isMoving = true;
            transform.DOKill();
            transform.position = startPosition;
            transform.DOMove(endPosition, hint.duration).SetEase(ease).OnComplete(() => { isMoving = false; });
        }
    }
}