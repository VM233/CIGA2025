using System;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    public interface IRangeChooserConfig<TVector> : IChooserConfig<TVector>, IMinMaxOwner<TVector> 
        where TVector : struct, IEquatable<TVector>
    {
        
    }
}