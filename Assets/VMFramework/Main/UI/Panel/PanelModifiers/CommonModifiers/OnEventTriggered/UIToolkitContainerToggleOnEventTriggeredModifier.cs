using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.GameEvents;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    [RequireComponent(typeof(IUIToolkitPanel))]
    public class UIToolkitContainerToggleOnEventTriggeredModifier : PanelModifier
    {
        [BoxGroup(CONFIGS_CATEGORY)]
        [VisualElementName]
        [IsNotNullOrEmpty]
        [DisallowDuplicateElements]
        public List<string> containersName = new();
        
        [BoxGroup(CONFIGS_CATEGORY)]
        [ListDrawerSettings(ShowFoldout = false)]
        [GamePrefabID(typeof(IGameEventConfig))]
        [DisallowDuplicateElements]
        public List<string> containerToggleGameEventIDs = new();

        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        protected readonly List<VisualElement> containers = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpenEvent += OnOpen;
            Panel.OnPostCloseEvent += OnClose;
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            containers.Clear();
            foreach (var containerName in containersName)
            {
                var container = this.RootVisualElement().QueryStrictly(containerName, nameof(containerName));
                containers.Add(container);
            }

            foreach (var gameEventID in containerToggleGameEventIDs)
            {
                GameEventManager.Instance.AddCallback(gameEventID, OnContainerToggle, PriorityDefines.TINY);
            }
        }

        protected virtual void OnClose(IUIPanel panel)
        {
            foreach (var gameEventID in containerToggleGameEventIDs)
            {
                GameEventManager.Instance.RemoveCallback(gameEventID, OnContainerToggle);
            }
        }
        
        protected virtual void OnContainerToggle()
        {
            foreach (var container in containers)
            {
                container.ToggleDisplay();
            }
        }
    }
}