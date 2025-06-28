using System;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;

namespace VMFramework.Configuration
{
    public abstract class KCubeFloatConfig<TPoint> : KCubeConfig<TPoint>, IKCubeFloatConfig<TPoint>
        where TPoint : struct, IEquatable<TPoint>
    {
        protected virtual string ExtentsName => "Radius";

        [LabelText("@" + nameof(ExtentsName)), VerticalGroup(INFO_VALUE_GROUP)]
        [ShowInInspector, DisplayAsString]
        public abstract TPoint Extents
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }
    }
}