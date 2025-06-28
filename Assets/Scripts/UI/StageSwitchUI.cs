using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.DOTweenExtension;
using VMFramework.OdinExtensions;
using VMFramework.UI;

namespace RoomPuzzle
{
    public class StageSwitchUI : PanelModifier
    {
        [VisualElementName]
        [IsNotNullOrEmpty]
        public string containerName;

        [MinValue(0)]
        public float fadeInDuration = 0.15f;

        [MinValue(0)]
        public float fadeOutDuration = 0.15f;

        [MinValue(0)]
        public float switchDuration = 0.3f;

        protected VisualElement Container { get; set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpenEvent += OnOpen;
        }

        protected virtual async void OnOpen(IUIPanel panel)
        {
            Container = this.RootVisualElement().QueryStrictly(containerName, nameof(containerName));

            var color = Container.style.backgroundColor.value;

            Container.style.backgroundColor = color.ReplaceAlpha(0);

            DOTween.To(getter: () => Container.style.backgroundColor.value.a,
                    setter: x => Container.style.backgroundColor = color.ReplaceAlpha(x), endValue: 1, fadeInDuration)
                .SetTarget(Container);

            await UniTask.WaitForSeconds(fadeInDuration);

            Container.DOKill();

            await UniTask.WaitForSeconds(switchDuration);

            DOTween.To(getter: () => Container.style.backgroundColor.value.a,
                setter: x => Container.style.backgroundColor = color.ReplaceAlpha(x), endValue: 0, fadeOutDuration);

            await UniTask.WaitForSeconds(fadeOutDuration);

            Panel.Close();
        }
    }
}