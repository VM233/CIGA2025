using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class ListMatcher
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CountOverlappingElements<T>(this IList<T> head, IList<T> tail)
        {
            var minLength = head.Count.Min(tail.Count);

            var headStartIndex = head.Count - minLength;

            for (var matchStart = headStartIndex; matchStart < head.Count; matchStart++)
            {
                bool matchFound = true;
                for (var i = matchStart; i < head.Count; i++)
                {
                    if (head[i].Equals(tail[i - matchStart]) == false)
                    {
                        matchFound = false;
                        break;
                    }
                }

                if (matchFound)
                {
                    return head.Count - matchStart;
                }
            }

            return 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CountOverlappingElements(this IList<string> head, IList<string> tail,
            StringComparison comparison)
        {
            var minLength = head.Count.Min(tail.Count);

            var headStartIndex = head.Count - minLength;

            for (var matchStart = headStartIndex; matchStart < head.Count; matchStart++)
            {
                bool matchFound = true;
                for (var i = matchStart; i < head.Count; i++)
                {
                    if (head[i].Equals(tail[i - matchStart], comparison) == false)
                    {
                        matchFound = false;
                        break;
                    }
                }

                if (matchFound)
                {
                    return head.Count - matchStart;
                }
            }

            return 0;
        }
    }
}