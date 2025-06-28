using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace VMFramework.Configuration
{
    public class GeneralCommonPreset<TValue> : CommonPreset
    {
        [ListDrawerSettings(ShowFoldout = false)]
        public List<CommonPresetEntry<TValue>> entries = new()
        {
            new("Default", default)
        };

        public override IEnumerable<ValueDropdownItem> GetDropdownItems()
        {
            foreach (var entry in entries)
            {
                yield return new ValueDropdownItem($"{entry.presetName}:{entry.value}", entry.value);
            }
        }
    }
}