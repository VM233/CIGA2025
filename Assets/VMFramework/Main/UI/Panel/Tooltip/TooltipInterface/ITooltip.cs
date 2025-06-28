namespace VMFramework.UI
{
    public interface ITooltip : IUIPanel
    {
        public void Open(object target, IUIPanel source, TooltipOpenInfo info);

        public void Close(object target);
    }
}