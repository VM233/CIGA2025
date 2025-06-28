using System.Collections;
using System.Collections.Generic;

namespace VMFramework.Properties
{
    public class HashSetProperty<TValue> : ICollectionProperty<TValue>
    {
        protected readonly HashSet<TValue> collection = new();

        public int Count => collection.Count;
        
        public object Owner { get; protected set; }

        public object ObjectValue => collection;

        public event PropertyDirtyHandler OnDirty;
        public event IReadOnlyCollectionProperty<TValue>.ValueAddedHandler OnValueAdded;
        public event IReadOnlyCollectionProperty<TValue>.ValueRemovedHandler OnValueRemoved;

        public void SetOwner(object owner)
        {
            Owner = owner;
        }

        public IEnumerable<TValue> GetValues() => collection;

        public void GetValues(ICollection<TValue> values)
        {
            foreach (var value in collection)
            {
                values.Add(value);
            }
        }

        public bool Add(TValue value, bool initial)
        {
            bool added = collection.Add(value);
            if (added)
            {
                OnValueAdded?.Invoke(Owner, value, initial);
                OnDirty?.Invoke(Owner, initial);
            }

            return added;
        }

        public void AddRange(IEnumerable<TValue> values, bool initial)
        {
            bool dirty = false;
            foreach (var value in values)
            {
                if (collection.Add(value))
                {
                    OnValueAdded?.Invoke(Owner, value, initial);
                    dirty = true;
                }
            }

            if (dirty)
            {
                OnDirty?.Invoke(Owner, initial);
            }
        }

        public bool Remove(TValue value, bool initial)
        {
            bool removed = collection.Remove(value);
            if (removed)
            {
                OnValueRemoved?.Invoke(Owner, value, initial);
                OnDirty?.Invoke(Owner, initial);
            }

            return removed;
        }

        public void Clear()
        {
            if (collection.Count <= 0)
            {
                return;
            }
            
            foreach (var value in collection)
            {
                OnValueRemoved?.Invoke(Owner, value, false);
            }

            collection.Clear();
            OnDirty?.Invoke(Owner, false);
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}