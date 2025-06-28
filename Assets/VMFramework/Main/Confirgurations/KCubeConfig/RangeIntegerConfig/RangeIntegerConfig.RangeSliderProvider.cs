using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    public partial class RangeIntegerConfig : IRangeSliderValueProvider
    {
        float IRangeSliderValueProvider.Min
        {
            get => min;
            set => min = value.Round();
        }

        float IRangeSliderValueProvider.Max
        {
            get => max;
            set => max = value.Round();
        }
    }
}