#if DOTWEEN && UNITASK_DOTWEEN_SUPPORT
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.Animations
{
    [LabelText(nameof(FadeOut))]
    public class FadeOut : Fade
    {
        [LabelWidth(180)]
        public bool setAlphaToOneOnStart = false;

        protected override UniTask Run(CanvasGroup canvasGroup, CancellationToken token)
        {
            return canvasGroup.DOFade(0, fadeDuration).AwaitForComplete(cancellationToken: token);
        }

        protected override void OnStart(CanvasGroup canvasGroup)
        {
            if (setAlphaToOneOnStart)
            {
                canvasGroup.alpha = 1;
            }
        }
    }
}
#endif