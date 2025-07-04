using VMFramework.Configuration;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.UI
{
    public abstract partial class TitleContentDebugEntry : DebugEntry
    {
        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [JsonProperty]
        public bool displayTitle = true;

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [ShowIf(nameof(displayTitle))]
        [SerializeField, JsonProperty]
        protected TextTagFormat titleFormat = new()
        {
            overrideFontColor = true,
            fontColor = Color.white
        };

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [SerializeField, JsonProperty]
        protected TextTagFormat contentFormat = new()
        {
            overrideFontColor = true,
            fontColor = Color.white
        };
        
        protected virtual string GetTitle() => name?.GetLocalizedString();

        protected abstract string GetContent();
        
        public sealed override string GetText()
        {
            if (displayTitle)
            {
                return titleFormat.GetText(GetTitle() + ":") +
                       contentFormat.GetText(GetContent());
            }
            else
            {
                return contentFormat.GetText(GetContent());
            }
        }
    }
}
