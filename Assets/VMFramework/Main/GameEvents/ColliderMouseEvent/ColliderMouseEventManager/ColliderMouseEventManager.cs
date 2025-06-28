using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using EnumsNET;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;

namespace VMFramework.GameEvents
{
    [ManagerCreationProvider(ManagerType.EventCore)]
    public sealed partial class ColliderMouseEventManager : ManagerBehaviour<ColliderMouseEventManager>
    {
        private const string DEBUGGING_CATEGORY = "Only For Debugging";

        private static ColliderMouseEventGeneralSetting Setting => CoreSetting.ColliderMouseEventGeneralSetting;

        [SerializeField]
        private Camera fixedBindCamera;

        [ShowInInspector]
        [HideInEditorMode]
        public Camera BindCamera { get; set; }

        [ShowInInspector]
        public float DetectDistance3D { get; set; }

        [ShowInInspector]
        public float DetectDistance2D { get; set; }

        [ShowInInspector]
        public ObjectDimensions DimensionsDetectPriority { get; set; }

        [ShowInInspector]
        public LayerMask LayerMask { get; set; }

        [ShowInInspector]
        public readonly List<PhysicsScene> physicsScene3Ds = new();

        [ShowInInspector]
        public readonly List<PhysicsScene2D> physicsScene2Ds = new();

        #region Triggers

        [FoldoutGroup(DEBUGGING_CATEGORY), ReadOnly, ShowInInspector]
        private ColliderMouseEventTrigger currentHoverTrigger;

        [FoldoutGroup(DEBUGGING_CATEGORY), ReadOnly, ShowInInspector]
        private ColliderMouseEventTrigger lastHoverTrigger;

        [FoldoutGroup(DEBUGGING_CATEGORY), ReadOnly, ShowInInspector]
        private ColliderMouseEventTrigger leftMouseUpDownTrigger;

        [FoldoutGroup(DEBUGGING_CATEGORY), ReadOnly, ShowInInspector]
        private ColliderMouseEventTrigger rightMouseUpDownTrigger;

        [FoldoutGroup(DEBUGGING_CATEGORY), ReadOnly, ShowInInspector]
        private ColliderMouseEventTrigger middleMouseUpDownTrigger;

        [FoldoutGroup(DEBUGGING_CATEGORY), ReadOnly, ShowInInspector]
        private ColliderMouseEventTrigger dragTrigger;
        
        private readonly HashSet<ColliderMouseEventTrigger> lastStayTriggers = new();
        
        private readonly List<ColliderMouseEventTrigger> currentStayTriggers = new();
        
        private readonly HashSet<ColliderMouseEventTrigger> triggersToLeave = new();

        #endregion

        #region Init

        protected override void Awake()
        {
            base.Awake();
            
            physicsScene2Ds.Clear();
            physicsScene3Ds.Clear();
            
            currentHoverTrigger = null;
            lastHoverTrigger = null;
            leftMouseUpDownTrigger = null;
            rightMouseUpDownTrigger = null;
            middleMouseUpDownTrigger = null;
            dragTrigger = null;
            
            lastStayTriggers.Clear();
            currentStayTriggers.Clear();
            triggersToLeave.Clear();
        }

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();

            if (fixedBindCamera != null)
            {
                BindCamera = fixedBindCamera;
            }
            else
            {
                BindCamera = Camera.main;
            }

            DetectDistance3D = Setting.detectDistance3D;

            DetectDistance2D = Setting.detectDistance2D;

            DimensionsDetectPriority = Setting.dimensionsDetectPriority;

            LayerMask = Setting.detectLayerMask;

            if (Setting.includingDefaultPhysicsScene3D)
            {
                physicsScene3Ds.Add(Physics.defaultPhysicsScene);
            }

            if (Setting.includingDefaultPhysicsScene2D)
            {
                physicsScene2Ds.Add(Physics2D.defaultPhysicsScene);
            }
        }

        #endregion

        private void Update()
        {
            if (BindCamera == null)
            {
                return;
            }
            
            currentStayTriggers.Clear();

            currentHoverTrigger = DetectTrigger(currentStayTriggers);

            var currentHoverTriggerIsNull = currentHoverTrigger == null;
            var lastHoverTriggerIsNull = lastHoverTrigger == null;

            #region Multiple Pointer Enter & Leave & Stay

            triggersToLeave.Clear();
            
            triggersToLeave.UnionWith(lastStayTriggers);
            triggersToLeave.ExceptWith(currentStayTriggers);

            foreach (var trigger in triggersToLeave)
            {
                Invoke(MouseEventType.PointerExitMultiple, trigger);
            }

            foreach (var trigger in currentStayTriggers)
            {
                if (lastStayTriggers.Contains(trigger) == false)
                {
                    Invoke(MouseEventType.PointerEnterMultiple, trigger);
                }
                else
                {
                    Invoke(MouseEventType.PointerStayMultiple, trigger);
                }
            }
            
            lastStayTriggers.Clear();
            lastStayTriggers.UnionWith(currentStayTriggers);

            #endregion

            #region Pointer Enter & Leave & Stay

            if (currentHoverTriggerIsNull)
            {
                // Pointer Leave
                if (lastHoverTriggerIsNull == false)
                {
                    Invoke(MouseEventType.PointerExit, lastHoverTrigger);
                }
            }
            else
            {
                // Pointer Leave & Enter
                if (currentHoverTrigger != lastHoverTrigger)
                {
                    if (lastHoverTriggerIsNull == false)
                    {
                        Invoke(MouseEventType.PointerExit, lastHoverTrigger);
                    }

                    Invoke(MouseEventType.PointerEnter, currentHoverTrigger);
                }

                // Pointer Hover
                Invoke(MouseEventType.PointerStay, currentHoverTrigger);
            }

            #endregion

            #region Left Mouse Button Up & Down

            if (leftMouseUpDownTrigger == null)
            {
                if (currentHoverTriggerIsNull == false)
                {
                    //Down
                    if (Input.GetMouseButtonDown(0))
                    {
                        leftMouseUpDownTrigger = currentHoverTrigger;

                        Invoke(MouseEventType.LeftMouseButtonDown, leftMouseUpDownTrigger);
                        Invoke(MouseEventType.LeftMouseButtonStay, leftMouseUpDownTrigger);
                    }
                }
            }
            else
            {
                if (currentHoverTrigger == leftMouseUpDownTrigger)
                {
                    //Up & Click
                    if (Input.GetMouseButtonUp(0))
                    {
                        Invoke(MouseEventType.LeftMouseButtonUp, leftMouseUpDownTrigger);
                        Invoke(MouseEventType.LeftMouseButtonClick, leftMouseUpDownTrigger);

                        leftMouseUpDownTrigger = null;
                    }
                    //Stay
                    else if (Input.GetMouseButton(0))
                    {
                        Invoke(MouseEventType.LeftMouseButtonStay, leftMouseUpDownTrigger);
                    }
                }
                else
                {
                    //Up
                    if (Input.GetMouseButtonUp(0))
                    {
                        Invoke(MouseEventType.LeftMouseButtonUp, leftMouseUpDownTrigger);

                        leftMouseUpDownTrigger = null;
                    }
                }
            }

            #endregion

            #region Right Mouse Button Up & Down

            if (rightMouseUpDownTrigger == null)
            {
                if (currentHoverTriggerIsNull == false)
                {
                    //Down
                    if (Input.GetMouseButtonDown(1))
                    {
                        rightMouseUpDownTrigger = currentHoverTrigger;

                        Invoke(MouseEventType.RightMouseButtonDown, rightMouseUpDownTrigger);
                        Invoke(MouseEventType.RightMouseButtonStay, rightMouseUpDownTrigger);
                    }
                }
            }
            else
            {
                if (currentHoverTrigger == rightMouseUpDownTrigger)
                {
                    //Up & Click
                    if (Input.GetMouseButtonUp(1))
                    {
                        Invoke(MouseEventType.RightMouseButtonUp, rightMouseUpDownTrigger);
                        Invoke(MouseEventType.RightMouseButtonClick, rightMouseUpDownTrigger);

                        rightMouseUpDownTrigger = null;
                    }
                    //Stay
                    else if (Input.GetMouseButton(1))
                    {
                        Invoke(MouseEventType.RightMouseButtonStay, rightMouseUpDownTrigger);
                    }
                }
                else
                {
                    //Up
                    if (Input.GetMouseButtonUp(1))
                    {
                        Invoke(MouseEventType.RightMouseButtonUp, rightMouseUpDownTrigger);

                        rightMouseUpDownTrigger = null;
                    }
                }
            }

            #endregion

            #region Middle Mouse Button Up & Down

            if (middleMouseUpDownTrigger == null)
            {
                if (currentHoverTriggerIsNull == false)
                {
                    //Down
                    if (Input.GetMouseButtonDown(2))
                    {
                        middleMouseUpDownTrigger = currentHoverTrigger;

                        Invoke(MouseEventType.MiddleMouseButtonDown, middleMouseUpDownTrigger);
                        Invoke(MouseEventType.MiddleMouseButtonStay, middleMouseUpDownTrigger);
                    }
                }
            }
            else
            {
                if (currentHoverTrigger == middleMouseUpDownTrigger)
                {
                    //Up & Click
                    if (Input.GetMouseButtonUp(2))
                    {
                        Invoke(MouseEventType.MiddleMouseButtonUp, middleMouseUpDownTrigger);
                        Invoke(MouseEventType.MiddleMouseButtonClick, middleMouseUpDownTrigger);

                        middleMouseUpDownTrigger = null;
                    }
                    //Stay
                    else if (Input.GetMouseButton(2))
                    {
                        Invoke(MouseEventType.MiddleMouseButtonStay, middleMouseUpDownTrigger);
                    }
                }
                else
                {
                    //Up
                    if (Input.GetMouseButtonUp(2))
                    {
                        Invoke(MouseEventType.MiddleMouseButtonUp, middleMouseUpDownTrigger);

                        middleMouseUpDownTrigger = null;
                    }
                }
            }

            #endregion

            #region Any Mouse Button Up & Down

            if (currentHoverTriggerIsNull == false)
            {
                //Down
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
                {
                    Invoke(MouseEventType.AnyMouseButtonDown, currentHoverTrigger);
                }

                //Up
                if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
                {
                    Invoke(MouseEventType.AnyMouseButtonUp, currentHoverTrigger);
                }

                //Stay
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButtonUp(2))
                {
                    Invoke(MouseEventType.AnyMouseButtonStay, currentHoverTrigger);
                }
            }

            #endregion

            #region Drag Begin & Stay & End

            if (dragTrigger == null)
            {
                // Drag Begin
                if (currentHoverTriggerIsNull == false && currentHoverTrigger.draggable)
                {
                    var triggerDrag = false;

                    foreach (var mouseType in currentHoverTrigger.dragButton.GetFlags())
                    {
                        if (mouseType == MouseButtonType.LeftButton && Input.GetMouseButton(0))
                        {
                            triggerDrag = true;
                            break;
                        }

                        if (mouseType == MouseButtonType.RightButton && Input.GetMouseButton(1))
                        {
                            triggerDrag = true;
                            break;
                        }

                        if (mouseType == MouseButtonType.MiddleButton && Input.GetMouseButton(2))
                        {
                            triggerDrag = true;
                            break;
                        }
                    }

                    if (triggerDrag)
                    {
                        dragTrigger = currentHoverTrigger;

                        Invoke(MouseEventType.DragBegin, dragTrigger);
                    }
                }
            }
            else
            {
                var keepDragging = false;

                foreach (var mouseType in dragTrigger.dragButton.GetFlags())
                {
                    if (mouseType == MouseButtonType.LeftButton && Input.GetMouseButton(0))
                    {
                        keepDragging = true;
                        break;
                    }

                    if (mouseType == MouseButtonType.RightButton && Input.GetMouseButton(1))
                    {
                        keepDragging = true;
                        break;
                    }

                    if (mouseType == MouseButtonType.MiddleButton && Input.GetMouseButton(2))
                    {
                        keepDragging = true;
                        break;
                    }
                }

                if (keepDragging)
                {
                    Invoke(MouseEventType.DragStay, dragTrigger);
                }
                else
                {
                    Invoke(MouseEventType.DragEnd, dragTrigger);

                    dragTrigger = null;
                }
            }

            #endregion

            lastHoverTrigger = currentHoverTrigger;
        }

        private ColliderMouseEventTrigger DetectTrigger(List<ColliderMouseEventTrigger> triggers)
        {
            if (DimensionsDetectPriority == ObjectDimensions.TWO_D)
            {
                ColliderMouseEventTrigger detected2D = Detect2DTrigger(triggers);
                if (detected2D != null)
                {
                    return detected2D;
                }

                return Detect3DTrigger();
            }

            ColliderMouseEventTrigger detected3D = Detect3DTrigger();
            if (detected3D != null)
            {
                return detected3D;
            }

            return Detect2DTrigger(triggers);
        }

        private ColliderMouseEventTrigger Detect3DTrigger()
        {
            Vector3 mousePos = Input.mousePosition;

            if (mousePos.IsInfinity() || mousePos.IsNaN())
            {
                return default;
            }

            var ray = BindCamera.ScreenPointToRay(mousePos);

            Debug.DrawRay(ray.origin, ray.direction, Color.green);

            foreach (var physicsScene in physicsScene3Ds)
            {
                if (physicsScene.Raycast(ray.origin, ray.direction, out var hit3D, DetectDistance3D, LayerMask))
                {
                    ColliderMouseEventTrigger detectResult =
                        hit3D.collider.gameObject.GetComponent<ColliderMouseEventTrigger>();

                    return detectResult;
                }
            }

            return default;
        }
        
        private readonly List<Collider2D> overlapResults = new();
        private readonly SortedList<int, ColliderMouseEventTrigger> triggerSorted = new();
        
        private ColliderMouseEventTrigger Detect2DTrigger(List<ColliderMouseEventTrigger> triggers)
        {
            Vector3 mousePos = Input.mousePosition;

            if (mousePos.IsInfinity())
            {
                return null;
            }

            triggerSorted.Clear();

            var contactFilter = new ContactFilter2D()
            {
                useTriggers = true,
                useLayerMask = true,
                layerMask = LayerMask
            };

            var point = BindCamera.ScreenToWorldPoint(mousePos);
            
            foreach (var physicsScene in physicsScene2Ds)
            {
                int count = physicsScene.OverlapPoint(point.XY(), contactFilter, overlapResults);

                for (int i = 0; i < count; i++)
                {
                    var collider = overlapResults[i];

                    if (collider.TryGetComponent(out ColliderMouseEventTrigger trigger) == false)
                    {
                        continue;
                    }

                    if (trigger.enabled == false)
                    {
                        continue;
                    }
                    
                    triggerSorted.TryAdd(-trigger.priority, trigger);
                    triggers.Add(trigger);
                }
            }

            if (triggerSorted.Count > 0)
            {
                return triggerSorted.Values[0];
            }
            
            return null;
        }
    }
}
