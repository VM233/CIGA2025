#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using VMFramework.Core.Linq;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    public partial class GeneralCircularSelectChooserConfig<TWrapper, TItem>
        : ITypeValidationProvider
    {
        protected virtual IEnumerable<ValidationResult> GetValidationResults(GUIContent label)
        {
            if (items.IsNullOrEmpty())
            {
                yield return new($"Circular items list cannot be empty.", ValidateType.Error);
                yield break;
            }

            if (startCircularIndex >= items.Count)
            {
                yield return new(
                    $"Start index cannot be greater than or equal to the number of items in the circular list.",
                    ValidateType.Error);
            }
        }

        IEnumerable<ValidationResult> ITypeValidationProvider.GetValidationResults(GUIContent label)
        {
            return GetValidationResults(label);
        }
    }
}
#endif