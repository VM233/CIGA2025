using Newtonsoft.Json;
using Sirenix.OdinInspector;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.GameLogicArchitecture
{
    public class GameTagInfo : BaseConfig, IIDOwner<string>
    {
        [LabelText("ID")]
        [IsNotNullOrEmpty, IsGameTagID]
        [DelayedProperty]
        [JsonProperty]
        public string id;

        #region Interface Implementation

        string IIDOwner<string>.id => id;

        #endregion
    }
}