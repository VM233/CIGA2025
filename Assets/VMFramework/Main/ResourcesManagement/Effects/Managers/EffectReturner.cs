using System.Collections.Generic;
using VMFramework.Core.Pools;
using VMFramework.Procedure;
using VMFramework.Timers;

namespace VMFramework.Effects
{
    [ManagerCreationProvider(ManagerType.ResourcesCore)]
    public class EffectReturner : ManagerBehaviour<EffectReturner>
    {
        protected readonly Stack<GameItemReturnTimer> pool = new();

        protected override void Awake()
        {
            base.Awake();
            
            pool.Clear();
        }

        public virtual void DelayedReturn(IEffect effect, float delay, DelayReturnHint hint)
        {
            effect.DelayedPreReturn(hint);
            if (pool.TryPop(out var timer) == false)
            {
                timer = new GameItemReturnTimer();
            }
            timer.Set(effect, pool);
            TimerManager.Instance.Add(timer, delay);
        }
    }
}