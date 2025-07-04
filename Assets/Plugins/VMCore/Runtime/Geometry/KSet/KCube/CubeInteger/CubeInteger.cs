﻿using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace VMFramework.Core
{
    public partial struct CubeInteger : IKCubeInteger<Vector3Int>, IEquatable<CubeInteger>, IFormattable
    {
        public static CubeInteger Max { get; } = new(CommonVector3Int.minValue, CommonVector3Int.maxValue);

        public static CubeInteger Zero { get; } = new(Vector3Int.zero, Vector3Int.zero);

        public static CubeInteger One { get; } = new(Vector3Int.one, Vector3Int.one);

        public static CubeInteger Unit { get; } = new(Vector3Int.zero, Vector3Int.one);

        public Vector3Int Size => max - min + Vector3Int.one;

        public Vector3Int Pivot => (max + min) / 2;

        public Vector3Int min, max;
        public bool inverseX, inverseY, inverseZ;

        public int Count => Size.Products();

        public RangeInteger XRange => new(min.x, max.x);

        public RangeInteger YRange => new(min.y, max.y);

        public RangeInteger ZRange => new(min.z, max.z);

        public RectangleInteger XYRectangle => new(min.x, min.y, max.x, max.y);

        public RectangleInteger XZRectangle => new(min.x, min.z, max.x, max.z);

        public RectangleInteger YZRectangle => new(min.y, min.z, max.y, max.z);

        #region Constructor

        public CubeInteger(RangeInteger xRange, RangeInteger yRange, RangeInteger zRange) : this(
            new Vector3Int(xRange.min, yRange.min, zRange.min), new Vector3Int(xRange.max, yRange.max, zRange.max))
        {

        }

        public CubeInteger(RectangleInteger xyRectangle, RangeInteger zRange) : this(
            new Vector3Int(xyRectangle.min.x, xyRectangle.min.y, zRange.min),
            new Vector3Int(xyRectangle.max.x, xyRectangle.max.y, zRange.max))
        {

        }

        public CubeInteger(RangeInteger xRange, RectangleInteger yzRectangle) : this(
            new Vector3Int(xRange.min, yzRectangle.min.x, yzRectangle.min.y),
            new Vector3Int(xRange.max, yzRectangle.max.x, yzRectangle.max.y))
        {

        }

        public CubeInteger(int xMin, int yMin, int zMin, int xMax, int yMax, int zMax) : this(
            new Vector3Int(xMin, yMin, zMin), new Vector3Int(xMax, yMax, zMax))
        {

        }

        public CubeInteger(Vector3Int min, Vector3Int max)
        {
            this.min = min;
            this.max = max;
            inverseX = false;
            inverseY = false;
            inverseZ = false;
        }

        public CubeInteger(int width, int length, int height) : this(Vector3Int.zero,
            new Vector3Int(width - 1, length - 1, height - 1))
        {

        }

        public CubeInteger(Vector3Int size) : this(Vector3Int.zero, size - Vector3Int.one)
        {

        }

        public CubeInteger(CubeInteger source) : this(source.min, source.max)
        {

        }

        public CubeInteger([DisallowNull] IMinMaxOwner<Vector3Int> config) : this(config.Min, config.Max)
        {

        }

        #endregion

        #region Equatable

        public bool Equals(CubeInteger other)
        {
            return min.Equals(other.min) && max.Equals(other.max);
        }

        public override bool Equals(object obj)
        {
            return obj is CubeInteger other && Equals(other);
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