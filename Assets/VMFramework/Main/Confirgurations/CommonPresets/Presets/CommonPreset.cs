﻿using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Configuration
{
    public abstract class CommonPreset : SerializedScriptableObject, INameOwner
    {
        public abstract IEnumerable<ValueDropdownItem> GetDropdownItems();

        string INameOwner.Name => name;
    }
}