using System.Collections.Generic;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public delegate void PanelOpenHandler(IUIPanel panel);
    public delegate void PanelCloseHandler(IUIPanel panel);
    public delegate void PanelDestructHandler(IUIPanel panel);
    
    public interface IUIPanel : IControllerGameItem, IToken
    {
        public bool IsUnique { get; }
        
        public bool IsOpened { get; }
        
        public bool UIEnabled { get; }
        
        public IUIPanel SourceUIPanel { get; }
        
        public IReadOnlyCollection<IPanelModifier> Modifiers { get; }

        public event PanelOpenHandler OnOpenEvent;

        public event PanelCloseHandler OnPreCloseEvent;
        
        public event PanelCloseHandler OnPostCloseEvent;
        
        public event PanelDestructHandler OnDestructEvent;

        public void OnOpen(IUIPanel source);
        
        public void OnPreClose();

        public void OnPostClose();

        public void SetEnabled(bool enableState);
    }
}