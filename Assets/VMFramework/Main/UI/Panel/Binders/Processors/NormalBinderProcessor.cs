using System.Collections.Generic;
using VMFramework.Configuration;

namespace VMFramework.UI
{
    public class NormalBinderProcessor<TTarget> : IFuncTargetsProcessor<object, object>
    {
        public virtual void ProcessTargets(IReadOnlyCollection<object> targets, ICollection<object> results)
        {
            foreach (var target in targets)
            {
                if (target is TTarget)
                {
                    results.Add(target);
                }
            }
        }
    }
}