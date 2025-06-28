using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.Procedure;

namespace VMFramework.UI
{
    [ManagerCreationProvider(ManagerType.UICore)]
    public class TooltipManager : ManagerBehaviour<TooltipManager>
    {
        private static TooltipGeneralSetting Setting => UISetting.TooltipGeneralSetting;
        
        [ShowInInspector]
        private readonly List<ITooltip> allTooltips = new();
        
        [ShowInInspector]
        public object CurrentTarget { get; private set; }

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();
            
            UIPanelManager.OnPanelCreatedEvent += OnPanelCreated;
        }

        protected virtual void OnPanelCreated(IUIPanel panel)
        {
            if (panel is ITooltip tooltip)
            {
                allTooltips.Add(tooltip);
            }
        }

        public void Open(object target, IUIPanel source)
        {
            if (target == null)
            {
                return;
            }

            var tooltipID = Setting.GetTooltipID(target);

            if (tooltipID.IsNullOrEmpty())
            {
                return;
            }
            
            if (UIPanelManager.TryGetUniquePanelWithWarning(tooltipID, out ITooltip tooltip) == false)
            {
                return;
            }

            int priority = Setting.GetTooltipPriority(target);
            TooltipOpenInfo info = new()
            {
                priority = priority,
            };
            
            tooltip.Open(target, source, info);
            
            CurrentTarget = target;
        }

        public void Close(object target)
        {
            if (target == null)
            {
                Debugger.LogWarning($"{nameof(target)} is Null");
                return;
            }
            
            foreach (var tooltip in allTooltips)
            {
                tooltip.Close(target);
            }

            if (CurrentTarget == target)
            {
                CurrentTarget = null;
            }
        }
    }
}