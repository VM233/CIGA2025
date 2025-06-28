#if UNITY_EDITOR
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Animations
{
    public partial class GameObjectAnimationGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Game Object Animations";
    }
}
#endif