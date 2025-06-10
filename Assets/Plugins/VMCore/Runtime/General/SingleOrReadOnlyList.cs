using System.Collections.Generic;
using VMFramework.Core.Linq;

namespace VMFramework.Core
{
    public readonly struct SingleOrReadOnlyList<TValue>
    {
        public readonly bool isSingle;
        public readonly TValue value;
        public readonly IReadOnlyList<TValue> list;

        public SingleOrReadOnlyList(TValue value)
        {
            isSingle = true;
            this.value = value;
            list = null;
        }

        public SingleOrReadOnlyList(IReadOnlyList<TValue> list)
        {
            isSingle = false;
            value = default;
            this.list = list;
        }
        
        public static implicit operator SingleOrReadOnlyList<TValue>(TValue value)
        {
            return new SingleOrReadOnlyList<TValue>(value);
        }
    }
}