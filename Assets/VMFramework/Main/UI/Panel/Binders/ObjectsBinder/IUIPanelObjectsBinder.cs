using System.Collections.Generic;
using VMFramework.Core;

namespace VMFramework.UI
{
    public delegate void PanelObjectBindHandler(IUIPanel panel, object obj);
    
    public interface IUIPanelObjectsBinder : IController
    {
        public IReadOnlyCollection<object> BindObjects { get; }
        
        public event PanelObjectBindHandler OnBindObjectAdded;
        
        public event PanelObjectBindHandler OnBindObjectRemoved;
        
        public bool ContainsBindObject(object obj, bool includePreAdd);
        
        public void AddBindObject(object obj);
        
        public void RemoveBindObject(object obj);
    }
}