using UnityEngine;

namespace RoomPuzzle
{
    public class PushableElement : MonoBehaviour
    {
        protected IStageElement stageElement;

        protected virtual void Awake()
        {
            stageElement = GetComponent<IStageElement>();
            stageElement.OnInteract += OnInteract;
        }

        protected virtual void OnInteract(IStageElement element, IStageElement other, InteractHint hint)
        {
            stageElement.Stage.MoveAndInteract(stageElement, hint);
        }
    }
}