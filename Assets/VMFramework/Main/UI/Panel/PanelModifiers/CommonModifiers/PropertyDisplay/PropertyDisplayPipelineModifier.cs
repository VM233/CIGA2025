using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Configuration;
using VMFramework.Core;

namespace VMFramework.UI
{
    public class PropertyDisplayPipelineModifier : BinderModifier, IRefreshable
    {
        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector, EnableGUI]
        public FuncTargetsProcessorPipeline<PropertyDisplayInfo> PropertyInfoPipeline { get; protected set; } = new();
        
        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector, EnableGUI]
        public ActionProcessorPipeline<IPropertyDisplayModifier> PropertyGroupPipeline { get; protected set; } = new();
        
        public event IRefreshable.RefreshHandler OnRefreshed;

        protected IPropertyDisplayModifier propertyDisplayModifier;

        protected readonly List<PropertyDisplayInfo> infos = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            propertyDisplayModifier = GetComponent<IPropertyDisplayModifier>();
        }

        protected override void OnBindObjectAdded(IUIPanel panel, object obj)
        {
            base.OnBindObjectAdded(panel, obj);
            
            Refresh();
        }

        protected virtual void OnRefresh()
        {
            propertyDisplayModifier.Clear();

            var bindTargets = GetBindTargets();

            if (bindTargets.Count <= 0)
            {
                return;
            }
            
            infos.Clear();
            PropertyInfoPipeline.ProcessTargets(bindTargets, infos);

            foreach (var info in infos)
            {
                propertyDisplayModifier.AddProperty(info);
            }
            
            PropertyGroupPipeline.ProcessTarget(propertyDisplayModifier);
        }

        public void Refresh()
        {
            if (Panel.IsOpened == false)
            {
                return;
            }
            
            OnRefresh();
            
            OnRefreshed?.Invoke(this);
        }
    }
}