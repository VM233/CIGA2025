using VMFramework.Properties;

#if FISHNET
namespace VMFramework.Containers
{
    public partial interface IContainer
    {
        public delegate void OpenStateChangedHandler(IContainer container, bool isOpen);
        
        public IProperty<bool> IsOpen { get; }
    }
}
#endif