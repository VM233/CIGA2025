﻿using System.Collections.Generic;
using VMFramework.Core;
using VMFramework.Procedure;

namespace VMFramework.UI
{
    [ManagerCreationProvider(ManagerType.UICore)]
    public sealed class ContextMenuManager : ManagerBehaviour<ContextMenuManager>
    {
        private static ContextMenuGeneralSetting ContextMenuGeneralSetting => 
            UISetting.ContextMenuGeneralSetting;
        
        private static readonly List<IContextMenu> contextMenusCache = new();
        
        public static void Open(IContextMenuProvider contextMenuProvider, IUIPanel source)
        {
            if (contextMenuProvider == null)
            {
                Debugger.LogWarning($"{nameof(contextMenuProvider)} is Null");
                return;
            }

            if (contextMenuProvider.DisplayContextMenu() == false)
            {
                return;
            }

            string contextMenuID = null;

            contextMenuID ??= ContextMenuGeneralSetting.defaultContextMenuID;

            if (UIPanelManager.TryGetUniquePanelWithWarning(contextMenuID, out IContextMenu contextMenu) == false)
            {
                return;
            }

            contextMenu.Open(contextMenuProvider, source);
        }

        public static void Close(IContextMenuProvider contextMenuProvider)
        {
            if (contextMenuProvider == null)
            {
                Debugger.LogWarning($"{nameof(contextMenuProvider)} is Null");
                return;
            }
            
            contextMenusCache.Clear();
            UIPanelManager.GetUniquePanels(contextMenusCache);

            foreach (var contextMenu in contextMenusCache)
            {
                contextMenu.Close(contextMenuProvider);
            }
        }
    }
}