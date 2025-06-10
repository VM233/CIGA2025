#if UNITY_EDITOR
namespace VMFramework.OdinExtensions
{
    public interface IMaximumValueProvider
    {
        public bool CanClampByMaximum => true;
        
        public void ClampByMaximum(double maximum);
    }
}
#endif