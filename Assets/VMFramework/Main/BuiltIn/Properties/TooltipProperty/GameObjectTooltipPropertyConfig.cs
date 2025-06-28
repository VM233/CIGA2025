using Sirenix.OdinInspector;
using VMFramework.Configuration;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.Properties
{
    public class GameObjectTooltipPropertyConfig : BaseConfig
    {
        [GamePrefabID(typeof(IGameProperty))]
        public string propertyID;
        
        public string groupName;

        [HideInEditorMode]
        public IGameProperty property;

        protected override void OnInit()
        {
            base.OnInit();

            property = GamePrefabManager.GetGamePrefabStrictly<IGameProperty>(propertyID);
        }
    }
}