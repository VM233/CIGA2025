using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;
using VMFramework.Timers;

namespace VMFramework.Effects
{
    public class AudioEffectRecycler : MonoBehaviour, ITimer<double>
    {
        public float checkInterval = 1f;

        protected IEffect effect;
        protected AudioSource audioSource;

        protected virtual void Awake()
        {
            effect = GetComponent<IEffect>();
            effect.OnGetEvent += OnGet;

            audioSource = GetComponent<AudioSource>();
        }

        protected virtual void OnGet(IPoolEventProvider provider)
        {
            TimerManager.Instance.Add(this, checkInterval);
        }

        public virtual void OnTimed()
        {
            if (audioSource.isPlaying)
            {
                TimerManager.Instance.Add(this, checkInterval);
                return;
            }

            GameItemManager.Instance.Return(effect);
        }

        #region Priority Queue Node

        double IGenericPriorityQueueNode<double>.Priority { get; set; }

        int IGenericPriorityQueueNode<double>.QueueIndex { get; set; }

        long IGenericPriorityQueueNode<double>.InsertionIndex { get; set; }

        #endregion
    }
}