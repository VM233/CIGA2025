﻿using System;
using System.Runtime.CompilerServices;
using VMFramework.Core;
using UnityEngine;
using Random = System.Random;

namespace VMFramework.Configuration
{
    /// <summary>
    /// <see cref="RectangleFloat"/>对应的配置版本，
    /// 需要高效的场景请使用<see cref="RectangleFloat"/>
    /// </summary>
    [Serializable]
    public partial class RectangleFloatConfig : KCubeFloatConfig<Vector2>
    {
        public override Vector2 Size => max - min;

        public override Vector2 Pivot => (max + min) / 2f;

        public override Vector2 Extents => (max - min) / 2f;

        public RangeFloat XRange => new(min.x, max.x);

        public RangeFloat YRange => new(min.y, max.y);

        public Vector2 LeftTop => new(min.x, max.y);

        public Vector2 RightBottom => new(max.x, min.y);

        #region Constructor

        public RectangleFloatConfig()
        {
            min = Vector2Int.zero;
            max = Vector2Int.zero;
        }

        public RectangleFloatConfig(Vector2 min, Vector2 max)
        {
            this.min = min;
            this.max = max;
        }

        public RectangleFloatConfig(Vector2 size)
        {
            min = Vector2.zero;
            max = size;
            max = max.ClampMin(0);
        }

        public RectangleFloatConfig(float xMin, float yMin, float xMax, float yMax) :
            this(new(xMin, yMin), new(xMax, yMax))
        {

        }

        #endregion

        #region KCube

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Contains(Vector2 pos) =>
            pos.x >= min.x && pos.x <= max.x &&
            pos.y >= min.y && pos.y <= max.y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Vector2 GetRelativePos(Vector2 pos) => pos - min;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Vector2 ClampMin(Vector2 pos) => pos.ClampMin(min);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Vector2 ClampMax(Vector2 pos) => pos.ClampMax(max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Vector2 GetRandomItem(Random random) => random.Range(min, max);

        #endregion

        #region Cloneable

        public override object Clone()
        {
            return new RectangleFloatConfig(min, max);
        }

        #endregion
    }
}
