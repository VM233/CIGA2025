using Sirenix.OdinInspector;
using VMFramework.Configuration;

namespace VMFramework.Containers
{
    public class GeneralContainerSlotFilter : ContainerSlotFilter
    {
        [Required]
        public ObjectFilter filter;

        public override IFilter GetFilter()
        {
            return filter;
        }
    }
}