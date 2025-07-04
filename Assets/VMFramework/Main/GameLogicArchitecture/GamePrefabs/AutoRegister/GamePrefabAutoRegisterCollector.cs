﻿using System;
using System.Collections.Generic;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    public static class GamePrefabAutoRegisterCollector
    {
        public static IEnumerable<AutoRegisterInfo> Collect()
        {
            foreach (var gamePrefabType in typeof(IGamePrefab).GetDerivedClasses(false, false)
                         .ExcludeAbstractAndInterface())
            {
                foreach (var gamePrefabAutoRegisterAttribute in gamePrefabType
                             .GetAttributes<GamePrefabTypeAutoRegisterAttribute>(false))
                {
                    var id = gamePrefabAutoRegisterAttribute.ID;

                    yield return new AutoRegisterInfo(id, gamePrefabType);
                }
            }

            foreach (var staticClass in AppDomain.CurrentDomain.GetAssemblies().GetAllStaticClasses())
            {
                foreach (var fieldInfo in staticClass.GetFields())
                {
                    if (fieldInfo.FieldType != typeof(string))
                    {
                        continue;
                    }
                    
                    if (fieldInfo.TryGetAttribute(false, out GamePrefabIDAutoRegisterAttribute attribute) ==
                        false)
                    {
                        continue;
                    }
                    
                    if (fieldInfo.IsConstant() == false)
                    {
                        Debugger.LogWarning($"String Field : {fieldInfo.Name} in Class : {staticClass.Name}" +
                                         $"with {nameof(GamePrefabIDAutoRegisterAttribute)}" +
                                         "must be a constant string.");
                        
                        continue;
                    }

                    var id = (string)fieldInfo.GetValue(null);

                    if (id.IsNullOrEmpty())
                    {
                        Debug.LogWarning($"The value of String Field : {fieldInfo.Name} " +
                                         $"in Class : {staticClass.Name} " +
                                         $"with {nameof(GamePrefabIDAutoRegisterAttribute)} " +
                                         $"cannot be null or empty.");
                        
                        continue;
                    }
                    
                    yield return new AutoRegisterInfo(id, attribute.GamePrefabType);
                }
            }
        } 
    }
}