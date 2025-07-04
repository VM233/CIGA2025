﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public partial struct RectangleInteger : IKCubeInteger<Vector2Int>, IEquatable<RectangleInteger>, 
        IFormattable
    {
        /// <summary>
        /// 创建一个[0, 0]x[0, 0]的整数矩形
        /// </summary>
        public static RectangleInteger Zero { get; } =
            new(Vector2Int.zero, Vector2Int.zero);

        /// <summary>
        /// 创建一个[1, 1]x[1, 1]的整数矩形
        /// </summary>
        public static RectangleInteger One { get; } =
            new(Vector2Int.one, Vector2Int.one);

        /// <summary>
        /// 创建一个[0, 1]x[0, 1]的整数矩形
        /// </summary>
        public static RectangleInteger Unit { get; } =
            new(Vector2Int.zero, Vector2Int.one);

        public Vector2Int Size => max - min + Vector2Int.one;

        public int Count => Size.Products();

        public Vector2Int Pivot => (max + min) / 2;

        public Vector2Int min, max;

        public RangeInteger XRange => new(min.x, max.x);

        public RangeInteger YRange => new(min.y, max.y);

        #region Constructor

        public RectangleInteger(RangeInteger xRange, RangeInteger yRange)
        {
            min = new Vector2Int(xRange.min, yRange.min);
            max = new Vector2Int(xRange.max, yRange.max);
        }

        public RectangleInteger(int xMin, int yMin, int xMax, int yMax)
        {
            min = new Vector2Int(xMin, yMin);
            max = new Vector2Int(xMax, yMax);
        }

        public RectangleInteger(Vector2Int min, Vector2Int max)
        {
            this.min = min;
            this.max = max;
        }

        public RectangleInteger(int width, int length)
        {
            min = Vector2Int.zero;
            max = new Vector2Int(width - 1, length - 1);
        }

        public RectangleInteger(Vector2Int size)
        {
            min = Vector2Int.zero;
            max = new(size.x - 1, size.y - 1);
        }

        public RectangleInteger(RectangleInteger source)
        {
            min = source.min;
            max = source.max;
        }

        public RectangleInteger(IMinMaxOwner<Vector2Int> config)
        {
            if (config is null)
            {
                min = Vector2Int.zero;
                max = Vector2Int.zero;
                return;
            }
            min = config.Min;
            max = config.Max;
        }

        #endregion

        #region Geometry Extension

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsOnBoundary(Vector2Int pos, out FourTypesDirection directions)
        {
            return pos.IsOnBoundary(min, max, out directions);
        }

        #endregion

        #region Equatable

        public bool Equals(RectangleInteger other)
        {
            return min.Equals(other.min) && max.Equals(other.max);
        }

        public override bool Equals(object obj)
        {
            return obj is RectangleInteger other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(min, max);
        }

        #endregion

        #region To String

        public override string ToString() => $"[{min}, {max}]";

        public string ToString(string format, IFormatProvider formatProvider) =>
            $"[{min.ToString(format, formatProvider)},{max.ToString(format, formatProvider)}]";

        #endregion
    }
}
