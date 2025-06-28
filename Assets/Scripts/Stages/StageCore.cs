using System;
using System.Collections.Generic;
using EnumsNET;
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
            element.SetStage(this);
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

            element.SetStage(null);
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

        public virtual bool CanMoveTo(IStageElement element, MoveHint hint, out bool shouldStop)
        {
            if (element.IsMoving())
            {
                shouldStop = true;
                return false;
            }
            
            var newPosition = element.Position + hint.direction;
            if (elementsLookup.TryGetValue(newPosition, out var elementsAtNewPosition))
            {
                foreach (var otherElement in elementsAtNewPosition)
                {
                    if (otherElement.IsMoving())
                    {
                        shouldStop = true;
                        return false;
                    }

                    if (otherElement.CanEnter(element, hint) == false)
                    {
                        shouldStop = false;
                        return false;
                    }
                }
            }

            shouldStop = false;
            return true;
        }

        public virtual bool MoveAndInteract(IStageElement element, InteractHint hint, out bool shouldStop)
        {
            var oldPosition = element.Position;
            var newPosition = oldPosition + hint.moveHint.direction;

            var elementsAtNewPosition = elementsLookup.GetValueOrAddNew(newPosition);

            var canMoveTo = CanMoveTo(element, hint.moveHint, out shouldStop);

            if (shouldStop)
            {
                return false;
            }

            if (canMoveTo == false)
            {
                foreach (var otherElement in elementsAtNewPosition)
                {
                    if (otherElement.InteractionMode.HasAnyFlags(ElementInteractionMode.TryEnter))
                    {
                        otherElement.Interact(element, hint, out _);
                    }
                }

                return false;
            }

            var otherElements = new List<IStageElement>();

            foreach (var otherElement in elementsAtNewPosition)
            {
                if (otherElement.IsMoving())
                {
                    return false;
                }
            }

            otherElements.AddRange(elementsAtNewPosition);
            elementsAtNewPosition.Add(element);

            if (elementsLookup.TryGetValue(oldPosition, out var elementsAtOldPosition))
            {
                elementsAtOldPosition.Remove(element);
            }

            element.Position = newPosition;

            element.Move(otherElements, previous: oldPosition, current: newPosition, hint.moveHint);

            foreach (var otherElement in otherElements)
            {
                if (otherElement.InteractionMode.HasAnyFlags(ElementInteractionMode.Overlap))
                {
                    otherElement.Interact(element, hint, out _);
                }
            }

            return true;
        }
    }
}