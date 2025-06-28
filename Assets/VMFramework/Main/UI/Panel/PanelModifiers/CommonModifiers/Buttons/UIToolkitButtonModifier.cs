using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public abstract class UIToolkitButtonModifier : PanelModifier
    {
        [BoxGroup(CONFIGS_CATEGORY)]
        [VisualElementName(typeof(Button))]
        [IsNotNullOrEmpty]
        public string buttonName;

        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        private Button button;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpenEvent += OnOpen;
            Panel.OnPostCloseEvent += OnClose;
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            button = this.RootVisualElement().QueryStrictly<Button>(buttonName, nameof(buttonName));

            button.clicked += OnClicked;
        }

        protected virtual void OnClose(IUIPanel panel)
        {
            if (button != null)
            {
                button.clicked -= OnClicked;

                button = null;
            }
        }
        
        protected abstract void OnClicked();
    }
}