#if DOTWEEN && UNITASK_DOTWEEN_SUPPORT
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.Core;

namespace VMFramework.Animations
{
    [LabelText(nameof(Leap))]
    public partial class Leap : GameObjectAnimationClip
    {
        [MinValue(0)]
        public float leapDuration = 0.7f;

        public IChooserConfig<Vector3> leapEndOffset = new SingleValueChooserConfig<Vector3>();

        [MinValue(0)]
        public IChooserConfig<float> leapPower = new SingleValueChooserConfig<float>();

        [MinValue(0)]
        public IChooserConfig<int> leapTimes = new SingleValueChooserConfig<int>(1);

        public override float GetDuration() => leapDuration;

        public override async UniTask Run(Transform target, CancellationToken token)
        {
            await base.Run(target, token);
            
            await target.DOLocalJump(target.localPosition + leapEndOffset.GetRandomItem(),
                    leapPower.GetRandomItem(), leapTimes.GetRandomItem(), leapDuration, false)
                .AwaitForComplete(cancellationToken: token);
        }

        public override void Kill(Transform target)
        {
            target.DOKill();
        }

        public override void CheckSettings()
        {
            base.CheckSettings();
            
            leapEndOffset.CheckSettings();
            leapPower.CheckSettings();
            leapTimes.CheckSettings();
        }

        protected override void OnInit()
        {
            base.OnInit();
            
            leapEndOffset.Init();
            leapPower.Init();
            leapTimes.Init();
        }
    }
}
#endif