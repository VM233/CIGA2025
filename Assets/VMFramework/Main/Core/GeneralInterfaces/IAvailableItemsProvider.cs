using System.Collections.Generic;
using VMFramework.Core.Pools;

namespace VMFramework.Core
{
    public interface IAvailableItemsProvider
    {
        public IEnumerable<object> GetAvailableItems();
    }
    
    public interface IAvailableItemsProvider<TItem> : IAvailableItemsProvider
    {
        public void GetAvailableItems(ICollection<TItem> items);

        IEnumerable<object> IAvailableItemsProvider.GetAvailableItems()
        {
            var items = ListPool<TItem>.Default.Get();
            items.Clear();
            GetAvailableItems(items);
            foreach (var item in items)
            {
                yield return item;
            }
            items.ReturnToDefaultPool();
        }
    }
}