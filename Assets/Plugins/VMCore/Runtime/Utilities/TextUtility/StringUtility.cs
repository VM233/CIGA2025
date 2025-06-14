﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace VMFramework.Core
{

    public static class StringUtility
    {
        public static int GetLineCount(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            return 1 + text.Count(c => c == '\n');
        }

        #region Split & Join

        public static List<string> Split(string oldText, int length)
        {
            var result = new List<string>();

            int totalLength = oldText.Length;
            int amount = totalLength / length;

            for (int i = 0; i < amount; i++)
            {
                result.Add(oldText.Substring(i * length, length));
            }

            if (amount * length < totalLength)
            {
                result.Add(oldText[(amount * length)..]);
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Join<T>(this string separator, IEnumerable<T> enumerable)
        {
            return string.Join(separator, enumerable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Join<T>(this IEnumerable<T> enumerable, string separator)
        {
            return string.Join(separator, enumerable);
        }

        #endregion

        #region Check

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty([NotNullWhen(false)] this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmptyOrWhiteSpace([NotNull] this string str)
        {
            foreach (var c in str)
            {
                if (char.IsWhiteSpace(c) == false)
                {
                    return false;
                }
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty([NotNull] this string str)
        {
            return str == "";
        }

        #endregion

        #region Trim

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string TrimStart(this string str, string trimStr)
        {
            return str.StartsWith(trimStr) ? str[trimStr.Length..] : str;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string TrimEnd(this string str, string trimStr)
        {
            return str.EndsWith(trimStr) ? str[..^trimStr.Length] : str;
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToString(this float value, int decimalPlaces)
        {
            return value.ToString("F" + decimalPlaces.ClampMin(0));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToString(this double value, int decimalPlaces)
        {
            return value.ToString("F" + decimalPlaces.ClampMin(0));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToString(this Vector2 value, int decimalPlaces)
        {
            return $"({value.x.ToString(decimalPlaces)},{value.y.ToString(decimalPlaces)})";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToString(this Vector3 value, int decimalPlaces)
        {
            return
                $"({value.x.ToString(decimalPlaces)},{value.y.ToString(decimalPlaces)},{value.z.ToString(decimalPlaces)})";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToString(this Vector4 value, int decimalPlaces)
        {
            return
                $"({value.x.ToString(decimalPlaces)},{value.y.ToString(decimalPlaces)},{value.z.ToString(decimalPlaces)}" +
                $"{value.w.ToString(decimalPlaces)})";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToFormattedString<T>(this IEnumerable<T> enumerable, string delimiter = ",")
        {
            return enumerable == null ? "Null" : delimiter.Join(enumerable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToMatrixString<T>(this T[,] array, string delimiter = ",", string columnOpen = "[",
            string columnClose = "]")
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);
            
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < rows; i++)
            {
                builder.Append(columnOpen);
                for (int j = 0; j < cols; j++)
                {
                    builder.Append(array[i, j]);
                    if (j < cols - 1)
                    {
                        builder.Append(delimiter);
                    }
                }

                builder.AppendLine(columnClose);
            }

            return builder.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToHexString(this float num)
        {
            return (num * 255).Round().ToString("X2");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToHexString(this int num)
        {
            return num.ToString("X2");
        }
    }
}