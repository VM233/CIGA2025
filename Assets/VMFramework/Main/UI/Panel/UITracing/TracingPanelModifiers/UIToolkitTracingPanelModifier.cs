using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class UIToolkitTracingPanelModifier : TracingPanelModifier
    {
        protected IUIToolkitPanel UIToolkitPanel => (IUIToolkitPanel)Panel;

        protected VisualElement TracingContainer { get; private set; }
        
        [LabelWidth(200), BoxGroup(CONFIGS_CATEGORY)]
        public bool useRightPosition;

        [BoxGroup(CONFIGS_CATEGORY)]
        [LabelWidth(200)]
        public bool useTopPosition;

        [BoxGroup(CONFIGS_CATEGORY)]
        [VisualElementName]
        [IsNotNullOrEmpty]
        public string containerVisualElementName;

        protected override void OnOpen(IUIPanel panel)
        {
            TracingContainer = UIToolkitPanel.RootVisualElement.QueryStrictly(containerVisualElementName,
                nameof(containerVisualElementName));

            base.OnOpen(panel);
        }

        protected override void OnPostClose(IUIPanel panel)
        {
            base.OnPostClose(panel);
            
            TracingContainer = null;
        }
        
        public override bool TryUpdatePosition(Vector2 screenPosition)
        {
            var rootVisualElement = UIToolkitPanel.RootVisualElement;
            var boundsSize = rootVisualElement.GetResolvedSize();

            if (boundsSize.TryGetPositionFromScreenPosition(screenPosition, out var position) == false)
            {
                return false;
            }
            
            var width = TracingContainer.resolvedStyle.width;
            var height = TracingContainer.resolvedStyle.height;

            if (width <= 0 || height <= 0 || width.IsNaN() || height.IsNaN())
            {
                return false;
            }

            var pivot = defaultPivot;
            if (enableScreenOverflow == false)
            {
                position = position.Clamp(boundsSize);
                
                if (position.x < defaultPivot.x * width)
                {
                    pivot.x = (position.x / width).ClampMin(0);
                }
                else if (position.x > boundsSize.x - (1 - defaultPivot.x) * width)
                {
                    pivot.x = (1 - (boundsSize.x - position.x) / width).ClampMax(1);
                }

                if (position.y < defaultPivot.y * height)
                {
                    pivot.y = (position.y / height).ClampMin(0);
                }
                else if (position.y > boundsSize.y - (1 - defaultPivot.y) * height)
                {
                    pivot.y = (1 - (boundsSize.y - position.y) / height).ClampMax(1);
                }
            }

            SetPivot(pivot);
            TracingContainer.SetPosition(position, useRightPosition, useTopPosition, boundsSize);

            return true;
        }

        public override void SetPivot(Vector2 pivot)
        {
            TracingContainer.style.translate = new StyleTranslate(new Translate(
                new Length(-100 * pivot.x, LengthUnit.Percent), new Length(100 * pivot.y, LengthUnit.Percent)));
        }
    }
}