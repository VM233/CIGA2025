#if UNITY_EDITOR
namespace VMFramework.OdinExtensions
{
    public interface IMinimumValueProvider
    {
        public bool CanClampByMinimum => true;
        
        public void ClampByMinimum(double minimum);
    }
}
#endif