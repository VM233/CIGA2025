#if UNITY_EDITOR
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;
using VMFramework.UI;

namespace VMFramework.Properties
{
    public partial class TooltipPropertyGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Property Tooltip";

        object IGameEditorMenuTreeNode.ParentNode {
            get
            {
                var tooltipGeneralSetting = UISetting.TooltipGeneralSetting;
                if (tooltipGeneralSetting == null)
                {
                    return null;
                }
                
                return tooltipGeneralSetting;
            }
        }
    }
}
#endif