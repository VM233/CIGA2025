using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Configuration;

namespace VMFramework.UI
{
    public class GeneralBindProcessor : MonoBehaviour, IFuncTargetsProcessor<object, object>
    {
        [Required]
        public ObjectFilter filter;

        public virtual void ProcessTargets(IReadOnlyCollection<object> targets, ICollection<object> results)
        {
            foreach (var target in targets)
            {
                if (filter.IsMatch(target))
                {
                    results.Add(target);
                }
            }
        }
    }
}