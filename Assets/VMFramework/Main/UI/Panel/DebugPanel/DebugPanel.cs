using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;
using VMFramework.Timers;

namespace VMFramework.UI
{
    public class DebugPanel : PanelModifier, ITimer<double>
    {
        public const string LEFT_GROUP_NAME = "LeftGroup";

        public const string RIGHT_GROUP_NAME = "RightGroup";
        
        [LabelText("Left Container Name"), BoxGroup(CONFIGS_CATEGORY)]
        [VisualElementName]
        [IsNotNullOrEmpty]
        public string leftContainerVisualElementName = LEFT_GROUP_NAME;

        [LabelText("Right Container Name"), BoxGroup(CONFIGS_CATEGORY)]
        [VisualElementName]
        [IsNotNullOrEmpty]
        public string rightContainerVisualElementName = RIGHT_GROUP_NAME;
        
        private struct DebugEntryInfo
        {
            public IconLabelVisualElement iconLabel;

            [ShowInInspector]
            private bool Display => iconLabel.style.display.value == DisplayStyle.Flex;
        }
        
        private static float UpdateInterval => UISetting.DebugPanelGeneralSetting.updateInterval;

        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        private VisualElement leftContainer;

        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        private VisualElement rightContainer;
        
        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        private List<(IDebugEntry debugEntry, DebugEntryInfo info)> debugEntryInfos = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            Panel.OnOpenEvent += OnOpen;
            Panel.OnPostCloseEvent += OnPostClose;
        }

        private void OnOpen(IUIPanel panel)
        {
            leftContainer = this.RootVisualElement().Q(leftContainerVisualElementName);
            rightContainer = this.RootVisualElement().Q(rightContainerVisualElementName);

            leftContainer.AssertIsNotNull(nameof(leftContainer));
            rightContainer.AssertIsNotNull(nameof(rightContainer));

            debugEntryInfos.Clear();

            foreach (var debugEntry in GamePrefabManager.GetAllActiveGamePrefabs<IDebugEntry>())
            {
                AddEntry(debugEntry);
            }
            
            TimerManager.Instance.Add(this, UpdateInterval);
        }
        
        private void OnPostClose(IUIPanel panel)
        {
            TimerManager.Instance.TryStop(this);
        }

        void ITimer<double>.OnTimed()
        {
            foreach (var (debugEntry, info) in debugEntryInfos)
            {
                if (debugEntry.ShouldDisplay())
                {
                    info.iconLabel.style.display = DisplayStyle.Flex;
                    info.iconLabel.Label.text = debugEntry.GetText();
                }
                else
                {
                    info.iconLabel.style.display = DisplayStyle.None;
                }
            }
            
            TimerManager.Instance.Add(this, UpdateInterval);
        }

        public void AddEntry(IDebugEntry debugEntry)
        {
            var container = debugEntry.Position switch
            {
                LeftRightDirection.Left => leftContainer,
                LeftRightDirection.Right => rightContainer,
                _ => throw new ArgumentOutOfRangeException()
            };

            var debugEntryVisualElement = new IconLabelVisualElement
            {
                pickingMode = PickingMode.Ignore,
                Label =
                {
                    text = ""
                }
            };

            container.Add(debugEntryVisualElement);

            debugEntryInfos.Add((debugEntry, new DebugEntryInfo
            {
                iconLabel = debugEntryVisualElement
            }));
        }
        
        #region Priority Queue Node

        double IGenericPriorityQueueNode<double>.Priority { get; set; }

        int IGenericPriorityQueueNode<double>.QueueIndex { get; set; }

        long IGenericPriorityQueueNode<double>.InsertionIndex { get; set; }

        #endregion
    }
}