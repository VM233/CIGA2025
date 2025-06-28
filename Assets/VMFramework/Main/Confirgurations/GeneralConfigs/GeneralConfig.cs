using Sirenix.OdinInspector;

namespace VMFramework.Configuration
{
    public abstract partial class GeneralConfig : SerializedScriptableObject, IConfig
    {
        public bool InitDone { get; private set; } = false;
        
        public virtual void CheckSettings()
        {

        }

        public void Init()
        {
            OnInit();
            
            InitDone = true;
        }

        protected virtual void OnInit()
        {

        }
    }
}