using UnityEngine;
using VMFramework.Configuration;

namespace VMFramework.UI
{
    public interface IIconPipelineModifier : IPanelModifier
    {
        public FuncProcessorPipeline<Sprite> IconPipeline { get; }
    }
}