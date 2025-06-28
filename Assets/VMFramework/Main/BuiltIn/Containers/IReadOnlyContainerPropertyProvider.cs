using VMFramework.Properties;

namespace VMFramework.Containers
{
    public interface IReadOnlyContainerPropertyProvider : IContainerProvider
    {
        public new IReadOnlyProperty<IContainer> Container { get; }

        IContainer IContainerProvider.Container => Container.GetValue();
    }
}