using UnityEngine;
using UnityEngine.Tilemaps;
using VMFramework.Core;

namespace RoomPuzzle
{
    public class Block : MonoBehaviour
    {
        public bool blocked = true;

        protected StageElement stageElement;

        protected virtual void Awake()
        {
            stageElement = GetComponent<StageElement>();
            stageElement.OnCheckEnterable += OnCheckEnterable;
        }

        protected virtual void OnCheckEnterable(IStageElement element, IStageElement other, ref bool canEnter)
        {
            if (blocked)
            {
                canEnter = false;
            }
        }

        protected virtual void Start()
        {
            var stageCore = gameObject.GetComponentInParent<StageCore>();
            var tilemap = gameObject.GetComponentInParent<Tilemap>();

            var position = tilemap.WorldToCell(transform.position).XY();

            stageCore.AddElement(position, stageElement);
        }
    }
}