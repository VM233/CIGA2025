using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.UI
{
    public static class TargetBinderUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetTarget<TTarget>(this IEnumerable<ITargetBinder<TTarget>> binders, object source, out TTarget target)
        {
            foreach (var binder in binders)
            {
                if (binder.TryGetTarget(source, out target))
                {
                    return true;
                }
            }
            
            target = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TTarget> GetTargets<TTarget>(this IEnumerable<ITargetBinder<TTarget>> binders,
            object source)
        {
            foreach (var binder in binders)
            {
                if (binder.TryGetTarget(source, out var target))
                {
                    yield return target;
                }
            }
        }
    }
}