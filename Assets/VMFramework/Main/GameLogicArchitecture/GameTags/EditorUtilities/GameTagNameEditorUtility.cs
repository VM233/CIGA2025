﻿#if UNITY_EDITOR
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture.Editor
{
    public static class GameTagNameEditorUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<ValueDropdownItem> GetAllGameTagsNameList()
        {
            foreach (var gameTagInfo in GameTag.GetAllTags())
            {
                string name = null;

                if (gameTagInfo is INameOwner nameOwner)
                {
                    name = nameOwner.Name;
                }

                if (name.IsNullOrEmpty())
                {
                    name = gameTagInfo.id;
                }
                
                yield return new ValueDropdownItem(name, gameTagInfo.id);
            }
        }
    }
}
#endif