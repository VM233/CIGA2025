using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Cameras;
using VMFramework.Core;
using VMFramework.Procedure;

namespace VMFramework.UI
{
    [ManagerCreationProvider(ManagerType.UICore)]
    public partial class TracingUIManager : ManagerBehaviour<TracingUIManager>
    {
        [ShowInInspector]
        private readonly Dictionary<ITracingPanelModifier, TracingInfo> allTracingInfos = new();

        private readonly List<ITracingPanelModifier> tracingUIPanelsToRemove = new();

        [ShowInInspector]
        private Camera mainCamera;
        
        #region Init

        protected override void Awake()
        {
            base.Awake();
            
            allTracingInfos.Clear();
        }

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();
            
            mainCamera = CameraManager.MainCamera;
        }

        #endregion

        #region Update

        protected virtual void Update()
        {
            var mousePosition = Input.mousePosition.To2D();

            foreach (var (panel, info) in allTracingInfos)
            {
                Vector2 screenPos = info.Config.tracingType switch
                {
                    TracingType.MousePosition => mousePosition,
                    TracingType.Transform => mainCamera.WorldToScreenPoint(info.Config.tracingTransform.position),
                    TracingType.WorldPosition => mainCamera.WorldToScreenPoint(info.Config.tracingWorldPosition),
                    _ => throw new ArgumentOutOfRangeException()
                };
                
                if (panel.TryUpdatePosition(screenPos) && info.Config.hasMaxTracingCount)
                {
                    info.tracingCount++;
                }
            }

            foreach (var (panel, info) in allTracingInfos)
            {
                if (info.Config.hasMaxTracingCount && info.tracingCount > info.Config.maxTracingCount)
                {
                    tracingUIPanelsToRemove.Add(panel);
                }
            }

            if (tracingUIPanelsToRemove.Count > 0)
            {
                foreach (var tracingUIPanel in tracingUIPanelsToRemove)
                {
                    StopTracing(tracingUIPanel);
                }

                tracingUIPanelsToRemove.Clear();
            }
        }

        #endregion

        #region Set Camera

        [Button]
        public void SetCamera(Camera camera)
        {
            mainCamera = camera;
        }

        #endregion
    }
}