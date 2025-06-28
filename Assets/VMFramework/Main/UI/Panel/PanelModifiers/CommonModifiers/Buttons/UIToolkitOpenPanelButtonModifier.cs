using Sirenix.OdinInspector;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class UIToolkitOpenPanelButtonModifier : UIToolkitButtonModifier
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
            UIPanelManager.GetAndOpenUniquePanel(uiPanelID);

            if (closeThisPanelAfterOpen)
            {
                Panel.Close();
            }
        }
    }
}