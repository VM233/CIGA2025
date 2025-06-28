using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace VMFramework.Configuration
{
    public class StringCommonPreset : CommonPreset
    {
        [ListDrawerSettings(ShowFoldout = false)]
        public List<string> presets = new()
        {
            "Default"
        };

        public override IEnumerable<ValueDropdownItem> GetDropdownItems()
        {
            return presets.Select(preset => new ValueDropdownItem(preset, preset));
        }
    }
}