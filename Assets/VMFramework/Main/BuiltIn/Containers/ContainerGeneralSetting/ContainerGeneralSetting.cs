using System;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.Containers
{
    public sealed partial class ContainerGeneralSetting : GamePrefabGeneralSetting
    {
        #region Meta Data

        public override Type BaseGamePrefabType => typeof(ContainerConfig);
        
        public override string GameItemName => nameof(Container);

        #endregion

        [IsNotNullOrEmpty]
        public string transformContainerName = "#Containers";
    }
}
