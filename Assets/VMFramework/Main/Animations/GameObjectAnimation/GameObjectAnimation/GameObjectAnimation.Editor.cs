#if UNITY_EDITOR
namespace VMFramework.Animations
{
    public partial class GameObjectAnimation
    {
        private const string PRESET_CATEGORY = "Preset";
        
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            clips ??= new();
        }
    }
}
#endif