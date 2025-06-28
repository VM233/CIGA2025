using UnityEngine.Scripting;
using VMFramework.Core;
using VMFramework.UI;

namespace VMFramework.Configuration
{
    [Preserve]
    public class IconOwnerCheckProcessor : TypedActionProcessor<IIconOwner>, ICheckProcessor
    {
        protected override void ProcessTypedTarget(IIconOwner typedTarget)
        {
            if (typedTarget.Icon == null)
            {
                Debugger.LogWarning($"[{nameof(IconOwnerCheckProcessor)}]" +
                                    $"The {nameof(typedTarget.Icon)} of {typedTarget} is not set.");
            }
        }
    }
}