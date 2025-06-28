using System;
using VMFramework.Procedure;

namespace VMFramework.Timers
{
    [ManagerCreationProvider(ManagerType.TimerCore)]
    public class UpdateDelegateManager : ManagerBehaviour<UpdateDelegateManager>
    {
        public event Action OnFixedUpdateEvent;
        public event Action OnUpdateEvent;
        public event Action OnLateUpdateEvent;
        public event Action OnGUIEvent;

        protected override void Awake()
        {
            base.Awake();

            OnFixedUpdateEvent = null;
            OnUpdateEvent = null;
            OnLateUpdateEvent = null;
            OnGUIEvent = null;
        }

        protected virtual void FixedUpdate()
        {
            OnFixedUpdateEvent?.Invoke();
        }

        protected virtual void Update()
        {
            OnUpdateEvent?.Invoke();
        }

        protected virtual void LateUpdate()
        {
            OnLateUpdateEvent?.Invoke();
        }

        protected virtual void OnGUI()
        {
            OnGUIEvent?.Invoke();
        }
    }
}
