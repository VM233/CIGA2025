using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;
using VMFramework.UI;

namespace RoomPuzzle
{
    public class InventoryUI : PanelModifier
    {
        [VisualElementName]
        [IsNotNullOrEmpty]
        public string containerName;

        protected VisualElement Container { get; set; }

        protected Inventory inventory;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpenEvent += OnOpen;
            Panel.OnPostCloseEvent += OnClose;
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            Container = this.RootVisualElement().QueryStrictly(containerName, nameof(containerName));
            Container.Clear();

            if (StageManager.Instance.CurrentPlayer.TryGetComponent(out inventory))
            {
                inventory.OnInventoryChanged += OnInventoryChanged;
            }
        }

        protected virtual void OnClose(IUIPanel panel)
        {
            if (inventory != null)
            {
                inventory.OnInventoryChanged -= OnInventoryChanged;
                inventory = null;
            }
        }

        protected virtual void OnInventoryChanged(Inventory inventory)
        {
            Container.Clear();

            foreach (var item in inventory.Items)
            {
                var slot = new SlotVisualElement
                {
                    Icon = item.icon
                };
                if (item.Count > 1)
                {
                    slot.Description = item.Count.ToString();
                }
                else
                {
                    slot.Description = string.Empty;
                }

                Container.Add(slot);
            }
        }
    }
}