using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.Core;

namespace VMFramework.Containers
{
    public class SlotFiltersManager : MonoBehaviour, ISlotFiltersManager
    {
        public bool addToFilteredSlotFirst = true;

        bool ISlotFiltersManager.AddToFilteredSlotFirst => addToFilteredSlotFirst;

        public int MinIndex { get; private set; }
        public int MaxIndex { get; private set; }
        
        [ShowInInspector, HideInEditorMode]
        private List<IFilter>[] filters;

        private void Awake()
        {
            var containerSlotFilters = GetComponentsInChildren<ContainerSlotFilter>();
            
            MinIndex = 0;
            MaxIndex = 0;

            foreach (var filterInfo in containerSlotFilters)
            {
                MinIndex = MinIndex.Min(filterInfo.slotRange.min);
                MaxIndex = MaxIndex.Max(filterInfo.slotRange.max);
            }
            
            var count = MaxIndex - MinIndex + 1;
            filters = new List<IFilter>[count];

            foreach (var filterInfo in containerSlotFilters)
            {
                foreach (var slotIndex in filterInfo.slotRange)
                {
                    var offset = slotIndex - MinIndex;
                    filters[offset] ??= new List<IFilter>();
                    filters[offset].Add(filterInfo.GetFilter());
                }
            }
        }
        
        public bool IsMatch(int slotIndex, IContainerItem item, out bool hasFilters)
        {
            if (slotIndex < MinIndex || slotIndex > MaxIndex)
            {
                hasFilters = false;
                return true;
            }
            
            var offset = slotIndex - MinIndex;

            if (filters[offset] == null)
            {
                hasFilters = false;
                return true;
            }

            hasFilters = true;
            
            foreach (var filter in filters[offset])
            {
                if (filter.IsMatch(item) == false)
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}