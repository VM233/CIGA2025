﻿using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.GameEvents;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class EventsDisabledOnOpenModifier : PanelModifier
    {
        [BoxGroup(CONFIGS_CATEGORY)]
        [GamePrefabID(typeof(IGameEventConfig))]
        [ListDrawerSettings(ShowFoldout = false)]
        [DisallowDuplicateElements]
        public List<string> gameEventDisabledOnOpen = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpenEvent += OnOpen;
            Panel.OnPostCloseEvent += OnClose;
        }

        protected override void OnDeinitialize()
        {
            base.OnDeinitialize();

            GameEventManager.Instance.Enable(gameEventDisabledOnOpen, Panel);
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            GameEventManager.Instance.Disable(gameEventDisabledOnOpen, panel);
        }

        protected virtual void OnClose(IUIPanel panel)
        {
            GameEventManager.Instance.Enable(gameEventDisabledOnOpen, panel);
        }
    }
}