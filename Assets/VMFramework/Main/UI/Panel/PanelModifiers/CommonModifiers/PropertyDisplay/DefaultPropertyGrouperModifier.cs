using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class DefaultPropertyGrouperModifier : PanelModifier, IActionProcessor<IPropertyDisplayModifier>
    {
        [BoxGroup(CONFIGS_CATEGORY)]
        [VisualElementName]
        [IsNotNullOrEmpty]
        public string groupContainerName;
        
        [BoxGroup(CONFIGS_CATEGORY)]
        [IsNotNullOrEmpty]
        public string groupClassName;
        
        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        public VisualElement GroupContainer { get; protected set; }
        
        protected IUIToolkitPanelConfig UIToolkitPanelConfig => (IUIToolkitPanelConfig)UIPanelConfig;
        
        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        protected readonly Dictionary<string, VisualElement> propertyGroups = new();
        protected PropertyDisplayPipelineModifier pipelineModifier;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            pipelineModifier = GetComponent<PropertyDisplayPipelineModifier>();
            pipelineModifier.PropertyGroupPipeline.AddProcessor(this, PriorityDefines.MEDIUM);
            
            Panel.OnOpenEvent += OnOpen;
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            GroupContainer = this.RootVisualElement()
                .QueryStrictly(groupContainerName, nameof(groupContainerName));
        }

        public virtual bool ProcessTarget(IPropertyDisplayModifier target)
        {
            GroupContainer.Clear();
            propertyGroups.Clear();
            
            foreach (var (info, visualElement) in target.AllPropertyElements)
            {
                if (info.groupName.IsNullOrEmpty()) 
                {
                    continue;
                }

                if (propertyGroups.TryGetValue(info.groupName, out var group) == false)
                {
                    group = new VisualElement();

                    if (UIToolkitPanelConfig.IgnoreMouseEvents)
                    {
                        group.pickingMode = PickingMode.Ignore;
                    }
                    
                    group.AddToClassList(groupClassName);
                    GroupContainer.Add(group);
                    propertyGroups[info.groupName] = group;
                }
                
                group.Add(visualElement);
            }
            
            return true;
        }
    }
}