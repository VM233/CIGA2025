using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class FourTypesDirectionVectorCompareUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FourTypesDirection GetRelativeDirectionTo(this Vector2Int vector, Vector2Int origin)
        {
            FourTypesDirection direction = FourTypesDirection.None;

            if (vector.x > origin.x)
            {
                direction |= FourTypesDirection.Right;
            }
            else if (vector.x < origin.x)
            {
                direction |= FourTypesDirection.Left;
            }
            
            if (vector.y > origin.y)
            {
                direction |= FourTypesDirection.Up;
            }
            else if (vector.y < origin.y)
            {
                direction |= FourTypesDirection.Down;
            }

            return direction;
        }
    }
}