#if UNITY_EDITOR
using VMFramework.Core;
using VMFramework.Localization;

namespace VMFramework.UI
{
    public partial class InfoDebugEntry
    {
        protected override void SetDefaultKeyValue(LocalizedStringAutoConfigSettings setting)
        {
            base.SetDefaultKeyValue(setting);

            var key = id.ToPascalCase() + "Content";
            LocalizedStringEditorUtility.SetDefaultKey(ref content, setting.defaultTableName, key, content: "",
                replace: false);
        }
    }
}
#endif