﻿#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Editor
{
    public partial class HierarchyGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Hierarchy";

        EditorIcon IEditorIconProvider.Icon => SdfIconType.FileEarmarkRichtextFill;
    }
}
#endif