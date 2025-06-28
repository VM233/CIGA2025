using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.GameEvents;
using VMFramework.OdinExtensions;
using VMFramework.Procedure;

namespace VMFramework.UI
{
    public sealed class UIToggleOnEventTriggeredModifier : PanelModifier
    {
        [BoxGroup(CONFIGS_CATEGORY)]
        [ListDrawerSettings(ShowFoldout = false)]
        [GamePrefabID(typeof(IGameEventConfig))]
        [DisallowDuplicateElements]
        public List<string> uiToggleGameEventIDs = new();

        [BoxGroup(CONFIGS_CATEGORY)]
        [ListDrawerSettings(ShowFoldout = false)]
        [ProcedureID]
        [DisallowDuplicateElements]
        public List<string> activeProceduresID = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            foreach (var gameEventID in uiToggleGameEventIDs)
            {
                GameEventManager.Instance.AddCallback(gameEventID, ToggleConditional, PriorityDefines.TINY);
            }
        }

        protected override void OnDeinitialize()
        {
            base.OnDeinitialize();
            
            foreach (var gameEventID in uiToggleGameEventIDs)
            {
                GameEventManager.Instance.RemoveCallback(gameEventID, ToggleConditional);
            }
        }

        private void ToggleConditional()
        {
            if (activeProceduresID.Count > 0)
            {
                if (activeProceduresID.All(procedureID =>
                        ProcedureManager.Instance.CurrentProcedureIDs.Contains(procedureID) == false))
                {
                    return;
                }

                Panel.Toggle();
                return;
            }

            Panel.Toggle();
        }
    }
}