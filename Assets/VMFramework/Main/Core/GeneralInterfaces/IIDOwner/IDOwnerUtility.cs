using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core.Pools;

namespace VMFramework.Core
{
    public static class IDOwnerUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TOwner> SelectWithID<TOwner, TID>(this IEnumerable<TOwner> owners, TID id)
            where TOwner : IIDOwner<TID>
            where TID : IEquatable<TID>
        {
            foreach (var owner in owners)
            {
                if (owner == null)
                {
                    continue;
                }

                if (owner.id.Equals(id))
                {
                    yield return owner;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TOwner> SelectWithAnyIDs<TOwner, TID>(this IEnumerable<TOwner> owners,
            IEnumerable<TID> ids)
            where TOwner : IIDOwner<TID>
            where TID : IEquatable<TID>
        {
            foreach (var owner in owners)
            {
                if (owner == null)
                {
                    continue;
                }

                foreach (var id in ids)
                {
                    if (owner.id.Equals(id))
                    {
                        yield return owner;
                        break;
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BuildUniqueIDDictionary<TOwner>(this IEnumerable<TOwner> owners,
            out Dictionary<string, TOwner> idDictionary)
            where TOwner : IIDOwner<string>
        {
            idDictionary = new();

            foreach (var owner in owners)
            {
                if (owner == null)
                {
                    continue;
                }

                idDictionary[owner.id] = owner;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BuildIDDictionary<TOwner>(this IEnumerable<TOwner> owners,
            out Dictionary<string, List<TOwner>> idDictionary)
            where TOwner : IIDOwner<string>
        {
            idDictionary = DictionaryPool<string, List<TOwner>>.Default.Get();
            idDictionary.Clear();

            foreach (var owner in owners)
            {
                if (owner == null)
                {
                    continue;
                }

                var list = idDictionary.GetOrAddFromDefaultPool(owner.id);
                list.Add(owner);
            }
        }
    }
}