namespace VMFramework.Configuration
{
    public class SingleValueChooserConfig<TItem> : GeneralSingleValueChooserConfig<TItem, TItem> where TItem : new()
    {
        public SingleValueChooserConfig() : base()
        {
            
        }

        public SingleValueChooserConfig(TItem value) : base(value)
        {
            
        }
        
        protected override TItem UnboxWrapper(TItem wrapper)
        {
            return wrapper;
        }
        
        public static implicit operator SingleValueChooserConfig<TItem>(TItem value)
        {
            return new SingleValueChooserConfig<TItem>(value);
        }
    }

    public class SingleValueChooserConfig<TWrapper, TItem> : GeneralSingleValueChooserConfig<TWrapper, TItem>
        where TWrapper : IChooserWrapper<TItem>
    {
        protected override TItem UnboxWrapper(TWrapper wrapper)
        {
            return wrapper.UnboxWrapper();
        }
    }
}