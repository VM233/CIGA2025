﻿#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    public partial class DefaultContainerItemGenerationConfig<TItem, TItemPrefab>
    {
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            count ??= new SingleValueChooserConfig<int>(1);
        }
        
        protected virtual IEnumerable<ValueDropdownItem> GetItemPrefabNameList()
        {
            return GamePrefabNameListQuery.GetGamePrefabNameListByType(typeof(TItemPrefab));
        }
    }
}
#endif