using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;
using VMFramework.UI;

namespace RoomPuzzle
{
    public class Popup : PanelModifier
    {
        [VisualElementName(typeof(Label))]
        public string textLabelName;

        public Label TextLabel { get; protected set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            Panel.OnOpenEvent += OnOpen;
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            TextLabel = this.RootVisualElement().QueryStrictly<Label>(textLabelName, nameof(textLabelName));
        }
    }
}