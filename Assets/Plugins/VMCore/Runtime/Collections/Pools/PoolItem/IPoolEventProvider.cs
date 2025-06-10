using System;

namespace VMFramework.Core.Pools
{
    public interface IPoolEventProvider
    {
        public delegate void GetHandler(IPoolEventProvider provider);
        public delegate void ReturnHandler(IPoolEventProvider provider);
        
        public event GetHandler OnGetEvent;
        public event ReturnHandler OnReturnEvent;
    }
}