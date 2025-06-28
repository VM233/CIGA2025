namespace VMFramework.Properties
{
    public delegate void PropertyDirtyHandler(object owner, bool initial);

    public delegate void PropertyChangedHandler<in TValue>(object owner, TValue previous, TValue current, bool initial);

    public delegate void PropertyChangedHandler<in TKey, in TValue>(object owner, bool hasPrevious, TValue previous,
        bool hasCurrent, TValue current, bool initial, TKey key);

    public interface IReadOnlyProperty
    {
        public object Owner { get; }
        
        public object ObjectValue { get; }

        public event PropertyDirtyHandler OnDirty;
    }

    public interface IReadOnlyProperty<out TValue> : IReadOnlyProperty
    {
        object IReadOnlyProperty.ObjectValue => GetValue();

        public event PropertyChangedHandler<TValue> OnChanged;

        public TValue GetValue();
    }

    public interface IReadOnlyProperty<in TKey, out TValue> : IReadOnlyProperty
    {
        public TValue GetValue(TKey key);
    }
}