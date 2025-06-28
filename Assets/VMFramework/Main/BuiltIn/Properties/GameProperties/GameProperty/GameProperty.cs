using System;
using VMFramework.GameLogicArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.UI;

namespace VMFramework.Properties
{
    public abstract partial class GameProperty : LocalizedGamePrefab, IGameProperty
    {
        public override string IDSuffix => "property";

        public sealed override Type GameItemType => null;

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [PreviewField(50, ObjectFieldAlignment.Center)]
        [Required]
        public Sprite icon;

        Sprite IIconOwner.Icon => icon;

        public abstract bool CanGetValueString(object target);

        public abstract void GetValueAndNameString(object target, out string nameString, out string valueString,
            out object source);

        public virtual void GetFullString(object target, out string str, out object source)
        {
            GetValueAndNameString(target, out var nameString, out string valueString, out source);
            str = $"{nameString}:{valueString}";
        }
    }
}