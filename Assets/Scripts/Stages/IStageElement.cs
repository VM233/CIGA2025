using System.Collections.Generic;
using UnityEngine;
using VMFramework.Core;

namespace RoomPuzzle
{
    public interface IStageElement : IController
    {
        public delegate void MoveHandler(IStageElement element, IReadOnlyList<IStageElement> others, Vector2Int previous,
            Vector2Int current, MoveHint hint);

        public delegate void MovingCheckableHandler(IStageElement element, ref bool isMoving);

        public delegate void EnterableCheckHandler(IStageElement element, IStageElement other, ref bool canEnter);

        public StageCore Stage { get; set; }

        public Vector2Int Position { get; set; }

        public event EnterableCheckHandler OnCheckEnterable;
        public event MovingCheckableHandler OnCheckMoving;
        public event MoveHandler OnMove;

        public bool CanEnter(IStageElement other);

        public bool IsMoving();

        public void Move(IReadOnlyList<IStageElement> others, Vector2Int previous, Vector2Int current, MoveHint hint);
    }
}