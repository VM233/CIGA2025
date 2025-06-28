using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.Core.Pools;

namespace VMFramework.UI
{
    public static class ElementDisabledManager
    {
        private static readonly Dictionary<VisualElement, HashSet<IToken>> disabledTokens = new();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Disable(this VisualElement element, IToken token)
        {
            var tokens = disabledTokens.GetOrAddFromDefaultPool(element);

            if (tokens.Add(token) == false)
            {
                return;
            }
            
            if (tokens.Count == 1)
            {
                element.SetEnabled(false);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Enable(this VisualElement element, IToken token)
        {
            if (disabledTokens.RemoveAndReturnToDefaultPool(element, token))
            {
                element.SetEnabled(true);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetEnabled(this VisualElement element, bool enabled, IToken token)
        {
            if (enabled)
            {
                Enable(element, token);
            }
            else
            {
                Disable(element, token);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClearDisabledTokens(this VisualElement element)
        {
            if (disabledTokens.Remove(element, out var tokens))
            {
                tokens.ReturnToDefaultPool();
            }
        }
    }
}