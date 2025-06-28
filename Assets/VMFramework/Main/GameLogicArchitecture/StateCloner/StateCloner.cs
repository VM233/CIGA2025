using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.GameLogicArchitecture
{
    public class StateCloner : MonoBehaviour, IStateCloner
    {
        [ListDrawerSettings(ShowFoldout = false)]
        [ShowInInspector]
        protected IStateCloneable[] stateUpdatables;

        protected virtual void Awake()
        {
            stateUpdatables = GetComponentsInChildren<IStateCloneable>();
        }

        public virtual void CloneFrom(IStateCloner cloner)
        {
            var stateCloner = (StateCloner)cloner;
            for (int i = 0; i < stateUpdatables.Length; i++)
            {
                stateUpdatables[i].CloneFrom(stateCloner.stateUpdatables[i]);
            }
        }
    }
}