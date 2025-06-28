using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace VMFramework.GameLogicArchitecture
{
    public abstract partial class GameTagGroupBase : SerializedScriptableObject
    {
        public abstract IEnumerable<GameTagInfo> GetGameTagInfos();
    }
}