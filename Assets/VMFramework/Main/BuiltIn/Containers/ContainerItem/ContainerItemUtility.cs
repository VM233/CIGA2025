using System.Runtime.CompilerServices;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Containers
{
    public static class ContainerItemUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReturnIfNoSourceContainer<TContainerItem>(this TContainerItem item)
            where TContainerItem : IContainerItem
        {
            if (item.SourceContainer == null)
            {
                GameItemManager.Instance.Return(item);
            }
        }
    }
}