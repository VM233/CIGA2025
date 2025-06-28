using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using VMFramework.Configuration;
using VMFramework.GameLogicArchitecture;
using VMFramework.UI;

namespace VMFramework.Properties
{
    public sealed partial class TooltipPropertyGeneralSetting : GeneralSetting
    {
        #region Categories

        public const string PROPERTY_TOOLTIP_CATEGORY = "Tooltip Property";

        #endregion

        [TabGroup(TAB_GROUP_NAME, PROPERTY_TOOLTIP_CATEGORY)]
        [JsonProperty]
        public List<GameObjectTooltipPropertyConfig> tooltipPropertyConfigs = new();

        #region Check & Init

        public override void CheckSettings()
        {
            base.CheckSettings();

            tooltipPropertyConfigs.CheckSettings();
        }

        protected override void OnInit()
        {
            base.OnInit();

            tooltipPropertyConfigs.Init();
        }

        #endregion

        public void GetGameObjectTooltipProperties(object obj, ICollection<PropertyDisplayInfo> infos)
        {
            foreach (var config in tooltipPropertyConfigs)
            {
                if (config.property.IsActive == false)
                {
                    continue;
                }

                var property = config.property;
                var icon = config.property.Icon;
                var groupName = config.groupName;

                if (property.CanGetValueString(obj) == false)
                {
                    continue;
                }

                property.GetFullString(obj, out var value, out var source);
                var info = new PropertyDisplayInfo(value, icon, groupName, source);

                infos.Add(info);
            }
        }
    }
}