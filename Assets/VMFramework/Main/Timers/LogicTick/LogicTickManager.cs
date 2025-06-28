using System;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Procedure;

namespace VMFramework.Timers
{
    [ManagerCreationProvider(ManagerType.TimerCore)]
    [DisallowMultipleComponent]
    public class LogicTickManager : ManagerBehaviour<ILogicTickManager>, ILogicTickManager
    {
        public const double DEFAULT_TICK_GAP = 1 / 30f;

        public bool autoStart = true;
        
        public bool enableTickGapOverride;
        
        [ShowIf(nameof(enableTickGapOverride))]
        public double tickGapOverride = DEFAULT_TICK_GAP;
        
        [ShowInInspector, DisplayAsString]
        public double TickGap { get; private set; }
        
        [ShowInInspector, DisplayAsString]
        public bool IsTicking { get; private set; } = false;
        
        [ShowInInspector, DisplayAsString]
        public ulong Tick { get; private set; } = 0;
        
        [ShowInInspector, DisplayAsString]
        public double TimeLeftOver { get; private set; } = 0;

        public event Action OnPreTick;
        public event Action OnTick;
        public event Action OnPostTick;

        protected override void Awake()
        {
            base.Awake();

            IsTicking = false;
            Tick = 0;
            TimeLeftOver = 0;
            TickGap = enableTickGapOverride ? tickGapOverride : DEFAULT_TICK_GAP;
        }

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();

            if (autoStart)
            {
                StartTick();
            }
        }

        public void IncreaseTick()
        {
            Tick++;
            
            OnPreTick?.Invoke();
            
            OnTick?.Invoke();
            
            OnPostTick?.Invoke();
        }

        protected virtual void Update()
        {
            if (IsTicking == false)
            {
                return;
            }
            
            TimeLeftOver += Time.deltaTime;

            while (TimeLeftOver >= tickGapOverride)
            {
                IncreaseTick();
                TimeLeftOver -= tickGapOverride;
            }
        }
        
        public void SetTickGap(float tickGap)
        {
            TickGap = tickGap;
        }

        [Button]
        public void StartTick()
        {
            IsTicking = true;
        }
        
        [Button]
        public void StopTick()
        {
            IsTicking = false;
        }
    }
}