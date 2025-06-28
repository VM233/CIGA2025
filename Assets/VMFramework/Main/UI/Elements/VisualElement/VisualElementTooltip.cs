using System;
using UnityEngine.UIElements;
using VMFramework.Core;

namespace VMFramework.UI
{
    public class VisualElementTooltip
    {
        public object TooltipData { get; protected set; }
        
        protected object TempTooltipData { get; set; }
        
        private readonly VisualElement visualElement;
        private readonly Func<object> tooltipDataGetter;

        public UIPanel SourcePanel { get; private set; }

        private bool hasMouseEntered = false;

        public VisualElementTooltip(VisualElement visualElement, Func<object> tooltipDataGetter = null)
        {
            this.visualElement = visualElement;
            this.tooltipDataGetter = tooltipDataGetter;
            this.tooltipDataGetter ??= () =>
            {
                if (visualElement.tooltip.IsNullOrEmpty())
                {
                    return null;
                }

                return visualElement.tooltip;
            };
            
            visualElement.RegisterCallback<MouseEnterEvent>(OnMouseEnterEvent);
            visualElement.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveEvent);
            visualElement.RegisterCallback<DetachFromPanelEvent>(OnDetach);
        }
        
        public void SetTooltip(object tooltipData)
        {
            var oldTooltipData = TooltipData;
            TooltipData = tooltipData;

            if (hasMouseEntered)
            {
                if (oldTooltipData != null)
                {
                    TooltipManager.Instance.Close(oldTooltipData);
                }

                if (TempTooltipData != null)
                {
                    TooltipManager.Instance.Close(TempTooltipData);
                    TempTooltipData = null;
                }
                
                hasMouseEntered = false;
            }
        }

        protected virtual void OnDetach(DetachFromPanelEvent evt)
        {
            if (hasMouseEntered)
            {
                if (TooltipData != null)
                {
                    TooltipManager.Instance.Close(TooltipData);
                }
            }
            
            hasMouseEntered = false;
        }

        private void OnMouseEnterEvent(MouseEnterEvent evt)
        {
            if (TooltipData != null)
            {
                TooltipManager.Instance.Open(TooltipData, SourcePanel);
            }
            else
            {
                if (TempTooltipData != null)
                {
                    TooltipManager.Instance.Close(TempTooltipData);
                }
                TempTooltipData = tooltipDataGetter();
                TooltipManager.Instance.Open(TempTooltipData, SourcePanel);
            }
            
            hasMouseEntered = true;
        }

        private void OnMouseLeaveEvent(MouseLeaveEvent evt)
        {
            if (TooltipData != null)
            {
                TooltipManager.Instance.Close(TooltipData);
            }

            if (TempTooltipData != null)
            {
                TooltipManager.Instance.Close(TempTooltipData);
                TempTooltipData = null;
            }
            
            hasMouseEntered = false;
        }

        public void SetSource(UIPanel source)
        {
            SourcePanel = source;
        }
    }
}