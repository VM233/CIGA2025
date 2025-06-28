using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoomPuzzle
{
    public class StageElement : MonoBehaviour, IStageElement
    {
        [EnumToggleButtons]
        public ElementInteractionMode interactionMode = ElementInteractionMode.All;

        [ShowInInspector]
        public StageCore Stage { get; protected set; }

        [ShowInInspector]
        public Vector2Int Position { get; set; }
        
        public Vector2Int InitialPosition { get; protected set; }

        public event IStageElement.StageChangedHandler OnStageChanged;
        public event IStageElement.EnterableCheckHandler OnCheckEnterable;
        public event IStageElement.MoveHandler OnMove;
        public event IStageElement.MovingCheckableHandler OnCheckMoving;
        public event IStageElement.InteractableCheckHandler OnCheckInteractable;
        public event IStageElement.InteractHandler OnInteract;

        public void SetStage(StageCore stage)
        {
            if (stage == null)
            {
                Stage = null;
                OnStageChanged?.Invoke(this, isAdd: false);
            }
            else
            {
                Stage = stage;
                InitialPosition = Position;
                OnStageChanged?.Invoke(this, isAdd: true);
            }
        }

        public bool CanEnter(IStageElement other, MoveHint hint)
        {
            bool canEnter = true;
            OnCheckEnterable?.Invoke(this, other, hint, ref canEnter);
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

        public bool CanInteract(IStageElement from, InteractHint hint, out bool validInteract)
        {
            bool canInteract = true;
            validInteract = false;
            OnCheckInteractable?.Invoke(this, from, hint, ref canInteract, ref validInteract);
            return canInteract;
        }

        public bool Interact(IStageElement from, InteractHint hint, out bool valid)
        {
            if (CanInteract(from, hint, out valid) == false)
            {
                return false;
            }

            OnInteract?.Invoke(this, from, hint);
            
            return true;
        }

        public void ResetElement()
        {
            Stage.MoveTo(this, InitialPosition);
        }

        ElementInteractionMode IStageElement.InteractionMode => interactionMode;
    }
}