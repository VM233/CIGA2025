using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;

namespace VMFramework.UI
{
    [RequireComponent(typeof(UIDocument))]
    [DisallowMultipleComponent]
    public partial class UIToolkitPanel : UIPanel, IUIToolkitPanel
    {
        public UIDocument UIDocument { get; private set; }

        protected IUIToolkitPanelConfig UIToolkitPanelConfig => (IUIToolkitPanelConfig)GamePrefab;

        public VisualElement RootVisualElement { get; private set; }

        protected CancellationTokenSource OpenCTS { get; private set; }

        public event Action<IUIToolkitPanel> OnLayoutChangeEvent;

        #region Pool Events

        protected override void OnCreate()
        {
            base.OnCreate();
            
            var uiDocument = GetComponent<UIDocument>();

            uiDocument.panelSettings = UIToolkitPanelConfig.PanelSettings;
            uiDocument.visualTreeAsset = UIToolkitPanelConfig.VisualTree;

            uiDocument.enabled = false;
            
            UIDocument = uiDocument;
        }

        #endregion

        #region Open

        protected override async void OnOpen(IUIPanel source)
        {
            base.OnOpen(source);

            if (UIDocument.enabled == false)
            {
                UIDocument.enabled = true;
            }
            
            RootVisualElement = UIDocument.rootVisualElement;
            
            RootVisualElement.DisplayFlex();
            
            var defaultTheme = UISetting.UIPanelGeneralSetting.defaultTheme;
            if (defaultTheme != null)
            {
                RootVisualElement.styleSheets.Add(defaultTheme);
            }
            
            RootVisualElement.style.visibility = Visibility.Hidden;

            OpenCTS = new();

            await UniTask.Yield(OpenCTS.Token);
            
            OnLayoutChange();

            OnLayoutChangeEvent?.Invoke(this);
            
            OnPostLayoutChange();
        }

        #endregion

        protected override void OnPreClose()
        {
            base.OnPreClose();
            
            OpenCTS?.Cancel();
        }

        protected override void OnPostClose()
        {
            base.OnPostClose();

            if (UIToolkitPanelConfig.CloseMode == UIToolkitPanelCloseMode.DisableDocument)
            {
                UIDocument.enabled = false;
            }
            else
            {
                RootVisualElement.DisplayNone();
            }
        }

        #region Layout Change
        
        protected virtual void OnLayoutChange()
        {

        }

        protected virtual void OnPostLayoutChange()
        {
            RootVisualElement.style.visibility = Visibility.Visible;

            if (UIToolkitPanelConfig.IgnoreMouseEvents)
            {
                foreach (var visualElement in RootVisualElement.QueryAll<VisualElement>())
                {
                    visualElement.pickingMode = PickingMode.Ignore;
                }
            }
        }

        #endregion
    }
}