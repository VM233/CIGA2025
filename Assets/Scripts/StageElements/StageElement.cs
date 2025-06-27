using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoomPuzzle
{
    public class StageElement : MonoBehaviour, IStageElement
    {
        [ShowInInspector]
        public StageCore Stage { get; set; }

        [ShowInInspector]
        public Vector2Int Position { get; set; }

        public event IStageElement.EnterableCheckHandler OnCheckEnterable;
        public event IStageElement.MoveHandler OnMove;
        public event IStageElement.MovingCheckableHandler OnCheckMoving;
        public event IStageElement.InteractableCheckHandler OnCheckInteractable;
        public event IStageElement.InteractHandler OnInteract;

        public bool CanEnter(IStageElement other, MoveHint hint)
        {
            bool canEnter = true;
            OnCheckEnterable?.Invoke(this, other, ref canEnter);
            return canEnter;
        }

        public void Move(IReadOnlyList<IStageElement> others, Vector2Int previous, Vector2Int current, MoveHint hint)
        {
            OnMove?.Invoke(this, others, previous, current, hint);
        }

        public bool IsMoving()
        {
            bool isMoving = false;
            OnCheckMoving?.Invoke(this, ref isMoving);
            return isMoving;
        }

        public bool CanInteract(IStageElement from, InteractHint hint)
        {
            bool canInteract = true;
            OnCheckInteractable?.Invoke(this, from, hint, ref canInteract);
            return canInteract;
        }

        public void Interact(IStageElement from, InteractHint hint)
        {
            if (CanInteract(from, hint) == false)
            {
                return;
            }

            OnInteract?.Invoke(this, from, hint);
        }
    }
}