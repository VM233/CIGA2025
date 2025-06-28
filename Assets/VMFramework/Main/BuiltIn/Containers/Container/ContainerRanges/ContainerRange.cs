using UnityEngine;
using VMFramework.Configuration;
using VMFramework.OdinExtensions;

namespace VMFramework.Containers
{
    public class ContainerRange : MonoBehaviour
    {
        [Minimum(0)]
        public RangeIntegerConfig slotRange;

        public bool addable = true;
        
        public bool sortable = true;
    }
}