using System.Collections.Generic;
using VMFramework.Configuration;
using VMFramework.Containers;
using VMFramework.Core;
using VMFramework.Core.Pools;

namespace VMFramework.UI
{
    public class ContainerBindProcessor : IFuncTargetsProcessor<object, object>
    {
        public void ProcessTargets(IReadOnlyCollection<object> targets, ICollection<object> results)
        {
            foreach (var target in targets)
            {
                if (target is IContainer targetContainer)
                {
                    results.Add(targetContainer);
                    continue;
                }

                if (target is IContainersProvider targetContainersProvider)
                {
                    AddContainerProviders(targetContainersProvider, results);
                    continue;
                }

                if (target.TryGetComponents(out IContainersProvider[] containersProviders))
                {
                    foreach (var containersProvider in containersProviders)
                    {
                        AddContainerProviders(containersProvider, results);
                    }
                }
            }
        }

        protected virtual void AddContainerProviders(IContainersProvider provider, ICollection<object> results)
        {
            var containers = ListPool<IContainer>.Default.Get();
            containers.Clear();
            provider.GetContainers(containers);
            foreach (var container in containers)
            {
                results.Add(container);
            }
            containers.ReturnToDefaultPool();
        }
    }
}