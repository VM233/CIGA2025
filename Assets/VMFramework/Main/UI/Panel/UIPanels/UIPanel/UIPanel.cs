using System.Collections.Generic;
using VMFramework.Core;
using Sirenix.OdinInspector;
using VMFramework.Core.Linq;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public partial class UIPanel : ControllerGameItem, IUIPanel
    {
        [ShowInInspector]
        public IUIPanelConfig Config => (IUIPanelConfig)GamePrefab;
        
        public bool IsUnique => Config.IsUnique;
        
        [ShowInInspector]
        public bool UIEnabled { get; private set; }

        [ShowInInspector]
        public bool IsOpened { get; protected set; } = false;

        [ShowInInspector]
        public IUIPanel SourceUIPanel { get; private set; }

        public IReadOnlyCollection<IPanelModifier> Modifiers => modifiers;

        public event PanelOpenHandler OnOpenEvent;
        
        public event PanelCloseHandler OnPreCloseEvent;

        public event PanelCloseHandler OnPostCloseEvent;
        
        public event PanelDestructHandler OnDestructEvent;

        [ShowInInspector, HideInEditorMode]
        protected readonly List<IPanelModifier> modifiers = new();

        #region Pool Events

        protected override void OnCreate()
        {
            base.OnCreate();
            
            UIPanelManager.Register(this);

            modifiers.Clear();

            foreach (var modifier in transform.QueryComponentsInChildren<IPanelModifier>(includingSelf: true))
            {
                modifiers.Add(modifier);
            }
            
            modifiers.Sort(modifier => modifier.InitializePriority);

            foreach (var modifier in modifiers)
            {
                modifier.Initialize(this, Config);
            }
        }

        protected override void OnClear()
        {
            base.OnClear();

            if (modifiers.Count > 0)
            {
                foreach (var modifier in modifiers)
                {
                    modifier.Deinitialize();
                }
                
                modifiers.Clear();
            }
            
            UIPanelManager.Unregister(this);
            
            OnDestructEvent?.Invoke(this);
        }

        #endregion

        #region Basic Event

        protected virtual void OnSetEnabled()
        {

        }

        #endregion

        #region Open

        void IUIPanel.OnOpen(IUIPanel source)
        {
            if (IsOpened)
            {
                return;
            }
            
            OnOpen(source);

            SourceUIPanel = source;

            if (SourceUIPanel != null)
            {
                SourceUIPanel.OnPostCloseEvent += UIPanelManager.Close;
            }
            
            IsOpened = true;
            
            OnPostOpen(source);
            
            OnOpenEvent?.Invoke(this);
        }

        protected virtual void OnOpen(IUIPanel source)
        {
            
        }

        protected virtual void OnPostOpen(IUIPanel source)
        {
            
        }

        #endregion

        #region Close

        protected virtual void OnPreClose()
        {
            IsOpened = false;
        }
        
        protected virtual void OnPostClose()
        {
            if (SourceUIPanel != null)
            {
                SourceUIPanel.OnPostCloseEvent -= UIPanelManager.Close;
            }

            SourceUIPanel = null;
        }

        void IUIPanel.OnPreClose()
        {
            OnPreCloseEvent?.Invoke(this);
            OnPreClose();
        }

        void IUIPanel.OnPostClose()
        {
            OnPostCloseEvent?.Invoke(this);
            OnPostClose();
        }

        #endregion
        
        public void SetEnabled(bool enableState)
        {
            if (IsDebugging)
            {
                Debugger.LogWarning($"Trying to set enabled state of {name} to {enableState}");
            }

            UIEnabled = enableState;

            OnSetEnabled();
        }
    }
}