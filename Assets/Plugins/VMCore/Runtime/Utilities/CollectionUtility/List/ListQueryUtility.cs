using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class ListQueryUtility
    {
        #region Get

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> Get<T>(this IList<T> list, RangeInteger range)
        {
            foreach (int index in range)
            {
                yield return list[index];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> Get<T>(this IList<T> list, IEnumerable<int> indices)
        {
            foreach (int index in indices)
            {
                yield return list[index];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> Get<T>(this IList<T> list, params int[] indices)
        {
            foreach (int index in indices)
            {
                yield return list[index];
            }
        }

        #endregion

        #region Get Range

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> GetRange<T>(this IList<T> list, int index, int count)
        {
            if (index < 0 || index >= list.Count)
            {
                throw new ArgumentOutOfRangeException($"{nameof(index)}:{index}");
            }

            if (count < 0 || index + count > list.Count)
            {
                throw new ArgumentOutOfRangeException($"{nameof(count)}:{count}");
            }

            for (int i = index; i < index + count; i++)
            {
                yield return list[i];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> GetRange<T>(this IList<T> list, RangeInteger range)
        {
            if (range.min < 0 || range.max >= list.Count)
            {
                throw new ArgumentOutOfRangeException($"{nameof(range)}:{range}");
            }

            for (int i = range.min; i <= range.max; i++)
            {
                yield return list[i];
            }
        }

        #endregion

        #region Get Size Range

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeInteger GetSizeRange<T>(this IList<T> list) => list.Count.GetRange();

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGet<T>(this IReadOnlyList<T> list, int index, out T value)
        {
            if (index < 0 || index >= list.Count)
            {
                value = default;
                return false;
            }

            value = list[index];
            return true;
        }
    }
}