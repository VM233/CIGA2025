using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public abstract class TracingPanelModifier : PanelModifier, ITracingPanelModifier
    {
        [SuffixLabel("Left bottom corner is (0, 0)"), BoxGroup(CONFIGS_CATEGORY)]
        [MinValue(0), MaxValue(1)]
        public Vector2 defaultPivot = new(0, 1);

        [BoxGroup(CONFIGS_CATEGORY)]
        public bool enableScreenOverflow;

        [BoxGroup(CONFIGS_CATEGORY)]
        public bool enableAutoMouseTracing = false;

        [BoxGroup(CONFIGS_CATEGORY)]
        [ToggleButtons("Persistent Tracing", "Single-Shot Tracing")]
        public bool persistentTracing = true;
        
        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpenEvent += OnOpen;
            Panel.OnPostCloseEvent += OnPostClose;
        }
        
        protected virtual void OnOpen(IUIPanel panel)
        {
            if (enableAutoMouseTracing)
            {
                TracingUIManager.Instance.StartTracing(this, persistentTracing);
            }
        }
        
        protected virtual void OnPostClose(IUIPanel panel)
        {
            TracingUIManager.Instance.StopTracing(this);
        }
        
        public abstract bool TryUpdatePosition(Vector2 screenPosition);
        
        public abstract void SetPivot(Vector2 pivot);
    }
}