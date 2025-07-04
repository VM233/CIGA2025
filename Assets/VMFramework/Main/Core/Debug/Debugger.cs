﻿using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public class Debugger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Log(object message)
        {
            Debug.Log(message);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Log(object message, Object context)
        {
            Debug.Log(message, context);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogWarning(object message)
        {
            Debug.LogWarning(message);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogWarning(object message, Object context)
        {
            Debug.LogWarning(message, context);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogError(object message)
        {
            Debug.LogError(message);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogError(object message, Object context)
        {
            Debug.LogError(message, context);
        }
    }
}