#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Animations 
{
    public partial class AnimationsSettingFile : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Animations";

        EditorIcon IEditorIconProvider.Icon => SdfIconType.Play;
    }
}
#endif