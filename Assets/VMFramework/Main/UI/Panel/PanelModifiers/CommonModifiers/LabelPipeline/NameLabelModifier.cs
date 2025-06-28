using System.Collections.Generic;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public class NameLabelModifier : PanelModifier, IFuncProcessor<object, string>
    {
        protected ILabelPipelineModifier labelPipeline;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            labelPipeline = GetComponent<ILabelPipelineModifier>();
            labelPipeline.LabelTextPipeline.AddProcessor(this, PriorityDefines.MEDIUM);
        }

        public virtual void ProcessTarget(object target, ICollection<string> results)
        {
            if (target is string title)
            {
                results.Add(title);
                return;
            }
            
            string name = null;
            var isGameObject = target.TryAsGameObject(out var targetObject);

            if (isGameObject)
            {
                if (targetObject.TryGetComponent(out INameOwner nameOwnerComponent))
                {
                    name = nameOwnerComponent.Name;
                }
            }
            else
            {
                if (target is INameOwner nameOwner)
                {
                    name = nameOwner.Name;
                }
            }

            if (name.IsNullOrEmpty() == false)
            {
                results.Add(name);
            }

            string id = null;

            if (isGameObject)
            {
                if (targetObject.TryGetComponent(out IIDOwner<string> idOwnerComponent))
                {
                    id = idOwnerComponent.id;
                }
            }
            else
            {
                if (target is IIDOwner<string> idOwner)
                {
                    id = idOwner.id;
                }
            }

            if (id.IsNullOrEmpty() == false)
            {
                results.Add(id);
            }
        }
    }
}