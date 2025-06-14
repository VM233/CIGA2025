﻿using System;
using System.Diagnostics;

namespace VMFramework.OdinExtensions
{
    [AttributeUsage(AttributeTargets.All)]
    [Conditional("UNITY_EDITOR")]
    public sealed class MinimumAttribute : SingleValidationAttribute
    {
        public double MinValue;
        public string MinExpression;

        public MinimumAttribute(double minValue)
        {
            MinValue = minValue;
        }

        public MinimumAttribute(string minExpression)
        {
            MinExpression = minExpression;
        }
    }
}
