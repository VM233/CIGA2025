using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;

namespace VMFramework.OdinExtensions
{
    internal sealed class RectangleFloatDrawer : OdinValueDrawer<IKCube<Vector2>>, IDefinesGenericMenuItems
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            var rect = GUIHelper.GetCurrentLayoutRect();
            var value = DragAndDropUtilities.DragAndDropZone(rect, null, typeof(Object), true, true, true);
            if (EditorGUI.EndChangeCheck())
            {
                if (value.TryGetComponent(out BoxCollider2D boxCollider))
                {
                    var colliderRect = boxCollider.GetRectangle();
                    ValueEntry.SmartValue.Min = colliderRect.min;
                    ValueEntry.SmartValue.Max = colliderRect.max;
                }
            }
            
            CallNextDrawer(label);
        }

        void IDefinesGenericMenuItems.PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
        {
            genericMenu.AddSeparator();
            
            genericMenu.AddItem("Reset", () =>
            {
                ValueEntry.SmartValue.Min = Vector2.zero;
                ValueEntry.SmartValue.Max = Vector2.zero;
            });
        }
    }
}