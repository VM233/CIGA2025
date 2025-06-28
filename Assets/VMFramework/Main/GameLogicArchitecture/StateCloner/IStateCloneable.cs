using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    public interface IStateCloneable : IController
    {
        public void CloneFrom(IStateCloneable stateCloneable);
    }
}