using VMFramework.Configuration;

namespace VMFramework.UI
{
    public interface ITargetBinder<TTarget> : IConfig
    {
        public bool TryGetTarget(object source, out TTarget target);
    }
}