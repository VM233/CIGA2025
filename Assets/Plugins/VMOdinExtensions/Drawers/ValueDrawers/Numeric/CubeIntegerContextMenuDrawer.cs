#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Drawers;
using UnityEditor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;

namespace VMFramework.OdinExtensions
{
    [DrawerPriority(DrawerPriorityLevel.SuperPriority)]
    internal sealed class CubeIntegerContextMenuDrawer : OdinValueDrawer<IKCube<Vector3Int>>, IDefinesGenericMenuItems
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            CallNextDrawer(label);
        }

        void IDefinesGenericMenuItems.PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
        {
            genericMenu.AddSeparator();
            
            genericMenu.AddItem("Reset", () =>
            {
                ValueEntry.SmartValue.Min = Vector3Int.zero;
                ValueEntry.SmartValue.Max = Vector3Int.zero;
            });
            
            genericMenu.AddItem("Set to max CubeInteger", () =>
            {
                ValueEntry.SmartValue.Max = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
                ValueEntry.SmartValue.Min = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
            });
        }
    }
}
#endif