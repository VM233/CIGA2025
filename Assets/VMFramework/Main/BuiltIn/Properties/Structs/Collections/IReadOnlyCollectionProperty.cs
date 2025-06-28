using System.Collections.Generic;

namespace VMFramework.Properties
{
    public interface IReadOnlyCollectionProperty<TValue> : IReadOnlyProperty, IReadOnlyCollection<TValue>
    {
        public delegate void ValueAddedHandler(object owner, TValue value, bool initial);
        public delegate void ValueRemovedHandler(object owner, TValue value, bool initial);
        
        public event ValueAddedHandler OnValueAdded;
        public event ValueRemovedHandler OnValueRemoved;

        public void GetValues(ICollection<TValue> values);
    }
}