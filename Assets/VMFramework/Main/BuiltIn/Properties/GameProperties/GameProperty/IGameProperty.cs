using VMFramework.GameLogicArchitecture;
using VMFramework.UI;

namespace VMFramework.Properties
{
    public interface IGameProperty : ILocalizedGamePrefab, IIconOwner
    {
        public bool CanGetValueString(object target);
        
        public void GetValueAndNameString(object target, out string nameString, out string valueString, out object source);

        public void GetFullString(object target, out string str, out object source);
    }
}