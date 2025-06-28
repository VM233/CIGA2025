using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class BlankDropperBase : PanelModifier
    {
        [VisualElementName]
        [IsNotNullOrEmpty]
        public string blankElementName;

        public VisualElement BlankElement { get; private set; }
        
        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            Panel.OnOpenEvent += OnOpen;
        }

        private void OnOpen(IUIPanel panel)
        {
            BlankElement = this.RootVisualElement().QueryStrictly(blankElementName, nameof(blankElementName));
            BlankElement.RegisterCallback<MouseDownEvent>(OnDrop);
        }

        protected virtual void OnDrop(MouseDownEvent evt)
        {
            
        }
    }
}