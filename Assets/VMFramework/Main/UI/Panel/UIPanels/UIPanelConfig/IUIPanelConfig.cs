using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public interface IUIPanelConfig : ILocalizedGamePrefab
    {
        public bool IsUnique { get; }
        
        public int SortingOrder { get; }
    }
}