using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Animations;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class UGUIContainerAnimationPanelModifier : PanelModifier
    {
        [BoxGroup(CONFIGS_CATEGORY)]
        [Required]
        public Transform animationContainer;

        [BoxGroup(CONFIGS_CATEGORY)]
        public bool splitContainerAnimation = false;

        [BoxGroup(CONFIGS_CATEGORY)]
        [GamePrefabID(typeof(GameObjectAnimation))]
        [HideIf(nameof(splitContainerAnimation))]
        [IsNotNullOrEmpty]
        public string containerAnimationID;

        [BoxGroup(CONFIGS_CATEGORY)]
        [HideIf(nameof(splitContainerAnimation))]
        public bool autoCloseAfterContainerAnimation = true;

        [BoxGroup(CONFIGS_CATEGORY)]
        [GamePrefabID(typeof(GameObjectAnimation))]
        [ShowIf(nameof(splitContainerAnimation))]
        [IsNotNullOrEmpty]
        public string startContainerAnimationID;

        [BoxGroup(CONFIGS_CATEGORY)]
        [GamePrefabID(typeof(GameObjectAnimation))]
        [ShowIf(nameof(splitContainerAnimation))]
        [IsNotNullOrEmpty]
        public string endContainerAnimationID;

        public GameObjectAnimation ContainerAnimation { get; private set; }
        public GameObjectAnimation StartContainerAnimation { get; private set; }
        public GameObjectAnimation EndContainerAnimation { get; private set; }

        private CancellationTokenSource openingCTS;
        private CancellationTokenSource closingCTS;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            if (splitContainerAnimation)
            {
                StartContainerAnimation =
                    GamePrefabManager.GetGamePrefabStrictly<GameObjectAnimation>(startContainerAnimationID);
                EndContainerAnimation =
                    GamePrefabManager.GetGamePrefabStrictly<GameObjectAnimation>(endContainerAnimationID);
            }
            else
            {
                ContainerAnimation = GamePrefabManager.GetGamePrefabStrictly<GameObjectAnimation>(containerAnimationID);
            }

            Panel.OnOpenEvent += OnOpen;
            Panel.OnPreCloseEvent += OnPreClose;
        }

        private async void OnOpen(IUIPanel panel)
        {
            animationContainer.ResetLocalArguments();

            closingCTS?.Cancel();
            openingCTS = new();

            await AwaitToOpen(openingCTS.Token);

            if (splitContainerAnimation == false)
            {
                if (autoCloseAfterContainerAnimation)
                {
                    panel.Close();
                }
            }
        }

        protected virtual async UniTask AwaitToOpen(CancellationToken token)
        {
            if (splitContainerAnimation)
            {
                await StartContainerAnimation.RunAndAwait(animationContainer, token);
            }
            else
            {
                await ContainerAnimation.RunAndAwait(animationContainer, token);
            }
        }

        private async void OnPreClose(IUIPanel panel)
        {
            openingCTS.Cancel();
            if (NeedToAwaitToClose())
            {
                closingCTS = new();
                await AwaitToClose(closingCTS.Token);
            }
        }

        protected virtual bool NeedToAwaitToClose()
        {
            return splitContainerAnimation;
        }

        protected virtual async UniTask AwaitToClose(CancellationToken token)
        {
            if (splitContainerAnimation)
            {
                StartContainerAnimation.Kill(animationContainer);

                await EndContainerAnimation.RunAndAwait(animationContainer, token);
            }
        }
    }
}