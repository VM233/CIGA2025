namespace VMFramework.Properties
{
    public interface IEnableStateProperty
    {
        public bool IsEnabled { get; }
        
        public void SetEnabled(bool isEnabled);
    }
}