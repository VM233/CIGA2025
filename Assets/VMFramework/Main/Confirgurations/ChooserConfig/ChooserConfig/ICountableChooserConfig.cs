using VMFramework.Core;

namespace VMFramework.Configuration
{
    public interface ICountableChooserConfig : IChooserConfig, IAvailableItemsProvider
    {
        
    }

    public interface ICountableChooserConfig<TItem>
        : ICountableChooserConfig, IChooserConfig<TItem>, IAvailableItemsProvider<TItem>
    {

    }
}