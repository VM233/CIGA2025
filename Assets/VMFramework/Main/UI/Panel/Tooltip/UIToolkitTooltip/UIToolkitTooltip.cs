using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.UI
{
    [RequireComponent(typeof(IUIPanelObjectsBinder))]
    public class UIToolkitTooltip : UIToolkitPanel, ITooltip
    {
        protected UIToolkitTooltipConfig TooltipConfig => (UIToolkitTooltipConfig)GamePrefab;

        [ShowInInspector]
        protected TooltipOpenInfo CurrentOpenInfo { get; private set; }

        private IUIPanelObjectsBinder binder;

        protected override void OnCreate()
        {
            base.OnCreate();
            
            binder = GetComponent<IUIPanelObjectsBinder>();
        }

        public void Open(object target, IUIPanel source, TooltipOpenInfo info)
        {
            if (binder.ContainsBindObject(target, true))
            {
                return;
            }

            if (binder.BindObjects.Count > 0)
            {
                if (info.priority < CurrentOpenInfo.priority)
                {
                    return;
                }
            }

            CurrentOpenInfo = info;

            this.Open(source);
            
            binder.AddBindObject(target);
        }

        public void Close(object target)
        {
            if (binder.ContainsBindObject(target, true))
            {
                this.Close();
            }
        }
    }
}
