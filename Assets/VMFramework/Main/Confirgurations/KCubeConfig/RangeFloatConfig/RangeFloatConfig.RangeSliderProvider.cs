#if UNITY_EDITOR
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    public partial class RangeFloatConfig : IRangeSliderValueProvider
    {
        float IRangeSliderValueProvider.Min
        {
            get => min;
            set => min = value;
        }

        float IRangeSliderValueProvider.Max
        {
            get => max;
            set => max = value;
        }
    }
}
#endif