using VMFramework.GameLogicArchitecture;

namespace VMFramework.Effects
{
    public interface IEffect : IControllerGameItem
    {
        public delegate void DelayedPreReturnHandler(IEffect effect, DelayReturnHint hint);
        
        public event DelayedPreReturnHandler OnDelayedPreReturn;
        
        public void DelayedPreReturn(DelayReturnHint hint);
    }
}