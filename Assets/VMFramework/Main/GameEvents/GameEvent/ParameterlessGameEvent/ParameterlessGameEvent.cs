using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;
using VMFramework.Properties;

namespace VMFramework.GameEvents
{
    public class ParameterlessGameEvent : GameItem, IParameterlessGameEvent
    {
        [ShowInInspector]
        private readonly SortedDictionary<int, HashSet<Action>> callbacks = new();

        [ShowInInspector]
        private readonly Dictionary<Action, int> callbacksLookup = new();

        public IDisableTokenProperty DisableToken => disableToken;

        [ShowInInspector]
        protected readonly DisableTokenProperty disableToken = new();

        #region Pool Events

        protected override void OnCreate()
        {
            base.OnCreate();

            disableToken.SetOwner(this);
            
            if (IsDebugging)
            {
                AddCallback(DebugLog, PriorityDefines.SUPER);
            }
        }

        protected override void OnReturn()
        {
            base.OnReturn();

            bool hasExtraCallbacks = false;
            if (IsDebugging)
            {
                if (callbacksLookup.Count > 1)
                {
                    hasExtraCallbacks = true;
                }
            }
            else
            {
                if (callbacksLookup.Count > 0)
                {
                    hasExtraCallbacks = true;
                }
            }

            if (hasExtraCallbacks)
            {
                Debugger.LogWarning($"{this} has extra callbacks. Callbacks Count : {callbacksLookup.Count}");
            }
        }

        #endregion

        private void DebugLog()
        {
            Debugger.LogWarning($"{this} was triggered!");
        }
        
        public void Reset()
        {
            foreach (var actions in callbacks.Values)
            {
                actions.Clear();
            }
            callbacksLookup.Clear();
            disableToken.Clear();
        }

        #region Callbacks

        public void AddCallback(Action callback, int priority)
        {
            if (callback == null)
            {
                Debug.LogError($"Cannot add null callback to {this}");
                return;
            }

            if (callbacks.TryAdd(callbacksLookup, priority, callback) == false)
            {
                var methodName = callback.Method.Name;
                Debugger.LogWarning($"Callback {methodName} already exists in {this} with priority {priority}.");
            }
        }

        public void RemoveCallback(Action callback)
        {
            if (callback == null)
            {
                Debug.LogError($"Cannot remove null callback from {this}");
                return;
            }

            if (callbacks.TryRemove(callbacksLookup, callback) == false)
            {
                Debugger.LogWarning($"Callback {callback.Method.Name} does not exist in {this}");
            }
        }

        #endregion

        #region Propagate

        protected virtual bool CanPropagate()
        {
            if (disableToken.IsEnabled == false)
            {
                Debugger.LogWarning($"GameEvent:{id} is disabled. Cannot propagate.");
                return false;
            }

            return true;
        }

        private readonly List<Action> tempCallbacks = new();

        public void Propagate()
        {
            if (CanPropagate() == false)
            {
                return;
            }

            tempCallbacks.Clear();

            foreach (var (_, set) in callbacks)
            {
                foreach (var callback in set)
                {
                    tempCallbacks.Add(callback);
                }
            }

            foreach (var callback in tempCallbacks)
            {
                callback();
            }

            tempCallbacks.ReturnToDefaultPool();

            OnPropagationStopped();
        }

        protected virtual void OnPropagationStopped()
        {

        }

        #endregion
    }
}