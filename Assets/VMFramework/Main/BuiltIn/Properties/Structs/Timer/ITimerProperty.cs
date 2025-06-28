namespace VMFramework.Properties
{
    public interface ITimerProperty<TValue> : IReadOnlyTimerProperty<TValue>
    {
        public TValue Value { get; set; }
        
        public float Scale { get; set; }
        
        public void SetOwner(object owner);

        public void SetValue(TValue value);

        public void SetScale(float scale);
    }
}