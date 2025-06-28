using UnityEngine.Localization.Settings;
using VMFramework.Procedure;

namespace VMFramework.UI
{
    [ManagerCreationProvider(ManagerType.UICore)]
    public sealed class LocalizedUIPanelManager : ManagerBehaviour<LocalizedUIPanelManager>
    {
        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();

            UIPanelManager.OnPanelCreatedEvent += OnUIPanelCreated;
        }

        private void OnUIPanelCreated(IUIPanel uiPanelController)
        {
            if (uiPanelController.TryGetComponent(out ILocalizedPanelModifier _))
            {
                uiPanelController.OnOpenEvent += OnUIPanelOpen;
                uiPanelController.OnPostCloseEvent += OnUIPanelClose;
                uiPanelController.OnDestructEvent += OnUIPanelDestruct;
            }
        }

        private void OnUIPanelOpen(IUIPanel uiPanelController)
        {
            foreach (var modifier in uiPanelController.GetComponents<ILocalizedPanelModifier>())
            {
                modifier.OnCurrentLanguageChanged(LocalizationSettings.SelectedLocale);

                LocalizationSettings.SelectedLocaleChanged += modifier.OnCurrentLanguageChanged;
            }
        }

        private void OnUIPanelClose(IUIPanel uiPanelController)
        {
            foreach (var modifier in uiPanelController.GetComponents<ILocalizedPanelModifier>())
            {
                LocalizationSettings.SelectedLocaleChanged -= modifier.OnCurrentLanguageChanged;
            }
        }

        private void OnUIPanelDestruct(IUIPanel uiPanelController)
        {
            foreach (var modifier in uiPanelController.GetComponents<ILocalizedPanelModifier>())
            {
                LocalizationSettings.SelectedLocaleChanged -= modifier.OnCurrentLanguageChanged;
            }
        }
    }
}