using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using VMFramework.Core.Pools;

namespace VMFramework.Properties
{
    public class DictionaryProperty<TKey, TValue> : IDictionaryProperty<TKey, TValue>
    {
        [ShowInInspector]
        protected readonly Dictionary<TKey, TValue> dict = new();

        public IReadOnlyDictionary<TKey, TValue> Dictionary => dict;

        public object ObjectValue => dict;

        public int Count => dict.Count;

        public TValue this[TKey key]
        {
            get => GetValue(key);
            set => SetValue(key, value, initial: false);
        }

        public object Owner { get; protected set; }

        public event PropertyDirtyHandler OnDirty;
        public event IReadOnlyCollectionProperty<KeyValuePair<TKey, TValue>>.ValueAddedHandler OnValueAdded;
        public event IReadOnlyCollectionProperty<KeyValuePair<TKey, TValue>>.ValueRemovedHandler OnValueRemoved;
        public event PropertyChangedHandler<TKey, TValue> OnChanged;

        public void SetOwner(object owner)
        {
            Owner = owner;
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> GetValues() => dict;

        public TValue GetValue(TKey key) => dict.GetValueOrDefault(key);

        public void GetValues(ICollection<KeyValuePair<TKey, TValue>> values)
        {
            foreach (var kvp in dict)
            {
                values.Add(kvp);
            }
        }

        public void SetValue(TKey key, TValue value, bool initial)
        {
            bool oldValueExists = dict.TryGetValue(key, out var previous);

            dict[key] = value;

            if (oldValueExists == false)
            {
                OnValueAdded?.Invoke(Owner, new KeyValuePair<TKey, TValue>(key, value), initial);
            }

            OnChanged?.Invoke(Owner, hasPrevious: oldValueExists, previous, hasCurrent: true, current: value, initial,
                key);
            OnDirty?.Invoke(Owner, initial);
        }

        public void ChangeToValues(IReadOnlyDictionary<TKey, TValue> newValues, bool initial)
        {
            var keysToRemove = HashSetPool<TKey>.Default.Get();
            keysToRemove.Clear();
            keysToRemove.UnionWith(dict.Keys);
            keysToRemove.ExceptWith(newValues.Keys);

            foreach (var key in keysToRemove)
            {
                Remove(key, initial);
            }

            foreach (var (key, value) in newValues)
            {
                SetValue(key, value, initial);
            }

            keysToRemove.ReturnToDefaultPool();
        }

        public bool Add(KeyValuePair<TKey, TValue> value, bool initial)
        {
            bool added = dict.TryAdd(value.Key, value.Value);
            if (added)
            {
                OnValueAdded?.Invoke(Owner, value, initial);
                OnChanged?.Invoke(Owner, hasPrevious: false, previous: default, hasCurrent: true, current: value.Value,
                    initial, value.Key);
                OnDirty?.Invoke(Owner, initial);
            }

            return added;
        }

        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> values, bool initial)
        {
            bool dirty = false;
            foreach (var value in values)
            {
                if (dict.TryAdd(value.Key, value.Value))
                {
                    OnValueAdded?.Invoke(Owner, value, initial);
                    OnChanged?.Invoke(Owner, hasPrevious: false, previous: default, hasCurrent: true,
                        current: value.Value, initial, value.Key);
                    dirty = true;
                }
            }

            if (dirty)
            {
                OnDirty?.Invoke(Owner, initial);
            }
        }

        public bool Remove(TKey key, bool initial)
        {
            bool removed = dict.Remove(key, out var value);

            if (removed)
            {
                OnChanged?.Invoke(Owner, hasPrevious: true, previous: value, hasCurrent: false, current: default,
                    initial, key);
                OnValueRemoved?.Invoke(Owner, new KeyValuePair<TKey, TValue>(key, value), initial);
                OnDirty?.Invoke(Owner, initial);
            }

            return removed;
        }

        bool ICollectionProperty<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> value, bool initial)
        {
            return Remove(value.Key, initial);
        }

        public void Clear()
        {
            if (dict.Count <= 0)
            {
                return;
            }

            foreach (var value in dict)
            {
                OnChanged?.Invoke(Owner, hasPrevious: true, previous: value.Value, hasCurrent: false, current: default,
                    initial: false, value.Key);
                OnValueRemoved?.Invoke(Owner, value, initial: false);
            }

            dict.Clear();
            OnDirty?.Invoke(Owner, initial: false);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}