using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;

namespace VMFramework.Animations 
{
    [ManagerCreationProvider(ManagerType.SettingCore)]
    public sealed partial class AnimationsSetting : GlobalSetting<AnimationsSetting, AnimationsSettingFile>
    {
        public static GameObjectAnimationGeneralSetting GameObjectAnimationGeneralSetting =>
            GlobalSettingFile == null ? null : GlobalSettingFile.gameObjectAnimationGeneralSetting;
    }
}