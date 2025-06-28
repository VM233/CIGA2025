#if DOTWEEN && UNITASK_DOTWEEN_SUPPORT
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.Animations
{
    [LabelText(nameof(Move))]
    public partial class Move : GameObjectAnimationClip
    {
        [MinValue(0)]
        public float moveDuration = 0.3f;

        public IChooserConfig<Vector3> end = new SingleValueChooserConfig<Vector3>();

        [Helper("https://easings.net/")]
        public Ease ease = Ease.Linear;

        public override float GetDuration() => moveDuration;

        public override async UniTask Run(Transform target, CancellationToken token)
        {
            await base.Run(target, token);
            
            await target.DOLocalMove(target.localPosition + end.GetRandomItem(), moveDuration).SetEase(ease)
                .AwaitForComplete(cancellationToken: token);
        }

        public override void Kill(Transform target)
        {
            target.DOKill();
        }

        public override void CheckSettings()
        {
            base.CheckSettings();
            
            end.CheckSettings();
        }

        protected override void OnInit()
        {
            base.OnInit();
            
            end.Init();
        }
    }
}
#endif