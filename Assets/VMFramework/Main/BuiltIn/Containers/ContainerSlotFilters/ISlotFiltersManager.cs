namespace VMFramework.Containers
{
    public interface ISlotFiltersManager
    {
        public bool AddToFilteredSlotFirst { get; }
        
        public int MinIndex { get; }
        public int MaxIndex { get; }

        public bool IsMatch(int slotIndex, IContainerItem item, out bool hasFilters);
    }
}