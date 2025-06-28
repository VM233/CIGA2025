﻿using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Containers
{
    public interface IContainerConfig : IGamePrefab
    {
        public int? Capacity { get; }
        
        public RangeInteger? AddAllowedRange { get; }
    }
}