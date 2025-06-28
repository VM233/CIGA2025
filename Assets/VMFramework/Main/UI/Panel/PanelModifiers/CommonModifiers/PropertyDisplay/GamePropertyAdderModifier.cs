using System.Collections.Generic;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public class GamePropertyAdderModifier : PanelModifier, IFuncTargetsProcessor<object, PropertyDisplayInfo>
    {
        protected PropertyDisplayPipelineModifier pipelineModifier;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            pipelineModifier = GetComponent<PropertyDisplayPipelineModifier>();
            pipelineModifier.PropertyInfoPipeline.AddProcessor(this, PriorityDefines.MEDIUM);
        }

        public virtual void ProcessTargets(IReadOnlyCollection<object> targets, ICollection<PropertyDisplayInfo> results)
        {
            foreach (var target in targets)
            {
                BuiltInModulesSetting.TooltipPropertyGeneralSetting.GetGameObjectTooltipProperties(target, results);
            }
        }
    }
}