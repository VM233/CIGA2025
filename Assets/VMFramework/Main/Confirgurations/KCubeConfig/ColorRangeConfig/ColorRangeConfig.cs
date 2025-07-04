﻿using System.Runtime.CompilerServices;
using VMFramework.Core;
using UnityEngine;
using Random = System.Random;

namespace VMFramework.Configuration
{
    public partial class ColorRangeConfig : KCubeFloatConfig<Color>
    {
        protected override bool RequireCheckSize => false;

        public override Color Size => max - min;

        public override Color Pivot => (min + max) / 2f;

        public override Color Extents => (max - min) / 2f;

        #region Constructor

        public ColorRangeConfig() : this(new(0, 0, 0, 0), new(0, 0, 0, 0)) { }

        public ColorRangeConfig(Color size)
        {
            min = ColorDefinitions.zero;
            max = size;
            max = max.ClampMin(ColorDefinitions.zero);
        }

        public ColorRangeConfig((Color, Color) minMaxPos) : this(minMaxPos.Item1, minMaxPos.Item2)
        {

        }

        public ColorRangeConfig(Color min, Color max)
        {
            this.min = min;
            this.max = max;
        }

        public ColorRangeConfig(float minX, float minY, float minZ, float minW, float maxX, float maxY, float maxZ, float maxW) :
            this(new(minX, minY, minZ, minW), new(maxX, maxY, maxZ, maxW))
        { }

        #endregion

        #region KCube

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Contains(Color pos) =>
            pos.r >= min.r && pos.r <= max.r &&
            pos.g >= min.g && pos.g <= max.g &&
            pos.b >= min.b && pos.b <= max.b &&
            pos.a >= min.a && pos.a <= max.a;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Color GetRelativePos(Color pos) => pos - min;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Color ClampMin(Color pos) => pos.ClampMin(min);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Color ClampMax(Color pos) => pos.ClampMax(max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Color GetRandomItem(Random random) => random.Range(min, max);

        #endregion

        #region Cloneable

        public override object Clone()
        {
            return new ColorRangeConfig(min, max);
        }

        #endregion
    }
}
