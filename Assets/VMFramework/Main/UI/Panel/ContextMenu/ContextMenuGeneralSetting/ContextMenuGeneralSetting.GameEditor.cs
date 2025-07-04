﻿#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public partial class ContextMenuGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Context Menu";

        EditorIcon IEditorIconProvider.Icon => SdfIconType.BorderStyle;
    }
}
#endif