using System.Collections;
using System.Collections.Generic;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Timers
{
    public class GameItemReturnTimer : ITimer<double>
    {
        protected IGameItem gameItem;
        protected Stack<GameItemReturnTimer> pool;

        public void Set(IGameItem gameItem, Stack<GameItemReturnTimer> pool)
        {
            this.gameItem = gameItem;
            this.pool = pool;
        }

        void ITimer<double>.OnTimed()
        {
            GameItemManager.Instance.Return(gameItem);
            pool.Push(this);
        }

        #region Priority Queue Node

        double IGenericPriorityQueueNode<double>.Priority { get; set; }

        int IGenericPriorityQueueNode<double>.QueueIndex { get; set; }

        long IGenericPriorityQueueNode<double>.InsertionIndex { get; set; }

        #endregion
    }
}