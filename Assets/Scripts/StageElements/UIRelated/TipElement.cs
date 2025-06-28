using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.OdinExtensions;
using VMFramework.UI;

namespace RoomPuzzle
{
    public class TipElement : MonoBehaviour
    {
        [GamePrefabID(typeof(IUIPanelConfig))]
        [IsNotNullOrEmpty]
        public string popupID;

        [IsNotNullOrEmpty]
        public string tipText = "Tip Text";

        [MinValue(0)]
        public float tipDuration = 3f;

        protected IStageElement stageElement;

        protected virtual void Awake()
        {
            stageElement = GetComponent<IStageElement>();
            stageElement.OnInteract += OnInteract;
        }

        protected virtual async void OnInteract(IStageElement element, IStageElement from, InteractHint hint)
        {
            var popup = PopupManager.Instance.PopupText(popupID, transform.position, tipText);

            await UniTask.WaitForSeconds(tipDuration);
            
            popup.Close();
        }
    }
}