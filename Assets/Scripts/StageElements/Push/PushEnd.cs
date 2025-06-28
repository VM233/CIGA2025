using UnityEngine;

namespace RoomPuzzle
{
    public class PushEnd : MonoBehaviour
    {
        protected IStageElement stageElement;
        
        protected PushableElement pushable;

        protected virtual void Awake()
        {
            stageElement = GetComponent<IStageElement>();
            stageElement.OnStageChanged += OnStateChanged;
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

        protected virtual void Update()
        {
            if (stageElement.Stage == null)
            {
                return;
            }

            var position = stageElement.Position;

            if (stageElement.Stage.TryGetElements(position, out var elements))
            {
                foreach (var element in elements)
                {
                    if (element.TryGetComponent(out PushableElement pushable))
                    {
                        stageElement.Stage.ModifyValidPush(pushable, true);
                        this.pushable = pushable;
                        return;
                    }
                }
            }

            if (pushable != null)
            {
                stageElement.Stage.ModifyValidPush(pushable, false);
            }
        }
    }
}