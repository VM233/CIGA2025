#if FISHNET
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.Procedure;

namespace VMFramework.Network
{
    [ManagerCreationProvider(ManagerType.NetworkCore)]
    public class LocalObservationManager : ManagerBehaviour<LocalObservationManager>
    {
        [ShowInInspector]
        [DictionaryDrawerSettings(IsReadOnly = true)]
        private readonly Dictionary<IUUIDOwner, HashSet<IToken>> observations = new();
        [ShowInInspector]
        [DictionaryDrawerSettings(IsReadOnly = true)]
        private readonly Dictionary<IToken, HashSet<IUUIDOwner>> observers = new();

        protected override void Awake()
        {
            base.Awake();
            
            observations.Clear();
            observers.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddObserver([DisallowNull] IToken token,[DisallowNull] IUUIDOwner owner)
        {
            var added = observations.AddValueToValueCollection(owner, token);
            observers.AddValueToValueCollection(token, owner);

            if (added)
            {
                UUIDCoreManager.Instance.Observe(owner);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveObserver([DisallowNull] IToken token, [DisallowNull] IUUIDOwner owner)
        {
            observations.RemoveValueAndRemoveKeyIfEmpty(owner, token, out var collectionRemoved);
            observers.RemoveValueAndRemoveKeyIfEmpty(token, owner);

            if (collectionRemoved)
            {
                UUIDCoreManager.Instance.Unobserve(owner);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveObserver([DisallowNull] IToken token)
        {
            if (observers.TryGetValue(token, out var owners) == false)
            {
                return;
            }

            foreach (var owner in owners)
            {
                observations.RemoveValueAndRemoveKeyIfEmpty(owner, token, out var collectionRemoved);
                if (collectionRemoved)
                {
                    UUIDCoreManager.Instance.Unobserve(owner);
                }
            }
            
            observers.Remove(token);
        }
    }
}
#endif