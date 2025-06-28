namespace VMFramework.Properties
{
    public delegate void TimerEndHandler(object owner);
    
    public delegate void TimerDirtyHandler(object owner);
    
    public interface IReadOnlyTimerProperty<out TValue>
    {
        public object Owner { get; }
        
        public event TimerEndHandler OnEnd;
        
        public event TimerDirtyHandler OnDirty;
        
        public TValue GetValue();

        public float GetScale();
    }
}