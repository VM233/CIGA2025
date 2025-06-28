using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core;

namespace VMFramework.Properties
{
    public class DisableTokenProperty : IDisableTokenProperty
    {
        [ShowInInspector]
        public bool IsEnabled => disabledTokens.Count <= 0;
        
        [ShowInInspector]
        public object Owner { get; protected set; }
        
        public event IDisableTokenProperty.EnabledChangedHandler OnEnabledChangedEvent;
        
        [ShowInInspector]
        protected readonly HashSet<IToken> disabledTokens = new();
        protected IDisableTokenProperty.EnabledChangedHandler onEnabledChangedFunc;

        public void SetOwner(object owner)
        {
            Owner = owner;
        }

        public void Initialize(IDisableTokenProperty.EnabledChangedHandler onEnabledChangedFunc = null)
        {
            this.onEnabledChangedFunc = onEnabledChangedFunc;
        }
        
        public void AddDisableToken(IToken token)
        {
            token.AssertIsNotNull(nameof(token));

            if (disabledTokens.Add(token) == false)
            {
                return;
            }
            
            if (disabledTokens.Count == 1)
            {
                onEnabledChangedFunc?.Invoke(Owner, true, false);
                OnEnabledChangedEvent?.Invoke(Owner, true, false);
            }
        }

        public void RemoveDisableToken(IToken token)
        {
            token.AssertIsNotNull(nameof(token));
            
            if (disabledTokens.Remove(token) == false)
            {
                return;
            }

            if (disabledTokens.Count == 0)
            {
                onEnabledChangedFunc?.Invoke(Owner, false, true);
                OnEnabledChangedEvent?.Invoke(Owner, false, true);
            }
        }

        public bool ContainsToken(IToken token)
        {
            return disabledTokens.Contains(token);
        }

        public void Clear()
        {
            disabledTokens.Clear();
        }
    }
}