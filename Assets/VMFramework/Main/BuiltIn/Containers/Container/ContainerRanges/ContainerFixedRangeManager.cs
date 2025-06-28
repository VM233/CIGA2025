using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.Containers
{
    public class ContainerFixedRangeManager : MonoBehaviour, IContainerRangeManager
    {
        [IsNotNullOrEmpty]
        public List<ContainerRange> containerRanges = new();

        public bool enableAddableRanges = true;

        public bool enableSortableRanges = true;

        [ShowInInspector]
        protected readonly List<RangeInteger> addableRanges = new();

        [ShowInInspector]
        protected readonly List<RangeInteger> sortableRanges = new();

        protected virtual void Awake()
        {
            addableRanges.Clear();
            sortableRanges.Clear();

            foreach (var containerRange in containerRanges)
            {
                if (containerRange.addable)
                {
                    addableRanges.Add(new(containerRange.slotRange));
                }

                if (containerRange.sortable)
                {
                    sortableRanges.Add(new(containerRange.slotRange));
                }
            }
        }

        public bool TryGetAddableRanges(out IEnumerable<RangeInteger> ranges)
        {
            if (enableAddableRanges)
            {
                ranges = addableRanges;
                return true;
            }

            ranges = null;
            return false;
        }

        public bool TryGetSortableRanges(out IEnumerable<RangeInteger> ranges)
        {
            if (enableSortableRanges)
            {
                ranges = sortableRanges;
                return true;
            }

            ranges = null;
            return false;
        }
    }
}