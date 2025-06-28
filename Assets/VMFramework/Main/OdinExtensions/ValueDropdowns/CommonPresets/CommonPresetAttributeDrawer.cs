#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using VMFramework.Core;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.OdinExtensions
{
    internal sealed class CommonPresetAttributeDrawer : GeneralValueDropdownAttributeDrawer<CommonPresetAttribute>
    {
        protected override IEnumerable<ValueDropdownItem> GetValues()
        {
            var setting = CoreSetting.CommonPresetGeneralSetting;
            if (setting == null)
            {
                return Enumerable.Empty<ValueDropdownItem>();
            }

            if (setting.presets == null)
            {
                return Enumerable.Empty<ValueDropdownItem>();
            }

            if (Attribute.Key.IsNullOrEmpty())
            {
                return Enumerable.Empty<ValueDropdownItem>();
            }

            if (setting.presets.TryGetValue(Attribute.Key, out var preset) == false)
            {
                return Enumerable.Empty<ValueDropdownItem>();
            }
            
            return preset.GetDropdownItems();
        }

        protected override void DrawCustomButtons()
        {
            base.DrawCustomButtons();
            
            var setting = CoreSetting.CommonPresetGeneralSetting;

            if (setting == null)
            {
                return;
            }
            
            var presets = CoreSetting.CommonPresetGeneralSetting.presets;

            if (presets == null)
            {
                return;
            }
            
            if (Button(GameEditorNames.JUMP_TO_GAME_EDITOR, SdfIconType.Search))
            {
                if (presets.TryGetValue(Attribute.Key, out var preset))
                {
                    var gameEditor = EditorWindow.GetWindow<GameEditor>();
                
                    gameEditor.SelectValue(preset);
                }
            }
        }
    }
}
#endif