#if UNITY_EDITOR
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;

namespace VMFramework.Properties
{
    public partial class GameProperty : IGameEditorMenuTreeNode
    {
        EditorIcon IEditorIconProvider.Icon => icon;
    }
}
#endif