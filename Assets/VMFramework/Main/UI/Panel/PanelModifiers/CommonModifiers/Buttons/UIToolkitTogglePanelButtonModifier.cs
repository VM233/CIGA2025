using Sirenix.OdinInspector;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class UIToolkitTogglePanelButtonModifier : UIToolkitButtonModifier
    {
        [BoxGroup(CONFIGS_CATEGORY)]
        [GamePrefabID(typeof(IUIPanelConfig))]
        [UIPanelConfigID(isUnique: true)]
        [IsNotNullOrEmpty]
        public string uiPanelID;
        
        [BoxGroup(CONFIGS_CATEGORY)]
        public bool closeThisPanelAfterOpen = false;

        protected override void OnClicked()
        {
            if (UIPanelManager.TryGetUniquePanelWithWarning(uiPanelID, out var targetPanel) == false)
            {
                return;
            }
            
            targetPanel.Toggle();
            
            if (closeThisPanelAfterOpen)
            {
                Panel.Close();
            }
        }
    }
}