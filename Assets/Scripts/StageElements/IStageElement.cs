using System.Collections.Generic;
using UnityEngine;
using VMFramework.Core;

namespace RoomPuzzle
{
    public interface IStageElement : IController
    {
        public delegate void MoveHandler(IStageElement element, IReadOnlyList<IStageElement> others,
            Vector2Int previous, Vector2Int current, MoveHint hint);

        public delegate void MovingCheckableHandler(IStageElement element, ref bool isMoving);

        public delegate void EnterableCheckHandler(IStageElement element, IStageElement other, MoveHint hint,
            ref bool canEnter);

        public delegate void InteractHandler(IStageElement element, IStageElement from, InteractHint hint);

        public delegate void InteractableCheckHandler(IStageElement element, IStageElement from, InteractHint hint,
            ref bool canInteract, ref bool valid);

        public delegate void StageChangedHandler(IStageElement element, bool isAdd);

        public bool AutoInteract { get; }
        
        public StageCore Stage { get; }

        public Vector2Int Position { get; set; }

        public event StageChangedHandler OnStageChanged;
        public event EnterableCheckHandler OnCheckEnterable;
        public event MovingCheckableHandler OnCheckMoving;
        public event MoveHandler OnMove;
        public event InteractableCheckHandler OnCheckInteractable;
        public event InteractHandler OnInteract;
        
        public void SetStage(StageCore stage);

        public bool CanEnter(IStageElement other, MoveHint hint);

        public bool IsMoving();

        public void Move(IReadOnlyList<IStageElement> others, Vector2Int previous, Vector2Int current, MoveHint hint);

        public bool CanInteract(IStageElement from, InteractHint hint, out bool validInteract);

        public bool Interact(IStageElement from, InteractHint hint, out bool valid);
    }
}