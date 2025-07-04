﻿// using System.Collections.Generic;
// using UnityEngine;
// using VMFramework.Core;
//
// namespace VMFramework.GameEvents
// {
//     public abstract class InputGameEvent<TArgument> : ParameterizedGameEvent<TArgument>, IInputGameEvent<TArgument>
//     {
//         protected InputGameEventConfig inputGameEventConfig => (InputGameEventConfig)GamePrefab;
//
//         protected override bool CanPropagate()
//         {
//             if (base.CanPropagate() == false)
//             {
//                 return false;
//             }
//
//             if (inputGameEventConfig.requireMouseInScreen)
//             {
//                 return Input.mousePosition.x.BetweenInclusive(0, Screen.width) &&
//                        Input.mousePosition.y.BetweenInclusive(0, Screen.height);
//             }
//             
//             return true;
//         }
//
//         public abstract IEnumerable<string> GetInputMappingContent(KeyCodeToStringMode mode);
//     }
// }