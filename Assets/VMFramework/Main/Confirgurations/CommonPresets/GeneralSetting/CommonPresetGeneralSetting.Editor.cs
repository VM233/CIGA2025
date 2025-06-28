#if UNITY_EDITOR
using System;
using VMFramework.Core;
using VMFramework.Core.Editor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Configuration 
{
    public partial class CommonPresetGeneralSetting
    {
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            
        }

        public bool AddPresetIfNotExists(string key, Type presetType)
        {
            if (key.IsNullOrEmpty()) 
            {
                return false;
            }
            
            if (presets.TryGetValue(key, out var preset))
            {
                if (preset != null)
                {
                    return false;
                }
            }

            var fileName = key.ToPascalCase(" ");
            var path = ConfigurationPath.DEFAULT_COMMON_PRIORITIES_PATH.PathCombine(fileName);
            var asset = presetType.CreateScriptableObjectAsset(path);
            preset = (CommonPreset)asset;
            presets[key] = preset;
            return true;
        }
    }
}
#endif