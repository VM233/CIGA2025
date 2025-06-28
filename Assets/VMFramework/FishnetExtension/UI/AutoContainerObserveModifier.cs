#if FISHNET
using VMFramework.Configuration;
using VMFramework.Containers;
using VMFramework.Core;
using VMFramework.Network;

namespace VMFramework.UI
{
    public class AutoContainerObserveModifier : BinderModifier<IContainer>, IToken
    {
        public override IFuncTargetsProcessor<object, object> DefaultProcessor => new ContainerBindProcessor();

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            Panel.OnPostCloseEvent += OnPostClose;
        }

        protected override void OnBindTargetAdded(IContainer target)
        {
            if (target is { UUIDOwner: not null })
            {
                LocalObservationManager.Instance.AddObserver(this, target.UUIDOwner);
            }
        }

        protected override void OnBindTargetRemoved(IContainer target)
        {
            if (target is { UUIDOwner: not null })
            {
                LocalObservationManager.Instance.RemoveObserver(this, target.UUIDOwner);
            }
        }
        
        protected virtual void OnPostClose(IUIPanel panel)
        {
            LocalObservationManager.Instance.RemoveObserver(this);
        }
    }
}
#endif