using System.Runtime.CompilerServices;

namespace VMFramework.Properties
{
    public static class PropertyUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ToggleValue<TProperty>(this TProperty property, bool initial)
            where TProperty : IProperty<bool>
        {
            property.SetValue(!property.GetValue(), initial);
        }
    }
}