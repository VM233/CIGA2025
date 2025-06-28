using UnityEngine;
using VMFramework.Configuration;
using VMFramework.OdinExtensions;

namespace VMFramework.Containers
{
    public abstract class ContainerSlotFilter : MonoBehaviour
    {
        [Minimum(0)]
        public RangeIntegerConfig slotRange;

        public abstract IFilter GetFilter();
    }
}