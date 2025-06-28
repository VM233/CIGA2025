using UnityEngine.Localization;
using VMFramework.Localization;

namespace VMFramework.GameLogicArchitecture
{
    public partial class LocalizedGameTagInfo : GameTagInfo, INameOwner, ILocalizedNameOwner
    {
        public LocalizedString name;

        #region Interface Implementation

        string INameOwner.Name => name.GetGeneralString();

        public LocalizedString NameReference => name;

        #endregion
    }
}