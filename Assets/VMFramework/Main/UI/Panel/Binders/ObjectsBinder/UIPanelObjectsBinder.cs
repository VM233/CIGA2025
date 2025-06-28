using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core.Pools;

namespace VMFramework.UI
{
    [RequireComponent(typeof(IUIPanel))]
    [DisallowMultipleComponent]
    public class UIPanelObjectsBinder : PanelModifier, IUIPanelObjectsBinder
    {
        public IReadOnlyCollection<object> BindObjects => bindObjects;
        
        public event PanelObjectBindHandler OnBindObjectAdded;
        public event PanelObjectBindHandler OnBindObjectRemoved;

        private IUIPanel panel;
        
        [ShowInInspector]
        protected readonly HashSet<object> bindObjects = new();
        protected readonly HashSet<object> preAddObjects = new();

        protected virtual void Awake()
        {
            panel = GetComponent<IUIPanel>();
            panel.OnPostCloseEvent += OnPostClose;
        }

        protected virtual void Update()
        {
            if (panel.IsOpened == false)
            {
                return;
            }

            if (IsInitialized == false)
            {
                return;
            } 
            
            if (preAddObjects.Count <= 0)
            {
                return;
            }

            var tempObjects = ListPool<object>.Default.Get();
            tempObjects.Clear();
            tempObjects.AddRange(preAddObjects);

            foreach (var obj in tempObjects)
            {
                if (bindObjects.Add(obj))
                {
                    OnBindObjectAdded?.Invoke(panel, obj);
                }
            }
            
            preAddObjects.ExceptWith(tempObjects);
            tempObjects.ReturnToDefaultPool();
        }

        protected virtual void OnPostClose(IUIPanel obj)
        {
            preAddObjects.Clear();
            
            foreach (var objToRemove in bindObjects)
            {
                OnBindObjectRemoved?.Invoke(panel, objToRemove);
            }
            
            bindObjects.Clear();
        }

        public virtual bool ContainsBindObject(object obj, bool includePreAdd)
        {
            if (includePreAdd)
            {
                if (preAddObjects.Contains(obj))
                {
                    return true;
                }
            }
            return bindObjects.Contains(obj);
        }

        public virtual void AddBindObject(object obj)
        {
            preAddObjects.Add(obj);
        }

        public virtual void RemoveBindObject(object obj)
        {
            preAddObjects.Remove(obj);
            if (bindObjects.Remove(obj))
            {
                OnBindObjectRemoved?.Invoke(panel, obj);
            }
        }
    }
}