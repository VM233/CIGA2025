using UnityEngine;

namespace VMFramework.Configuration
{
    public abstract class ConfigBehaviour : MonoBehaviour, ICheckableConfig, IInitializableConfig
    {
        public bool InitDone { get; private set; }

        public void Init()
        {
            OnInit();
            
            InitDone = true;
        }

        protected virtual void OnInit()
        {
            
        }

        public virtual void CheckSettings()
        {
            
        }
    }
}