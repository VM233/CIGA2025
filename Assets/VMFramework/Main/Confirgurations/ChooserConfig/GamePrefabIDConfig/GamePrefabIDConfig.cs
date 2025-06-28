using System;
using Sirenix.OdinInspector;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    [Serializable]
    public partial struct GamePrefabIDConfig : IChooserWrapper<string>
    {
        [HideLabel]
        [GamePrefabID]
        [IsNotNullOrEmpty]
        public string id;
        
        public override string ToString() => id;

        string IChooserWrapper<string>.UnboxWrapper() => id;
    }
}