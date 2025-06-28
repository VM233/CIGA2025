using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class UIToolkitLabelPipelineModifier : BinderModifier, ILabelPipelineModifier
    {
        [BoxGroup(CONFIGS_CATEGORY)]
        [VisualElementName(typeof(Label))]
        [IsNotNullOrEmpty]
        public string labelName;
        
        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        public Label Label { get; protected set; }

        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector, EnableGUI]
        public FuncProcessorPipeline<string> LabelTextPipeline { get; protected set; } = new();
        
        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        protected readonly List<string> labelTextResults = new();

        protected override void OnOpen(IUIPanel panel)
        {
            base.OnOpen(panel);
            
            Label = this.RootVisualElement().QueryStrictly<Label>(labelName, nameof(labelName));
            Label.text = "";
        }

        protected override void OnBindTargetAdded(object target)
        {
            base.OnBindTargetAdded(target);

            if (target == null)
            {
                return;
            }
            
            labelTextResults.Clear();
            LabelTextPipeline.ProcessTarget(target, labelTextResults);

            if (labelTextResults.TryFirstNotNullOrEmpty(out var result))
            {
                Label.text = result;
                Label.DisplayFlex();
            }
            else
            {
                Label.DisplayNone();
            }
        }
    }
}