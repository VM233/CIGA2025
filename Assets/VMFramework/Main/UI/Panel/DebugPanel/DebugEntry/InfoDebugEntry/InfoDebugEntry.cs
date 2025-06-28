using Newtonsoft.Json;
using UnityEngine.Localization;

namespace VMFramework.UI
{
    public sealed partial class InfoDebugEntry : TitleContentDebugEntry
    {
        [JsonProperty]
        public LocalizedString content = new();

        protected override string GetContent() => content?.GetLocalizedString();
    }
}
