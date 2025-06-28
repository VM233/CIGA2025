namespace VMFramework.GameLogicArchitecture
{
    public interface IStateCloner
    {
        public void CloneFrom(IStateCloner cloner);
    }
}