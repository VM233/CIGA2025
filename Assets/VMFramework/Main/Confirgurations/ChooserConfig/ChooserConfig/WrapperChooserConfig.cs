namespace VMFramework.Configuration
{
    public abstract class WrapperChooserConfig<TItem> : GeneralWrapperChooserConfig<TItem, TItem>
    {
        protected sealed override TItem UnboxWrapper(TItem wrapper)
        {
            return wrapper;
        }
    }
    
    public abstract class WrapperChooserConfig<TWrapper, TItem> : GeneralWrapperChooserConfig<TWrapper, TItem>
        where TWrapper : IChooserWrapper<TItem>
    {
        protected override TItem UnboxWrapper(TWrapper wrapper)
        {
            return wrapper.UnboxWrapper();
        }
    }
}