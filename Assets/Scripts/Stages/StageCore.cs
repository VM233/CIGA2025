using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using VMFramework.Core;

namespace RoomPuzzle
{
    public class StageCore : MonoBehaviour
    {
        public delegate void ElementMovedHandler(IStageElement element, Vector2Int previous, Vector2Int current);

        [Required]
        public Tilemap tilemap;

        [MinValue(0)]
        public int stageIndex;

        public Vector2Int startPosition;

        public event ElementMovedHandler ElementMoved;

        [ShowInInspector]
        protected readonly Dictionary<Vector2Int, List<IStageElement>> elementsLookup = new();

        public virtual Vector2 GetRealPosition(Vector2Int position)
        {
            return tilemap.CellToWorld(position.InsertAsZ(0)) + tilemap.layoutGrid.cellSize / 2f;
        }

        public void AddElement(Vector2Int position, IStageElement element)
        {
            if (element.Stage != null)
            {
                throw new Exception("Element already belongs to a stage.");
            }

            element.Position = position;
            element.Stage = this;
            var elements = elementsLookup.GetValueOrAddNew(position);
            elements.Add(element);

            element.transform.position = GetRealPosition(position);
        }

        public void RemoveElement(IStageElement element)
        {
            if (element.Stage != this)
            {
                throw new Exception("Element does not belong to this stage.");
            }

            var position = element.Position;
            var elements = elementsLookup[position];
            elements.Remove(element);

            element.Stage = null;
        }

        public virtual bool TryGetElements(Vector2Int position, out IReadOnlyList<IStageElement> elements)
        {
            if (elementsLookup.TryGetValue(position, out var elementList))
            {
                elements = elementList;
                return true;
            }

            elements = null;
            return false;
        }

        public virtual bool CanMoveTo(IStageElement element, Vector2Int direction)
        {
            var newPosition = element.Position + direction;
            if (elementsLookup.TryGetValue(newPosition, out var elementsAtNewPosition))
            {
                foreach (var otherElement in elementsAtNewPosition)
                {
                    if (otherElement.CanEnter(element) == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public virtual bool Move(IStageElement element, Vector2Int direction, MoveHint hint)
        {
            if (CanMoveTo(element, direction) == false)
            {
                return false;
            }

            var otherElements = new List<IStageElement>();

            var oldPosition = element.Position;
            var newPosition = oldPosition + direction;
            if (elementsLookup.TryGetValue(oldPosition, out var elementsAtOldPosition))
            {
                elementsAtOldPosition.Remove(element);
            }

            if (elementsLookup.TryGetValue(newPosition, out var elementsAtNewPosition))
            {
                otherElements.AddRange(elementsAtNewPosition);
                elementsAtNewPosition.Add(element);
            }

            element.Position = newPosition;

            element.Move(otherElements, oldPosition, newPosition, hint);

            ElementMoved?.Invoke(element, oldPosition, newPosition);

            return true;
        }
    }
}