#if UNITY_EDITOR
using VMFramework.Core;

namespace VMFramework.Configuration
{
    public partial class GeneralSingleValueChooserConfig<TWrapper, TItem>
    {
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            ReflectionUtility.TryCreateInstance(ref value);
        }
    }
}
#endif