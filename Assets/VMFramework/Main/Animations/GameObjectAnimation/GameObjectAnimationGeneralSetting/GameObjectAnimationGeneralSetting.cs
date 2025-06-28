using System;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Animations
{
    public sealed partial class GameObjectAnimationGeneralSetting : GamePrefabGeneralSetting
    {
        public override Type BaseGamePrefabType => typeof(GameObjectAnimation);
    }
}