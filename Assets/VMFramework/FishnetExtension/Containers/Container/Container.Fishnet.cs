#if FISHNET
using VMFramework.Core;
using VMFramework.Properties;

namespace VMFramework.Containers
{
    public partial class Container
    {
        public IProperty<bool> IsOpen => isOpenProperty;

        protected readonly SimpleProperty<bool> isOpenProperty = new();

        public Container()
        {
            isOpenProperty.SetOwner(this);
        }
    }
}
#endif