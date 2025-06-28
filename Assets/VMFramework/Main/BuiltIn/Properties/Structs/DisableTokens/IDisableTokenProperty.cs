using System.Diagnostics.CodeAnalysis;
using VMFramework.Core;

namespace VMFramework.Properties
{
    public interface IDisableTokenProperty
    {
        public delegate void EnabledChangedHandler(object owner, bool previous, bool current);
        
        public bool IsEnabled { get; }

        public event EnabledChangedHandler OnEnabledChangedEvent;

        public void AddDisableToken([DisallowNull] IToken token);

        public void RemoveDisableToken([DisallowNull] IToken token);
        
        public bool ContainsToken([DisallowNull] IToken token);
    }
}