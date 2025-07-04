﻿using Sirenix.OdinInspector;
using VMFramework.Configuration;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;

namespace VMFramework.UI
{
    public sealed partial class UIPanelProcedureGeneralSetting : GeneralSetting
    {
        private const string PROCEDURE_CATEGORY = "Procedures";
        
        [TabGroup(TAB_GROUP_NAME, PROCEDURE_CATEGORY)]
        public DictionaryConfigs<string, UIPanelProcedureConfig> procedureConfigs = new();

        public override void CheckSettings()
        {
            base.CheckSettings();
            
            procedureConfigs.CheckSettings();
        }

        protected override void OnInit()
        {
            base.OnInit();
            
            procedureConfigs.Init();
            ProcedureManager.Instance.OnEnterProcedureEvent += OnEnterProcedure;
            ProcedureManager.Instance.OnExitProcedureEvent += OnExitProcedure;
        }

        private void OnEnterProcedure(string procedureID)
        {
            if (procedureConfigs.TryGetConfig(procedureID, out var config) == false)
            {
                return;
            }

            if (config.uiPanelAutoCloseOnEnter != null)
            {
                foreach (var uiPanelID in config.uiPanelAutoCloseOnEnter)
                {
                    if (UIPanelManager.TryGetOpenedPanels(uiPanelID, out var uiPanels))
                    {
                        var openedUIPanels = ListPool<IUIPanel>.Default.Get();
                        openedUIPanels.Clear();
                        openedUIPanels.AddRange(uiPanels);
                        
                        foreach (var uiPanelController in openedUIPanels)
                        {
                            uiPanelController.Close();
                        }
                        
                        openedUIPanels.ReturnToDefaultPool();
                    }
                }
            }

            if (config.uniqueUIPanelAutoOpenOnEnter != null)
            {
                foreach (var uiPanelID in config.uniqueUIPanelAutoOpenOnEnter)
                {
                    if (UIPanelManager.TryGetUniquePanelWithWarning(uiPanelID, out var panel))
                    {
                        panel.Open(null);
                    }
                }
            }
        }

        private void OnExitProcedure(string procedureID)
        {
            if (procedureConfigs.TryGetConfig(procedureID, out var config) == false)
            {
                return;
            }

            if (config.uiPanelAutoCloseOnExit != null)
            {
                foreach (var uiPanelID in config.uiPanelAutoCloseOnExit)
                {
                    if (UIPanelManager.TryGetOpenedPanels(uiPanelID, out var uiPanels))
                    {
                        var openedUIPanels = ListPool<IUIPanel>.Default.Get();
                        openedUIPanels.Clear();
                        openedUIPanels.AddRange(uiPanels);
                        
                        foreach (var uiPanelController in openedUIPanels)
                        {
                            uiPanelController.Close();
                        }
                        
                        openedUIPanels.ReturnToDefaultPool();
                    }
                }
            }

            if (config.uniqueUIPanelAutoOpenOnExit != null)
            {
                foreach (var uiPanelID in config.uniqueUIPanelAutoOpenOnExit)
                {
                    if (UIPanelManager.TryGetUniquePanelWithWarning(uiPanelID, out var panel))
                    {
                        panel.Open(null);
                    }
                }
            }
        }
    }
}