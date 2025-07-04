﻿#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using VMFramework.OdinExtensions;

namespace VMFramework.GameLogicArchitecture
{
    [TypeValidation(DrawCurrentRect = true)]
    public partial class GamePrefabBehaviour : ITypeValidationProvider
    {
        public IEnumerable<ValidationResult> GetValidationResults(GUIContent label)
        {
            if (IsIDStartsWithPrefix == false)
            {
                yield return new($"ID should start with prefix : {IDPrefix}", ValidateType.Warning);
            }

            if (IsIDEndsWithSuffix == false)
            {
                yield return new($"ID should end with suffix : {IDSuffix}", ValidateType.Warning);
            }
        }
    }
}
#endif