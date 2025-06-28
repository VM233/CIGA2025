using System.Collections.Generic;
using VMFramework.Configuration;
using VMFramework.Core;

namespace VMFramework.UI
{
    public class ComponentBindProcessor<TComponent> : IFuncTargetsProcessor<object, object>
    {
        public virtual void ProcessTargets(IReadOnlyCollection<object> targets, ICollection<object> results)
        {
            foreach (var target in targets)
            {
                if (target.TryGetComponent(out TComponent component))
                {
                    results.Add(component);
                }
            }
        }
    }
}