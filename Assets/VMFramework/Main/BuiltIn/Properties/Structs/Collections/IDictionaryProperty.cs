#nullable enable
using System.Collections.Generic;

namespace VMFramework.Properties
{
    public interface IDictionaryProperty<TKey, TValue>
        : IReadOnlyDictionaryProperty<TKey, TValue>, ICollectionProperty<KeyValuePair<TKey, TValue>>,
            IProperty<TKey, TValue>
    {

    }
}