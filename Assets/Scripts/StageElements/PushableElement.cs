using UnityEngine;

namespace RoomPuzzle
{
    public class PushableElement : MonoBehaviour
    {
        protected IStageElement stageElement;

        protected virtual void Awake()
        {
            stageElement = GetComponent<IStageElement>();
            stageElement.OnCheckEnterable += OnCheckEnterable;
            stageElement.OnCheckInteractable += OnCheckInteractable;
            stageElement.OnInteract += OnInteract;
        }

        protected virtual void OnCheckEnterable(IStageElement element, IStageElement other, MoveHint hint,
            ref bool canEnter)
        {
            if (stageElement.Stage.CanMoveTo(stageElement, hint, out var shouldStop) == false)
            {
                canEnter = false;
            }
        }

        protected virtual void OnCheckInteractable(IStageElement element, IStageElement other, InteractHint hint,
            ref bool canInteract, ref bool valid)
        {
            if (stageElement.Stage.CanMoveTo(stageElement, hint.moveHint, out var shouldStop) == false)
            {
                canInteract = false;
            }
            else
            {
                valid = true;
            }
        }

        protected virtual void OnInteract(IStageElement element, IStageElement other, InteractHint hint)
        {
            stageElement.Stage.MoveAndInteract(stageElement, hint, out var shouldStop);
        }
    }
}