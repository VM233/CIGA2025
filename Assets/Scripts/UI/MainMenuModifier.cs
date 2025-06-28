using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;
using VMFramework.UI;

namespace RoomPuzzle
{
    public class MainMenuModifier : PanelModifier
    {
        [VisualElementName(typeof(Button))]
        [IsNotNullOrEmpty]
        public string startGameButtonName;
        
        public Button StartGameButton { get; protected set; }
        
        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            Panel.OnOpenEvent += OnOpen;
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            StartGameButton = this.RootVisualElement()
                .QueryStrictly<Button>(startGameButtonName, nameof(startGameButtonName));
            
            StartGameButton.clicked += OnGameStart;
        }

        protected virtual void OnGameStart()
        {
            GameManager.Instance.StartGame();
        }
    }
}