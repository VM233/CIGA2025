#if FISHNET
using System;
using FishNet.Connection;
using FishNet.Serializing;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Network
{
    [DisallowMultipleComponent]
    public partial class UUIDObject : MonoBehaviour, IUUIDOwner, IController, INetworkSerializationReceiver
    {
        [ShowInInspector]
        public Guid UUID { get; private set; }

        Guid IUUIDOwner.UUID
        {
            get => UUID;
            set => UUID = value;
        }

        public event Action<IUUIDOwner, NetworkConnection> OnObservedEvent;
        public event Action<IUUIDOwner, NetworkConnection> OnUnobservedEvent;

        void IUUIDOwner.OnObserved(NetworkConnection connection)
        {
            OnObservedEvent?.Invoke(this, connection);
        }

        void IUUIDOwner.OnUnobserved(NetworkConnection connection)
        {
            OnUnobservedEvent?.Invoke(this, connection);
        }

        void INetworkSerializationReceiver.Write(Writer writer)
        {
            writer.WriteGuidAllocated(UUID);
        }

        void INetworkSerializationReceiver.Read(Reader reader)
        {
            UUID = reader.ReadGuid();
        }
    }
}
#endif