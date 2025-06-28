using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    [RequireComponent(typeof(IUIToolkitPanel))]
    public class DraggableUIToolkitPanelModifier : PanelModifier, IDraggablePanelModifier
    {
        public virtual bool EnableDragging => true;

        private Vector2 ReferenceResolution => this.UIDocument().panelSettings.referenceResolution;

        [BoxGroup(CONFIGS_CATEGORY)]
        [VisualElementName]
        [IsNotNullOrEmpty]
        public string draggableAreaName;

        [BoxGroup(CONFIGS_CATEGORY)]
        [VisualElementName]
        [IsNotNullOrEmpty]
        public string draggingContainerName;

        [BoxGroup(CONFIGS_CATEGORY)]
        public bool draggableOverflowScreen = false;

        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        private VisualElement draggableArea;

        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        private VisualElement draggingContainer;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpenEvent += OnOpen;
        }

        private void OnOpen(IUIPanel panel)
        {
            draggableArea = this.RootVisualElement().QueryStrictly(draggableAreaName, nameof(draggableAreaName));

            draggingContainer = this.RootVisualElement()
                .QueryStrictly(draggingContainerName, nameof(draggingContainerName));

            draggableArea.RegisterCallback<MouseDownEvent>(_ => PanelDraggingManager.StartDrag(this));
            draggableArea.RegisterCallback<MouseUpEvent>(_ => PanelDraggingManager.StopDrag(this));
        }

        protected virtual void OnDragStart()
        {

        }

        protected virtual void OnDragStop()
        {

        }

        protected virtual void OnDrag(Vector2 mouseDelta, Vector2 mousePosition)
        {
            Vector2 screenSize = new(Screen.width, Screen.height);

            if (mousePosition.IsOverflow(Vector2.zero, screenSize))
            {
                PanelDraggingManager.StopDrag(this);
            }

            Vector2 boundsSize = ReferenceResolution;

            Vector2 delta = mouseDelta.Divide(screenSize).Multiply(boundsSize);

            var left = draggingContainer.resolvedStyle.left;
            var bottom = -draggingContainer.resolvedStyle.bottom;

            var width = draggingContainer.resolvedStyle.width;
            var height = draggingContainer.resolvedStyle.height;

            var resultLeft = left + delta.x;
            var resultBottom = bottom + delta.y;

            if (draggableOverflowScreen == false)
            {
                resultLeft = resultLeft.Clamp(0, boundsSize.x - width);
                resultBottom = resultBottom.Clamp(0, boundsSize.y - height);
            }

            draggingContainer.SetPosition(new Vector2(resultLeft, resultBottom));
        }

        void IDraggablePanelModifier.OnDragStart()
        {
            OnDragStart();
        }

        void IDraggablePanelModifier.OnDragStop()
        {
            OnDragStop();
        }

        void IDraggablePanelModifier.OnDrag(Vector2 mouseDelta, Vector2 mousePosition)
        {
            OnDrag(mouseDelta, mousePosition);
        }
    }
}