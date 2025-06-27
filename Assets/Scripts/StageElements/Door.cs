using System;
using UnityEngine;

namespace RoomPuzzle
{
    public class Door : MonoBehaviour
    {
        public int stageIndex;

        protected IStageElement stageElement;

        protected virtual void Awake()
        {
            stageElement = GetComponent<IStageElement>();
            stageElement.OnCheckEnterable += OnCheckEnterable;
            stageElement.OnInteract += OnInteract;
        }

        protected virtual void OnCheckEnterable(IStageElement element, IStageElement other, MoveHint hint,
            ref bool canEnter)
        {
            if (other.TryGetComponent(out PlayerController playerController) == false)
            {
                canEnter = false;
            }
        }

        protected virtual void OnInteract(IStageElement element, IStageElement other, InteractHint hint)
        {
            if (other.TryGetComponent(out PlayerController playerController) == false)
            {
                return;
            }

            StageManager.Instance.LoadStage(stageIndex);
        }
    }
}