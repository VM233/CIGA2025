using System;
using UnityEngine;

namespace RoomPuzzle
{
    public class PushEnd : MonoBehaviour
    {
        protected IStageElement stageElement;

        protected virtual void Awake()
        {
            stageElement = GetComponent<IStageElement>();
            stageElement.OnStageChanged += OnStateChanged;
            stageElement.OnInteract += OnInteract;
        }

        protected virtual void OnStateChanged(IStageElement element, bool isAdd)
        {
            if (isAdd)
            {
                element.Stage.PushEndCount++;
            }
            else
            {
                element.Stage.PushEndCount--;
            }
        }

        protected virtual void OnInteract(IStageElement element, IStageElement from, InteractHint hint)
        {
            if (from.TryGetComponent(out PushableElement pushable) == false)
            {
                return;
            }
            
            element.Stage.ModifyValidPushEndCount(true);
        }
    }
}