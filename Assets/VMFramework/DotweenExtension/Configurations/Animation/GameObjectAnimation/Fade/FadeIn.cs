#if DOTWEEN && UNITASK_DOTWEEN_SUPPORT
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.Animations
{
    [LabelText(nameof(FadeIn))]
    public class FadeIn : Fade
    {
        [LabelWidth(180)]
        public bool setAlphaToZeroOnStart = false;

        protected override UniTask Run(CanvasGroup canvasGroup, CancellationToken token)
        {
            return canvasGroup.DOFade(1, fadeDuration).AwaitForComplete(cancellationToken: token);
        }

        protected override void OnStart(CanvasGroup canvasGroup)
        {
            if (setAlphaToZeroOnStart)
            {
                canvasGroup.alpha = 0;
            }
        }
    }
}
#endif