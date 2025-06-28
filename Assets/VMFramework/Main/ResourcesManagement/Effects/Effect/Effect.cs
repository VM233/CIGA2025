using VMFramework.GameLogicArchitecture;

namespace VMFramework.Effects
{
    public partial class Effect : ControllerGameItem, IEffect
    {
        public event IEffect.DelayedPreReturnHandler OnDelayedPreReturn;

        protected override void OnReturn()
        {
            base.OnReturn();

            transform.SetParent(EffectSpawner.Instance.Container);
        }

        public virtual void DelayedPreReturn(DelayReturnHint hint)
        {
            OnDelayedPreReturn?.Invoke(this, hint);
        }
    }
}