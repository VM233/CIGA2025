using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.OdinExtensions;
using VMFramework.Timers;

namespace VMFramework.UI
{
    public class UIToolkitPropertyDisplayModifier : PanelModifier, IPropertyDisplayModifier, ITimer<double>
    {
        [BoxGroup(CONFIGS_CATEGORY)]
        [VisualElementName]
        [IsNotNullOrEmpty]
        public string propertyContainerName;
        
        [BoxGroup(CONFIGS_CATEGORY)]
        [MinValue(0.1)]
        public float iconSwitchInterval = 0.75f;

        public IEnumerable<object> PropertySources => propertyElementsLookup.Keys;
        
        public IReadOnlyList<(PropertyDisplayInfo info, VisualElement element)> AllPropertyElements => allPropertyElements;

        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        public VisualElement PropertyContainer { get; protected set; }

        protected IUIToolkitPanelConfig UIToolkitPanelConfig => (IUIToolkitPanelConfig)UIPanelConfig;
        
        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        protected readonly Dictionary<IReadOnlyList<Sprite>, int> iconIndexLookup = new();

        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        protected readonly List<(PropertyDisplayInfo info, VisualElement element)> allPropertyElements = new();
        
        protected readonly Dictionary<object, List<VisualElement>> propertyElementsLookup = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            Panel.OnOpenEvent += OnOpen;
            Panel.OnPostCloseEvent += OnPostClose;
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            PropertyContainer = this.RootVisualElement()
                .QueryStrictly(propertyContainerName, nameof(propertyContainerName));
            Clear();
            
            TimerManager.Instance.Add(this, iconSwitchInterval);
        }
        
        protected virtual void OnPostClose(IUIPanel panel)
        {
            iconIndexLookup.Clear();
            TimerManager.Instance.TryStop(this);
        }

        public bool TryGetElementsByPropertySource(object propertySource, out IReadOnlyList<VisualElement> elements)
        {
            if (propertyElementsLookup.TryGetValue(propertySource, out var visualElements))
            {
                elements = visualElements;
                return true;
            }
            
            elements = null;
            return false;
        }

        public void AddProperty(PropertyDisplayInfo propertyConfig)
        {
            var iconLabel = new IconLabelVisualElement();

            if (UIToolkitPanelConfig.IgnoreMouseEvents)
            {
                iconLabel.pickingMode = PickingMode.Ignore;
                iconLabel.Icon.pickingMode = PickingMode.Ignore;
                iconLabel.Label.pickingMode = PickingMode.Ignore;
            }

            if (propertyConfig.icon.isSingle == false)
            {
                var icons = propertyConfig.icon.list;

                if (icons.IsNullOrEmpty() == false)
                {
                    if (iconIndexLookup.TryGetValue(icons, out var index) == false)
                    {
                        index = 0;
                        iconIndexLookup.Add(icons, index);
                    }
                    else
                    {
                        index %= icons.Count;
                    }
                    
                    iconLabel.SetIcon(icons[index]);
                }
            }
            else
            {
                iconLabel.SetIcon(propertyConfig.icon.value);
            }

            iconLabel.SetContent(propertyConfig.attributeValue);

            PropertyContainer.Add(iconLabel);

            var elements = propertyElementsLookup.GetValueOrAddNew(propertyConfig.source);
            elements.Add(iconLabel);
            
            allPropertyElements.Add((propertyConfig, iconLabel));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            PropertyContainer.Clear();
            propertyElementsLookup.Clear();
            allPropertyElements.Clear();
        }
        
        protected readonly List<IReadOnlyList<Sprite>> iconsToSwitch = new();

        protected virtual void OnTimed()
        {
            iconsToSwitch.Clear();
            iconsToSwitch.AddRange(iconIndexLookup.Keys);

            foreach (var icons in iconsToSwitch)
            {
                iconIndexLookup[icons] = (iconIndexLookup[icons] + 1) % icons.Count;
            }
        }

        void ITimer<double>.OnTimed()
        {
            OnTimed();
        }

        #region Proprity Queue Node

        double IGenericPriorityQueueNode<double>.Priority { get; set; }

        int IGenericPriorityQueueNode<double>.QueueIndex { get; set; }

        long IGenericPriorityQueueNode<double>.InsertionIndex { get; set; }

        #endregion
    }
}