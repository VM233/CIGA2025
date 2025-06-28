namespace VMFramework.Configuration
{
    public class CircularSelectChooserConfig<TItem>
        : GeneralCircularSelectChooserConfig<TItem, TItem>, ICircularSelectChooserConfig<TItem>
    {
        protected sealed override TItem UnboxWrapper(TItem wrapper)
        {
            return wrapper;
        }
    }

    public class CircularSelectChooserConfig<TWrapper, TItem> : GeneralCircularSelectChooserConfig<TWrapper, TItem>
        where TWrapper : IChooserWrapper<TItem>
    {
        protected override TItem UnboxWrapper(TWrapper wrapper)
        {
            return wrapper.UnboxWrapper();
        }
    }
}