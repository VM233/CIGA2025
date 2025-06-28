using System;
using Sirenix.OdinInspector;
using VMFramework.Configuration;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    [Serializable]
    public class ContainerSlotDistributorConfig
    {
        [VisualElementName]
        public string parentName;

        public SlotFindMethod findMethod = SlotFindMethod.Default;
        
        [ShowIf(nameof(findMethod), SlotFindMethod.ByName)]
        [VisualElementName]
        [IsNotNullOrEmpty]
        public string slotName;

        public bool isFinite = true;
        
        [HideIf(nameof(isFinite))]
        public int startSlotIndex;
        
        [ShowIf(nameof(isFinite))]
        public RangeIntegerConfig slotIndexRange = new(0, 0);

        [ShowIf(nameof(isFinite))]
        public bool autoFill;

        [EnableIf(nameof(autoFill))]
        public bool hasCustomContainer;
        
        [ShowIf(nameof(hasCustomContainer))]
        [EnableIf(nameof(autoFill))]
        [VisualElementName]
        public string customContainerName;
        
        public string ContainerName => hasCustomContainer? customContainerName : parentName;

        public int StartIndex => isFinite ? slotIndexRange.min : startSlotIndex;
        
        public int Count => isFinite ? slotIndexRange.Count : int.MaxValue;
    }
}