﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.FSM;

namespace VMFramework.Procedure
{
    [ManagerCreationProvider(ManagerType.ProcedureCore)]
    public sealed partial class ProcedureManager : ManagerBehaviour<IProcedureManager>, IProcedureManager
    {
        [ShowInInspector]
        private IMultiFSM<string, ProcedureManager> fsm;

        [ShowInInspector]
        private List<IManagerBehaviour> managerBehaviours = new();

        private readonly Dictionary<string, IProcedure> procedures = new();
        
        [ShowInInspector]
        public IReadOnlyList<IProcedure> Procedures => procedures.Values.ToList();

        [ShowInInspector]
        [ListDrawerSettings(ShowFoldout = false)]
        public IReadOnlyList<string> CurrentProcedureIDs => fsm?.CurrentStates.Keys.ToList();

        [ShowInInspector]
        private readonly Queue<(string fromProcedureID, string toProcedureID)> procedureSwitchQueue =
            new();

        public event Action<string> OnEnterProcedureEvent;
        
        public event Action<string> OnExitProcedureEvent;

        #region Awake & Start & Update

        private void Start()
        {
            procedures.Clear();
            fsm = new MultiFSM<string, ProcedureManager>();
            
            string startProcedureID = null;
            int startProcedurePriority = 0;
            
            foreach (var procedureType in typeof(IProcedure).GetDerivedInstantiableClasses(false))
            {
                var procedure = (IProcedure)procedureType.CreateInstance();

                procedures.Add(procedure.id, procedure);

                fsm.AddState(procedure);

                if (procedureType.TryGetAttribute<StartProcedureAttribute>(false,
                        out var startProcedureAttribute))
                {
                    if (startProcedureID == null)
                    {
                        startProcedureID = procedure.id;
                        startProcedurePriority = startProcedureAttribute.Priority;
                    }
                    else if (startProcedureAttribute.Priority > startProcedurePriority)
                    {
                        startProcedureID = procedure.id;
                        startProcedurePriority = startProcedureAttribute.Priority;
                    }
                }
            }

            if (startProcedureID == null)
            {
                throw new InvalidOperationException("No start procedure found.");
            }

            fsm.Init(this);

            OnEnterProcedureEvent += procedureID => Debugger.Log($"Enter Procedure:<color=orange>{procedureID}</color>");

            OnExitProcedureEvent += procedureID => Debugger.Log($"Exit Procedure:<color=orange>{procedureID}</color>");

            ProcedureAutoSwitchBinder.Init(procedures.Values);

            OnEnterProcedureEvent += procedureID =>
            {
                if (ProcedureAutoSwitchBinder.TryGetNextProcedureID(procedureID, out var nextProcedureID))
                {
                    EnterProcedure(procedureID, nextProcedureID);
                }
            };

            CollectGameInitializers();

            EnterProcedure(startProcedureID);
        }

        private void Update()
        {
            if (procedureSwitchQueue.Count == 0)
            {
                return;
            }

            var (fromProcedureID, toProcedureID) = procedureSwitchQueue.Dequeue();

            if (fromProcedureID.IsNullOrEmpty() == false)
            {
                if (HasCurrentProcedure(fromProcedureID) == false)
                {
                    Debug.LogError($"Failed to switch procedure from {fromProcedureID} to {toProcedureID}. " +
                                   $"{fromProcedureID} is not current state.");
                    EnterProcedureImmediately(toProcedureID);
                }
                else
                {
                    ExitProcedureImmediately(fromProcedureID, () => EnterProcedureImmediately(toProcedureID));
                }
            }
            else
            {
                EnterProcedureImmediately(toProcedureID);
            }
        }

        #endregion

        #region Enter & Exit Procedure

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnterProcedureImmediately(string procedureID)
        {
            if (IsLoading)
            {
                Debugger.LogWarning("ProcedureManager is still loading, cannot switch procedure.");
                return;
            }
            
            if (fsm.HasCurrentState(procedureID))
            {
                Debugger.LogWarning($"Procedure with ID:{procedureID} is already current state.");
                return;
            }

            if (fsm.CanEnterState(procedureID) == false)
            {
                Debug.LogError($"Failed to enter procedure with ID:{procedureID}.");
                return;
            }
            
            StartLoading(procedureID, ProcedureLoadingType.OnEnter, () =>
            {
                if (fsm.CanEnterState(procedureID) == false)
                {
                    throw new InvalidOperationException($"Failed to enter procedure with ID:{procedureID}.");
                }
                
                fsm.EnterState(procedureID);
                
                OnEnterProcedureEvent?.Invoke(procedureID);
            }).Forget();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ExitProcedureImmediately(string procedureID)
        {
            ExitProcedureImmediately(procedureID, () => { });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ExitProcedureImmediately(string procedureID, Action onExit)
        {
            if (IsLoading)
            {
                Debugger.LogWarning("ProcedureManager is still loading, cannot switch procedure.");
                return;
            }
            
            if (fsm.HasCurrentState(procedureID) == false)
            {
                Debugger.LogWarning($"Procedure with ID:{procedureID} is not current state.");
                return;
            }

            if (fsm.CanExitState(procedureID) == false)
            {
                Debug.LogError($"Failed to exit procedure with ID:{procedureID}.");
                return;
            }

            StartLoading(procedureID, ProcedureLoadingType.OnExit, () =>
            {
                if (fsm.CanExitState(procedureID) == false)
                {
                    throw new InvalidOperationException($"Failed to exit procedure with ID:{procedureID}.");
                }

                fsm.ExitState(procedureID);
                
                OnExitProcedureEvent?.Invoke(procedureID);
                
                onExit();
            }).Forget();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnterProcedure(string procedureID)
        {
            if (IsLoading)
            {
                Debugger.LogWarning("ProcedureManager is still loading, cannot switch procedure.");
                return;
            }
            
            if (procedures.ContainsKey(procedureID) == false)
            {
                throw new ArgumentException($"Procedure with ID:{procedureID} does not exist.");
            }
            
            procedureSwitchQueue.Enqueue((null, procedureID));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnterProcedure(string fromProcedureID, string toProcedureID)
        {
            if (IsLoading)
            {
                Debugger.LogWarning("ProcedureManager is still loading, cannot switch procedure.");
                return;
            }
            
            if (procedures.ContainsKey(fromProcedureID) == false)
            {
                throw new ArgumentException($"Procedure with ID:{fromProcedureID} does not exist.");
            }

            if (procedures.ContainsKey(toProcedureID) == false)
            {
                throw new ArgumentException($"Procedure with ID:{toProcedureID} does not exist.");
            }

            procedureSwitchQueue.Enqueue((fromProcedureID, toProcedureID));
        }

        #endregion
    }
}