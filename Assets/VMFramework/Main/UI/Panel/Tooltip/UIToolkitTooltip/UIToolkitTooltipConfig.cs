using System;

namespace VMFramework.UI
{
    public partial class UIToolkitTooltipConfig : UIToolkitPanelConfig, ITooltipConfig
    {
        public override Type GameItemType => typeof(UIToolkitTooltip);
    }
}