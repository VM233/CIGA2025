#if FISHNET
using System;
using FishNet.Connection;
using FishNet.Object;
using VMFramework.Core;
using VMFramework.Network;

namespace VMFramework.Containers
{
    public partial class ContainerManager
    {
        #region Swap Item

        [Client]
        public void SwapItem(IContainer fromContainer, int fromSlotIndex, IContainer toContainer, int toSlotIndex)
        {
            if (fromContainer == null || toContainer == null)
            {
                Debugger.LogWarning($"{nameof(fromContainer)} or {nameof(toContainer)} is Null");
                return;
            }
            
            fromContainer.SwapItem(fromSlotIndex, toContainer, toSlotIndex);
            
            if (IsServerStarted == false)
            {
                RequestSwapItem(fromContainer.UUIDOwner.UUID, fromSlotIndex, toContainer.UUIDOwner.UUID, toSlotIndex);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void RequestSwapItem(Guid fromContainerUUID, int fromSlotIndex, Guid toContainerUUID, int toSlotIndex,
            NetworkConnection connection = null)
        {
            if (UUIDCoreManager.Instance.TryGetComponentWithWarning(fromContainerUUID, out IContainer fromContainer) ==
                false)
            {
                return;
            }

            if (UUIDCoreManager.Instance.TryGetComponentWithWarning(toContainerUUID, out IContainer toContainer) ==
                false)
            {
                return;
            }

            fromContainer.SwapItem(fromSlotIndex, toContainer, toSlotIndex);
        }

        #endregion
        
        #region Add Or Swap Item

        [Client]
        public void AddOrSwapItem(IContainer fromContainer, int fromSlotIndex, IContainer toContainer, int toSlotIndex)
        {
            if (fromContainer == null || toContainer == null)
            {
                Debugger.LogWarning($"{nameof(fromContainer)} or {nameof(toContainer)} is Null");
                return;
            }

            fromContainer.AddOrSwapItemTo(fromSlotIndex, toContainer, toSlotIndex);

            if (IsServerStarted == false)
            {
                RequestAddOrSwapItem(fromContainer.UUIDOwner.UUID, fromSlotIndex, toContainer.UUIDOwner.UUID,
                    toSlotIndex);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void RequestAddOrSwapItem(Guid fromContainerUUID, int fromSlotIndex, Guid toContainerUUID,
            int toSlotIndex, NetworkConnection connection = null)
        {
            if (UUIDCoreManager.Instance.TryGetComponentWithWarning(fromContainerUUID, out IContainer fromContainer) ==
                false)
            {
                return;
            }

            if (UUIDCoreManager.Instance.TryGetComponentWithWarning(toContainerUUID, out IContainer toContainer) ==
                false)
            {
                return;
            }

            fromContainer.AddOrSwapItemTo(fromSlotIndex, toContainer, toSlotIndex);
        }

        #endregion

        #region Pop Items To

        /// <summary>
        /// Pop Items to Other Container either on Client or Server
        /// </summary>
        /// <param name="fromContainer"></param>
        /// <param name="count"></param>
        /// <param name="toContainer"></param>
        public void PopItemsTo(IContainer fromContainer, int count, IContainer toContainer)
        {
            if (fromContainer == null || toContainer == null)
            {
                Debugger.LogWarning($"{nameof(fromContainer)} or {nameof(toContainer)} is Null");
                return;
            }

            fromContainer.PopItemsTo(count, toContainer, out var remainingCount);

            if (remainingCount < count)
            {
                RequestPopItemsTo(fromContainer.UUIDOwner.UUID, count, toContainer.UUIDOwner.UUID);
            }
            else if (IsClientStarted)
            {
                RequestReconcileAll(fromContainer);
                RequestReconcileAll(toContainer);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void RequestPopItemsTo(Guid fromContainerUUID, int count, Guid toContainerUUID,
            NetworkConnection connection = null)
        {
            if (UUIDCoreManager.Instance.TryGetComponentWithWarning(fromContainerUUID, out IContainer fromContainer) ==
                false)
            {
                return;
            }

            if (UUIDCoreManager.Instance.TryGetComponentWithWarning(toContainerUUID, out IContainer toContainer) ==
                false)
            {
                return;
            }

            // ReSharper disable once PossibleNullReferenceException
            if (connection.IsHost == false)
            {
                fromContainer.PopItemsTo(count, toContainer, out _);
            }
        }

        #endregion

        #region Pop All Items To

        [Client]
        public void PopAllItemsTo(IContainer fromContainer, IContainer toContainer)
        {
            if (fromContainer == null || toContainer == null)
            {
                Debugger.LogWarning($"{nameof(fromContainer)} or {nameof(toContainer)} is Null");
                return;
            }

            fromContainer.TryPopAllItemsTo(toContainer);

            RequestPopAllItemsTo(fromContainer.UUIDOwner.UUID, toContainer.UUIDOwner.UUID);
        }

        [ServerRpc(RequireOwnership = false)]
        public void RequestPopAllItemsTo(Guid fromContainerUUID, Guid toContainerUUID,
            NetworkConnection connection = null)
        {
            if (UUIDCoreManager.Instance.TryGetComponentWithWarning(fromContainerUUID, out IContainer fromContainer) ==
                false)
            {
                return;
            }

            if (UUIDCoreManager.Instance.TryGetComponentWithWarning(toContainerUUID, out IContainer toContainer) ==
                false)
            {
                return;
            }

            // ReSharper disable once PossibleNullReferenceException
            if (connection.IsHost == false)
            {
                fromContainer.TryPopAllItemsTo(toContainer);
            }
        }

        #endregion

        #region Pop Item By Slot Index To

        public void PopItemBySlotIndexTo(IContainer fromContainer, int slotIndex, IContainer toContainer)
        {
            if (fromContainer == null || toContainer == null)
            {
                Debugger.LogWarning($"{nameof(fromContainer)} or {nameof(toContainer)} is Null");
                return;
            }
            
            fromContainer.TryPopItemBySlotIndexTo(slotIndex, toContainer, out _);
            
            RequestPopItemBySlotIndexTo(fromContainer.UUIDOwner.UUID, slotIndex, toContainer.UUIDOwner.UUID);
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void RequestPopItemBySlotIndexTo(Guid fromContainerUUID, int slotIndex, Guid toContainerUUID,
            NetworkConnection connection = null)
        {
            if (UUIDCoreManager.Instance.TryGetComponentWithWarning(fromContainerUUID, out IContainer fromContainer) ==
                false)
            {
                return;
            }

            if (UUIDCoreManager.Instance.TryGetComponentWithWarning(toContainerUUID, out IContainer toContainer) ==
                false)
            {
                return;
            }

            // ReSharper disable once PossibleNullReferenceException
            if (connection.IsHost == false)
            {
                fromContainer.TryPopItemBySlotIndexTo(slotIndex, toContainer, out _);
            }
        }

        #endregion

        [Client]
        public void PopItemByPreferredCountTo(IContainer fromContainer, int slotIndex, int preferredCount,
            IContainer toContainer, int toSlotIndex)
        {
            if (fromContainer == null || toContainer == null)
            {
                Debugger.LogWarning($"{nameof(fromContainer)} or {nameof(toContainer)} is Null");
                return;
            }

            fromContainer.TryPopItemByPreferredCountTo(slotIndex, preferredCount, toContainer, toSlotIndex, out _);

            RequestPopItemByPreferredCountTo(fromContainer.UUIDOwner.UUID, slotIndex, preferredCount,
                toContainer.UUIDOwner.UUID, toSlotIndex);
        }

        [ServerRpc(RequireOwnership = false)]
        public void RequestPopItemByPreferredCountTo(Guid fromContainerUUID, int slotIndex, int preferredCount,
            Guid toContainerUUID, int toSlotIndex, NetworkConnection connection = null)
        {
            if (UUIDCoreManager.Instance.TryGetComponentWithWarning(fromContainerUUID, out IContainer fromContainer) ==
                false)
            {
                return;
            }

            if (UUIDCoreManager.Instance.TryGetComponentWithWarning(toContainerUUID, out IContainer toContainer) ==
                false)
            {
                return;
            }

            // ReSharper disable once PossibleNullReferenceException
            if (connection.IsHost)
            {
                return;
            }

            if (fromContainer.TryPopItemByPreferredCountTo(slotIndex, preferredCount, toContainer, toSlotIndex, poppedCount: out _))
            {
                return;
            }

            ReconcileOnTarget(connection, fromContainer, slotIndex);
            ReconcileOnTarget(connection, toContainer, toSlotIndex);
        }

        #region Stack Item

        [Client]
        public void StackItem(IContainer container)
        {
            if (container == null)
            {
                Debugger.LogWarning($"{nameof(container)} is Null");
                return;
            }

            container.StackItems();

            StackItemRequest(container.UUIDOwner.UUID);
        }

        [ServerRpc(RequireOwnership = false)]
        public void StackItemRequest(Guid containerUUID, NetworkConnection connection = null)
        {
            if (UUIDCoreManager.Instance.TryGetComponentWithWarning(containerUUID, out IContainer container) == false)
            {
                return;
            }

            // ReSharper disable once PossibleNullReferenceException
            if (connection.IsHost == false)
            {
                container.StackItems();
            }
        }

        #endregion
    }
}
#endif