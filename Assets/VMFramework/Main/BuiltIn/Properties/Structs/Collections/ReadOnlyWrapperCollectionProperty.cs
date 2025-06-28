using System.Collections;
using System.Collections.Generic;

namespace VMFramework.Properties
{
    public class ReadOnlyWrapperCollectionProperty<TValue> : IReadOnlyCollectionProperty<TValue>, IProperty
    {
        public IReadOnlyCollectionProperty<TValue> WrappedProperty { get; protected set; }

        public int Count => WrappedProperty.Count;

        public object Owner { get; protected set; }

        public object ObjectValue => WrappedProperty.ObjectValue;

        public event PropertyDirtyHandler OnDirty;
        public event IReadOnlyCollectionProperty<TValue>.ValueAddedHandler OnValueAdded;
        public event IReadOnlyCollectionProperty<TValue>.ValueRemovedHandler OnValueRemoved;

        protected readonly IReadOnlyCollectionProperty<TValue>.ValueAddedHandler onWrapperValueAddedFunc;
        protected readonly IReadOnlyCollectionProperty<TValue>.ValueRemovedHandler onWrapperValueRemovedFunc;

        public ReadOnlyWrapperCollectionProperty()
        {
            onWrapperValueAddedFunc = OnWrapperValueAdded;
            onWrapperValueRemovedFunc = OnWrapperValueRemoved;
        }

        public void SetOwner(object owner)
        {
            Owner = owner;
        }

        public void GetValues(ICollection<TValue> values) => WrappedProperty.GetValues(values);

        public void Set(IReadOnlyCollectionProperty<TValue> wrappedProperty)
        {
            bool isDirty = false;
            if (WrappedProperty != null)
            {
                foreach (var value in WrappedProperty)
                {
                    OnValueRemoved?.Invoke(Owner, value, initial: false);
                }

                isDirty = true;

                WrappedProperty.OnValueAdded -= onWrapperValueAddedFunc;
                WrappedProperty.OnValueRemoved -= onWrapperValueRemovedFunc;
            }

            WrappedProperty = wrappedProperty;

            if (wrappedProperty != null)
            {
                foreach (var value in wrappedProperty)
                {
                    OnValueAdded?.Invoke(Owner, value, initial: true);
                }

                isDirty = true;

                wrappedProperty.OnValueAdded += onWrapperValueAddedFunc;
                wrappedProperty.OnValueRemoved += onWrapperValueRemovedFunc;
            }

            if (isDirty)
            {
                OnDirty?.Invoke(Owner, initial: false);
            }
        }

        protected void OnWrapperValueAdded(object owner, TValue value, bool initial)
        {
            OnValueAdded?.Invoke(Owner, value, initial);
            OnDirty?.Invoke(Owner, initial);
        }

        protected void OnWrapperValueRemoved(object owner, TValue value, bool initial)
        {
            OnValueRemoved?.Invoke(Owner, value, initial);
            OnDirty?.Invoke(Owner, initial);
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return WrappedProperty.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}