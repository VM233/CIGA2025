#if UNITY_EDITOR
using Sirenix.OdinInspector;

namespace VMFramework.Containers
{
    public abstract partial class ContainerItem
    {
        [Button]
        private void RemoveFromContainer()
        {
            if (SourceContainer != null)
            {
                SourceContainer.SetItem(SlotIndex, null);
                this.ReturnIfNoSourceContainer();
            }
        }
    }
}
#endif