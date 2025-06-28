using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class UIToolkitContainerToggleButtonModifier : UIToolkitButtonModifier
    {
        public enum InitialOperation
        {
            None,
            Show,
            Hide
        }
        
        [BoxGroup(CONFIGS_CATEGORY)]
        [VisualElementName]
        [IsNotNullOrEmpty]
        public List<string> containerNames = new();
        
        [BoxGroup(CONFIGS_CATEGORY)]
        public InitialOperation initialOperation = InitialOperation.None;

        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        protected readonly List<VisualElement> containers = new();

        protected override void OnOpen(IUIPanel panel)
        {
            base.OnOpen(panel);
            
            containers.AddRange(this.RootVisualElement().QueryStrictly(containerNames, nameof(containerNames)));

            if (initialOperation == InitialOperation.Show)
            {
                containers.DisplayFlex();
            }
            else if (initialOperation == InitialOperation.Hide)
            {
                containers.DisplayNone();
            }
        }

        protected override void OnClicked()
        {
            foreach (var container in containers)
            {
                container.ToggleDisplay();
            }
        }
    }
}