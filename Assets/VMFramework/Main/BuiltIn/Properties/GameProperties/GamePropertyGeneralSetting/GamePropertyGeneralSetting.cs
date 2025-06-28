using System;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Properties
{
    public sealed partial class GamePropertyGeneralSetting : GamePrefabGeneralSetting
    {
        #region Categories
        
        public const string PROPERTY_SETTING_CATEGORY = "Property";

        #endregion

        #region Metadata

        public override Type BaseGamePrefabType => typeof(GameProperty);

        #endregion
    }
}
