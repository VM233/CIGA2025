#if UNITY_EDITOR && DOTWEEN && UNITASK_DOTWEEN_SUPPORT
using UnityEngine;
using VMFramework.Configuration;

namespace VMFramework.Animations
{
    public partial class Move
    {
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            end ??= new SingleValueChooserConfig<Vector3>();
        }
    }
}
#endif