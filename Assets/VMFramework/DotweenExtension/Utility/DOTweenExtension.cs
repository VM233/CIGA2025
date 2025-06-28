#if DOTWEEN
using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.DOTweenExtension
{
    public static class DOTweenExtension
    {
        public static void DOKill(this IEnumerable<Transform> targets)
        {
            foreach (var target in targets)
            {
                target.DOKill();
            }
        }

        public static Tween DOCircle(this Transform target, Vector3 center,
            float radius, float startAngle, float duration)
        {
            return DOTween.To(() => startAngle, newAngle =>
            {
                startAngle = newAngle;
                target.transform.position =
                    center.To2D().GetPositionOnCircle(radius, newAngle);
            }, 360, duration).SetTarget(target);
        }

        public static Sequence DOLocalFadeJump(this Transform target, float jumpPower, int jumpNumbers,
            float shrinkEndValue, float duration)
        {
            if (jumpNumbers < 1)
            {
                jumpNumbers = 1;
            }

            var s = DOTween.Sequence();

            float startScaleY = target.localScale.y;
            
            var totalWeights = jumpNumbers * (jumpNumbers + 1) / 2;
            var jumpDurationUnit = duration / totalWeights;
            var jumpPowerUnit = jumpPower / jumpNumbers;
            var shrinkUnit = (startScaleY - shrinkEndValue) / jumpNumbers;
            
            float lastHalfDuration = 0;

            for (int i = 0; i < jumpNumbers; i++)
            {
                var weight = jumpNumbers - i;
                var jumpDuration = jumpDurationUnit * weight;

                var halfDuration = jumpDuration / 2;
                lastHalfDuration = halfDuration;

                var currentShrinkEndValue = startScaleY - shrinkUnit * (jumpNumbers - i);

                var yTween = DOTween
                    .To(getter: () => target.localPosition.y,
                        setter: y => target.localPosition = target.localPosition.ReplaceY(y),
                        endValue: jumpPowerUnit * (jumpNumbers - i), halfDuration).SetEase(Ease.OutQuad).SetRelative()
                    .SetLoops(2, LoopType.Yoyo).SetTarget(target);

                var yScaleTween = DOTween.To(getter: () => target.localScale.y,
                    setter: y => target.localScale = target.localScale.ReplaceY(y), endValue: currentShrinkEndValue,
                    halfDuration).SetEase(Ease.OutExpo).SetLoops(2, LoopType.Yoyo).SetTarget(target).SetInverted();

                s.Append(yTween).Join(yScaleTween);
            }
            
            var lastShrinkTween = DOTween.To(getter: () => target.localScale.y,
                setter: y => target.localScale = target.localScale.ReplaceY(y), endValue: startScaleY,
                lastHalfDuration).SetEase(Ease.OutExpo).SetTarget(target);
            s.Append(lastShrinkTween);

            return s;
        }

        public static Tween To<T>(DOGetter<T> getter, DOSetter<T> setter, T endValue,
            float duration)
        {
            return endValue switch
            {
                float => DOTween.To(getter.ConvertTo<DOGetter<float>>(),
                    setter.ConvertTo<DOSetter<float>>(),
                    endValue.ConvertTo<float>(), duration),
                int => DOTween.To(getter.ConvertTo<DOGetter<int>>(),
                    setter.ConvertTo<DOSetter<int>>(),
                    endValue.ConvertTo<int>(), duration),
                Vector2 => DOTween.To(getter.ConvertTo<DOGetter<Vector2>>(),
                    setter.ConvertTo<DOSetter<Vector2>>(),
                    endValue.ConvertTo<Vector2>(), duration),
                Vector3 => DOTween.To(getter.ConvertTo<DOGetter<Vector3>>(),
                    setter.ConvertTo<DOSetter<Vector3>>(),
                    endValue.ConvertTo<Vector3>(), duration),
                Vector4 => DOTween.To(getter.ConvertTo<DOGetter<Vector4>>(),
                    setter.ConvertTo<DOSetter<Vector4>>(),
                    endValue.ConvertTo<Vector4>(), duration),
                Color => DOTween.To(getter.ConvertTo<DOGetter<Color>>(),
                    setter.ConvertTo<DOSetter<Color>>(),
                    endValue.ConvertTo<Color>(), duration),
                _ => throw new ArgumentException()
            };
        }
    }
}


#endif