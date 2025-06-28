using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.GameEvents;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    [RequireComponent(typeof(IUIToolkitPanel))]
    public class UIToolkitEventsDisabledOnMouseEnterContainerModifier : PanelModifier
    {
        [BoxGroup(CONFIGS_CATEGORY)]
        [VisualElementName]
        [IsNotNullOrEmpty]
        [ListDrawerSettings(ShowFoldout = false)]
        public List<string> containerNames = new();

        [BoxGroup(CONFIGS_CATEGORY)]
        [ListDrawerSettings(ShowFoldout = false)]
        [GamePrefabID(typeof(IGameEventConfig))]
        public List<string> gameEventsDisabledOnMouseEnter = new();

        protected EventCallback<MouseEnterEvent> onMouseEnterFunc;
        protected EventCallback<MouseLeaveEvent> onMouseLeaveFunc;

        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        protected readonly List<VisualElement> containers = new();

        protected readonly Dictionary<VisualElement, IToken> tokens = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            onMouseEnterFunc = OnMouseEnterElement;
            onMouseLeaveFunc = OnMouseLeaveElement;

            Panel.OnOpenEvent += OnOpen;
            Panel.OnPostCloseEvent += OnPostClose;
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            if (gameEventsDisabledOnMouseEnter.IsNullOrEmpty())
            {
                return;
            }

            containers.Clear();
            containers.AddRange(this.RootVisualElement().QueryStrictly(containerNames, nameof(containerNames)));

            tokens.Clear();
            foreach (var container in containers)
            {
                container.RegisterCallback(onMouseEnterFunc);
                container.RegisterCallback(onMouseLeaveFunc);

                var token = new Token
                {
                    Source = this
                };
                tokens[container] = token;
            }
        }

        protected virtual void OnPostClose(IUIPanel panel)
        {
            OnMouseLeaveElement(null);

            foreach (var container in containers)
            {
                container.UnregisterCallback(onMouseEnterFunc);
                container.UnregisterCallback(onMouseLeaveFunc);
            }

            containers.Clear();

            foreach (var token in tokens.Values)
            {
                GameEventManager.Instance.Enable(gameEventsDisabledOnMouseEnter, token);
            }
        }

        protected virtual void OnMouseEnterElement(MouseEnterEvent evt)
        {
            var target = (VisualElement)evt?.target;
            if (target == null)
            {
                return;
            }

            if (tokens.TryGetValue(target, out var token) == false)
            {
                Debugger.LogError($"Could not find token for {target.name}");
                return;
            }

            GameEventManager.Instance.Disable(gameEventsDisabledOnMouseEnter, token);
        }

        protected virtual void OnMouseLeaveElement(MouseLeaveEvent evt)
        {
            var target = (VisualElement)evt?.target;
            if (target == null)
            {
                return;
            }

            if (tokens.TryGetValue(target, out var token) == false)
            {
                Debugger.LogError($"Could not find token for {target.name}");
                return;
            }

            GameEventManager.Instance.Enable(gameEventsDisabledOnMouseEnter, token);
        }
    }
}