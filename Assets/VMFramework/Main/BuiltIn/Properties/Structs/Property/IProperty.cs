namespace VMFramework.Properties
{
    public interface IProperty : IReadOnlyProperty
    {
        public void SetOwner(object owner);
    }
    
    public interface IProperty<TValue> : IProperty, IReadOnlyProperty<TValue>
    {
        public TValue Value { get; set; }

        public void SetValue(TValue value, bool initial);
    }
    
    public interface IProperty<in TKey, TValue> : IProperty, IReadOnlyProperty<TKey, TValue>
    {
        public TValue this[TKey argument] { get; set; }
        
        public void SetValue(TKey key, TValue value, bool initial);
    }
}