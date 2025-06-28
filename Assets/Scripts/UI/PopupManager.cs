using Sirenix.OdinInspector;
using VMFramework.OdinExtensions;
using VMFramework.Procedure;
using VMFramework.UI;

namespace RoomPuzzle
{
    [ManagerCreationProvider(ManagerType.UICore)]
    public class PopupManager : ManagerBehaviour<PopupManager>
    {
        [Button]
        public IUIPanel PopupText([GamePrefabID(typeof(IUIPanelConfig))] string panelID, TracingConfig config,
            SimpleText text)
        {
            var panel = TracingUIManager.Instance.OpenOn(panelID, config, out _);

            if (panel.TryGetComponent(out Popup popup))
            {
                popup.TextLabel.text = text.text;
            }
            
            return panel;
        }
    }
}