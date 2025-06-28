#if UNITY_EDITOR && ODIN_INSPECTOR && ENABLE_INPUT_SYSTEM
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine.InputSystem;

namespace VMFramework.OdinExtensions
{
    public sealed class InputActionIDAttributeDrawer : GeneralValueDropdownAttributeDrawer<InputActionIDAttribute>
    {
        protected override IEnumerable<ValueDropdownItem> GetValues()
        {
            foreach (var actionMap in InputSystem.actions.actionMaps)
            {
                foreach (var action in actionMap.actions)
                {
                    yield return new(actionMap.name + "/" + action.name, action.id);
                }
            }
        }

        protected override void DrawCustomButtons()
        {
            base.DrawCustomButtons();

            if (Button("Open Input Action Map", SdfIconType.XDiamondFill))
            {
                SettingsService.OpenProjectSettings("Project/Input System Package");
            }
        }
    }
}
#endif