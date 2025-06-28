using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.Timers;

namespace VMFramework.Properties
{
    public class TickTimerProperty : ITimerProperty<uint>, ITimer<ulong>
    {
        [ShowInInspector]
        public object Owner { get; private set; }

        [DelayedProperty]
        [ShowInInspector]
        public uint Value
        {
            get => GetValue();
            set => SetValue(value);
        }
        
        [DelayedProperty]
        [ShowInInspector]
        public float Scale
        {
            get => GetScale();
            set => SetScale(value);
        }
        
        [ShowInInspector, ReadOnly]
        public bool IsTimerActive { get; private set; } = false;

        public event TimerEndHandler OnEnd;
        public event TimerDirtyHandler OnDirty;
        
        protected ulong expectedTime = 0;
        protected uint existingValue = 0;
        protected float scale = 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetOwner(object owner)
        {
            Owner = owner;
        }

        public void SetValue(uint value)
        {
            if (IsTimerActive == false)
            {
                existingValue = value;
                return;
            }
             
            if (value <= 0)
            {
                if (GetValue() > 0)
                {
                    LogicTickTimerManager.Instance.Stop(this);
                    OnDirty?.Invoke(Owner);
                    OnEnd?.Invoke(Owner);
                }

                return;
            }

            LogicTickTimerManager.Instance.TryStop(this);
            LogicTickTimerManager.Instance.Add(this, GetActualValue(value));
            OnDirty?.Invoke(Owner);
        }

        public uint GetValue()
        {
            if (IsTimerActive == false)
            {
                return existingValue;
            }

            return (uint)expectedTime.MinusAndClampZero(LogicTickManager.Instance.Tick);
        }

        public float GetOriginalValue()
        {
            return GetValue() / scale;
        }

        public void SetScale(float scale)
        {
            var oldScale = this.scale;
            this.scale = scale;
            if (IsTimerActive)
            {
                var value = GetValue();
                if (value > 0)
                {
                    LogicTickTimerManager.Instance.Stop(this);
                    var originalValue = value / oldScale;
                    var actualValue = GetActualValue(originalValue);
                    LogicTickTimerManager.Instance.Add(this, actualValue);
                }
            }
            
            OnDirty?.Invoke(Owner);
        }

        public float GetScale()
        {
            return scale;
        }

        public void Reset()
        {
            if (IsTimerActive)
            {
                Debugger.LogError($"Timer {nameof(TickTimerProperty)} of {Owner} cannot be reset while active.");
                return;
            }
            
            expectedTime = 0;
            existingValue = 0;
            scale = 1;
        }

        [Button]
        public void StartTimer()
        {
            if (IsTimerActive)
            {
                Debugger.LogWarning($"Timer {this} of {Owner} is already active.");
                return;
            }

            IsTimerActive = true;

            if (existingValue > 0)
            {
                LogicTickTimerManager.Instance.Add(this, GetActualValue(existingValue));
            }
        }

        [Button]
        public void StopTimer()
        {
            if (IsTimerActive == false)
            {
                return;
            }
            
            IsTimerActive = false;
            LogicTickTimerManager.Instance.TryStop(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected uint GetActualValue(double value)
        {
            return (uint)System.Math.Round(value * scale);
        }

        void ITimer<ulong>.OnStart(ulong startedTime, ulong expectedTime)
        {
            this.expectedTime = expectedTime;
        }
        
        void ITimer<ulong>.OnStopped(ulong stoppedTime)
        {
            existingValue = (uint)expectedTime.MinusAndClampZero(stoppedTime);
            expectedTime = stoppedTime;
        }

        void ITimer<ulong>.OnTimed()
        {
            existingValue = 0;
            OnEnd?.Invoke(Owner);
        }

        #region Proprity Queue Node

        ulong IGenericPriorityQueueNode<ulong>.Priority { get; set; }

        int IGenericPriorityQueueNode<ulong>.QueueIndex { get; set; }

        long IGenericPriorityQueueNode<ulong>.InsertionIndex { get; set; }

        #endregion
    }
}