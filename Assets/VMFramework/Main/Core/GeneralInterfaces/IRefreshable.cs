namespace VMFramework.Core
{
    public interface IRefreshable
    {
        public delegate void RefreshHandler(IRefreshable refreshable);
        
        public event RefreshHandler OnRefreshed;
        
        public void Refresh();
    }
}