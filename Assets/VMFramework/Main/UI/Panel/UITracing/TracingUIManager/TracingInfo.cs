using Sirenix.OdinInspector;

namespace VMFramework.UI
{
    internal class TracingInfo
    {
        [ShowInInspector]
        public TracingConfig Config { get; private set; }

        public int tracingCount = 1;

        public void SetConfig(TracingConfig config)
        {
            Config = config;
        }
    }
}