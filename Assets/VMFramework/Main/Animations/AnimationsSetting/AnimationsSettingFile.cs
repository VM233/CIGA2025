using Sirenix.OdinInspector;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Animations 
{
    [GlobalSettingFileConfig(FileName = nameof(AnimationsSettingFile))]
    public sealed partial class AnimationsSettingFile : GlobalSettingFile
    {
        public const string ANIMATIONS_CATEGORY = "Animations";
        
        [TabGroup(TAB_GROUP_NAME, ANIMATIONS_CATEGORY)]
        [Required]
        public GameObjectAnimationGeneralSetting gameObjectAnimationGeneralSetting;
    }
}