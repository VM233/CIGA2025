using System;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.Timers
{
    public partial class Timer : ITimer<double>
    {
        private readonly Action<Timer> onTimed;
        private readonly Action<Timer> onStopped;
        private double priority;
        
        public double ExpectedTime => priority;
        
        public double StoppedTime { get; private set; }
        
        public double StartedTime { get; private set; }

        public Timer(Action<Timer> onTimed = null, Action<Timer> onStopped = null)
        {
            this.onTimed = onTimed;
            this.onStopped = onStopped;
        }

        #region Interface Implementation

        double IGenericPriorityQueueNode<double>.Priority
        {
            get => priority;
            set => priority = value;
        }

        int IGenericPriorityQueueNode<double>.QueueIndex { get; set; }

        long IGenericPriorityQueueNode<double>.InsertionIndex { get; set; }

        void ITimer<double>.OnStart(double startedTime, double expectedTime)
        {
            StartedTime = startedTime;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ITimer<double>.OnTimed()
        {
            onTimed?.Invoke(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ITimer<double>.OnStopped(double stoppedTime)
        {
            StoppedTime = stoppedTime;
            onStopped?.Invoke(this);
        }

        #endregion
    }
}