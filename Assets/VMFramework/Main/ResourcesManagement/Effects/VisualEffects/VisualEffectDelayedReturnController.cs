using UnityEngine;
using UnityEngine.VFX;
using VMFramework.Effects;

namespace VMFramework.ResourcesManagement
{
    public class VisualEffectDelayedReturnController : MonoBehaviour
    {
        protected VisualEffect visualEffect;
        protected IEffect effect;

        protected virtual void Awake()
        {
            visualEffect = GetComponent<VisualEffect>();
            effect = GetComponent<IEffect>();
            effect.OnDelayedPreReturn += OnDelayedPreReturn;
        }
        
        protected virtual void OnDelayedPreReturn(IEffect effect, DelayReturnHint hint)
        {
            visualEffect.Stop();
        }
    }
}