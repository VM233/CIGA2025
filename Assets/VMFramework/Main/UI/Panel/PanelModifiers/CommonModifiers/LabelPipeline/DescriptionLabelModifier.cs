using System.Collections.Generic;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public class DescriptionLabelModifier : PanelModifier, IFuncProcessor<object, string>
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
            string description = null;
            var isGameObject = target.TryAsGameObject(out var targetObject);

            if (isGameObject)
            {
                if (targetObject.TryGetComponent(out IDescriptionOwner descriptionOwner))
                {
                    description = descriptionOwner.Description;
                }
                else if (targetObject.TryGetComponent(out IIDOwner<string> idOwner))
                {
                    if (GamePrefabManager.TryGetGamePrefab(idOwner.id, out var gamePrefab))
                    {
                        if (gamePrefab is IDescriptionOwner prefabDescriptionOwner)
                        {
                            description = prefabDescriptionOwner.Description;
                        }
                    }
                }
            }
            else
            {
                if (target is IDescriptionOwner descriptionOwner)
                {
                    description = descriptionOwner.Description;
                }
                else if (target is IIDOwner<string> idOwner)
                {
                    if (GamePrefabManager.TryGetGamePrefab(idOwner.id, out var gamePrefab))
                    {
                        if (gamePrefab is IDescriptionOwner prefabDescriptionOwner)
                        {
                            description = prefabDescriptionOwner.Description;
                        }
                    }
                }
            }

            if (description.IsNullOrEmpty() == false)
            {
                results.Add(description);
            }
        }
    }
}