using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;

namespace VMFramework.GameLogicArchitecture
{
    public partial class ControllerGameItem : MonoBehaviour, IControllerGameItem
    {
        #region Properties & Fields

        [ShowInInspector, HideInEditorMode]
        protected IGamePrefab GamePrefab { get; private set; }
        
        public string id => GamePrefab.id;

        public string Name => GamePrefab.Name;

        public bool IsDebugging => GamePrefab.IsDebugging;
        
        public ICollection<string> GameTags => GamePrefab.GameTags;

        public IStateCloner StateCloner { get; protected set; }

        [ShowInInspector, ReadOnly]
        public bool IsDestroyed { get; private set; } = false;

        public event Action<IReadOnlyDestructible> OnDestructed;

        #endregion

        #region Pool Events

        public event IPoolEventProvider.GetHandler OnGetEvent;
        public event IPoolEventProvider.ReturnHandler OnReturnEvent;

        void IPoolItem.OnGet()
        {
            IsDestroyed = false;
            OnGet();
            OnGetEvent?.Invoke(this);
        }

        void ICreatablePoolItem<string>.OnCreate(string id)
        {
            GamePrefab = GamePrefabManager.GetGamePrefabStrictly<IGamePrefab>(id);
            
            IsDestroyed = false;
            OnCreate();
            OnGet();
            OnGetEvent?.Invoke(this);
        }

        void IPoolItem.OnReturn()
        {
            OnReturnEvent?.Invoke(this);
            OnReturn();
            IsDestroyed = true;
        }

        void IPoolItem.OnClear()
        {
            OnReturnEvent?.Invoke(this);
            OnReturn();
            IsDestroyed = true;
            OnClear();
        }

        protected virtual void OnGet()
        {
            gameObject.SetActive(true);
        }

        protected virtual void OnCreate()
        {
            name = id;
            StateCloner = GetComponent<IStateCloner>();
        }

        protected virtual void OnReturn()
        {
            OnDestructed?.Invoke(this);
            gameObject.SetActive(false);
        }

        protected virtual void OnClear()
        {

        }

        #endregion

        #region To String

        protected virtual void OnGetStringProperties(
            ICollection<(string propertyID, string propertyContent)> collection)
        {

        }

        public override string ToString()
        {
            var list = ListPool<(string propertyID, string propertyContent)>.Shared.Get();
            list.Clear();
            OnGetStringProperties(list);

            var extraString = list.Select(property => property.propertyID + ":" + property.propertyContent).Join(", ");
            list.ReturnToSharedPool();

            return $"[{GetType()}:id:{id},{extraString}]";
        }

        #endregion
    }
}