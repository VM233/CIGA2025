using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace VMFramework.Properties
{
    public class ReadOnlyKeyValueProperty<TKey, TValue> : IReadOnlyProperty<TKey, TValue>
    {
        public object Owner { get; private set; }

        public object ObjectValue => null;

        public event PropertyDirtyHandler OnDirty;

        protected Func<TKey, TValue> getter;

        public void Initialize([DisallowNull] Func<TKey, TValue> getter)
        {
            this.getter = getter;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetOwner(object owner)
        {
            Owner = owner;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MarkDirty(bool initial)
        {
            OnDirty?.Invoke(Owner, initial);
        }

        public TValue GetValue(TKey argument)
        {
            return getter(argument);
        }
    }
}