using System.Collections.Generic;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.Core;

namespace VMFramework.UI
{
    public class IconOwnerIconModifier : PanelModifier, IFuncProcessor<object, Sprite>
    {
        protected IIconPipelineModifier iconPipelineModifier;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            iconPipelineModifier = GetComponent<IIconPipelineModifier>();
            iconPipelineModifier.IconPipeline.AddProcessor(this, PriorityDefines.MEDIUM);
        }

        public virtual void ProcessTarget(object target, ICollection<Sprite> results)
        {
            if (target is IIconOwner iconOwner)
            {
                results.Add(iconOwner.Icon);
                return;
            }

            if (target.TryGetComponent(out IIconOwner iconOwnerComponent))
            {
                results.Add(iconOwnerComponent.Icon);
            }
        }
    }
}