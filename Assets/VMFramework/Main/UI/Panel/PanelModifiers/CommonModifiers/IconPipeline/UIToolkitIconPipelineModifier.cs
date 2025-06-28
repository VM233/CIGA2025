using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class UIToolkitIconPipelineModifier : BinderModifier, IIconPipelineModifier
    {
        [BoxGroup(CONFIGS_CATEGORY)]
        public bool autoHideContainer = true;
        
        [BoxGroup(CONFIGS_CATEGORY)]
        [VisualElementName]
        [IsNotNullOrEmpty]
        public string iconName;

        [BoxGroup(CONFIGS_CATEGORY)]
        [VisualElementName]
        [IsNotNullOrEmpty]
        public string iconContainerName;
        
        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        public VisualElement Icon { get; protected set; }
        
        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        public VisualElement IconContainer { get; protected set; }
        
        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector, EnableGUI]
        public FuncProcessorPipeline<Sprite> IconPipeline { get; protected set; } = new();

        protected readonly List<Sprite> iconResults = new();

        protected override void OnOpen(IUIPanel panel)
        {
            base.OnOpen(panel);

            Icon = this.RootVisualElement().QueryStrictly(iconName, nameof(iconName));
            IconContainer = this.RootVisualElement().QueryStrictly(iconContainerName, nameof(iconContainerName));

            if (autoHideContainer)
            {
                IconContainer.DisplayNone();
            }
        }

        protected override void OnBindTargetAdded(object target)
        {
            base.OnBindTargetAdded(target);

            if (target == null)
            {
                return;
            }
            
            iconResults.Clear();
            IconPipeline.ProcessTarget(target, iconResults);

            if (iconResults.TryFirstNotUnityNull(out var icon))
            {
                Icon.style.backgroundImage = new StyleBackground(icon);
                IconContainer.DisplayFlex();
            }
            else
            {
                Icon.style.backgroundImage = null;
                if (autoHideContainer)
                {
                    IconContainer.DisplayNone();
                }
            }
        }
    }
}