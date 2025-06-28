using VMFramework.Configuration;

namespace VMFramework.UI
{
    public interface ILabelPipelineModifier : IPanelModifier
    {
        public FuncProcessorPipeline<string> LabelTextPipeline { get; }
    }
}