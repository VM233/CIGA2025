using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Configuration;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    [CommonPresetAutoRegister(TOOLTIP_PRIORITY_PRESET_KEY, typeof(int))]
    public sealed partial class TooltipGeneralSetting : GeneralSetting
    {
        private const string TOOLTIP_CATEGORY = "Tooltip";

        private const string TOOLTIP_ID_BIND_CATEGORY = TAB_GROUP_NAME + "/" + TOOLTIP_CATEGORY + "/Tooltip ID Bind";

        private const string TOOLTIP_PRIORITY_CATEGORY =
            TAB_GROUP_NAME + "/" + TOOLTIP_CATEGORY + "/Tooltip Priority Bind";

        public const string TOOLTIP_PRIORITY_PRESET_KEY = "Tooltip Priority";

        [TabGroup(TAB_GROUP_NAME, TOOLTIP_CATEGORY), TitleGroup(TOOLTIP_ID_BIND_CATEGORY)]
        [GamePrefabID(typeof(ITooltipConfig))]
        [IsNotNullOrEmpty]
        [JsonProperty]
        public string defaultTooltipID;

        [TitleGroup(TOOLTIP_ID_BIND_CATEGORY)]
        [GamePrefabID(typeof(ITooltipConfig))]
        [JsonProperty]
        public List<ITargetBinder<string>> tooltipBinders = new();

        [TitleGroup(TOOLTIP_PRIORITY_CATEGORY)]
        [CommonPreset(TOOLTIP_PRIORITY_PRESET_KEY)]
        [JsonProperty]
        public int defaultPriority;
        
        [TitleGroup(TOOLTIP_PRIORITY_CATEGORY)]
        [CommonPreset(TOOLTIP_PRIORITY_PRESET_KEY, DisableDraw = true)]
        [JsonProperty]
        public List<ITargetBinder<int>> tooltipPriorityBinders = new();

        #region Check & Init

        public override void CheckSettings()
        {
            base.CheckSettings();

            if (defaultTooltipID.IsNullOrEmpty())
            {
                Debugger.LogWarning($"{nameof(defaultTooltipID)} is not set.");
            }
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetTooltipID(object source)
        {
            if (source == null)
            {
                return null;
            }

            if (tooltipBinders.TryGetTarget(source, out string tooltipID))
            {
                return tooltipID;
            }
            
            return defaultTooltipID;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetTooltipPriority(object source)
        {
            if (source == null)
            {
                return defaultPriority;
            }

            if (tooltipPriorityBinders.TryGetTarget(source, out int priority))
            {
                return priority;
            }
            
            return defaultPriority;
        }
    }
}
