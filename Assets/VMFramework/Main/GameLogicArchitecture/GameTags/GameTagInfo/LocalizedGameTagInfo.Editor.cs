#if UNITY_EDITOR
using VMFramework.Core;
using VMFramework.Localization;

namespace VMFramework.GameLogicArchitecture
{
    public partial class LocalizedGameTagInfo : ILocalizedStringOwnerConfig
    {
        public virtual void RemoveInvalidLocalizedStrings()
        {
            if (name != null && name.IsValid() == false)
            {
                name = null;
            }
        }

        public virtual void SetDefaultKeyValue(LocalizedStringAutoConfigSettings setting)
        {
            var key = id.ToPascalCase() + "TagName";
            var content = id.ToPascalCase(" ");
            string defaultTableName = setting.defaultTableName;
            if (defaultTableName.IsNullOrEmpty())
            {
                defaultTableName = CoreSetting.GameTagGeneralSetting.defaultLocalizationTableName;
            }
            LocalizedStringEditorUtility.SetDefaultKey(ref name, defaultTableName, key, content, replace: false);
        }
    }
}
#endif