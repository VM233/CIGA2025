using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.Core.Pools;

namespace VMFramework.UI
{
    public abstract class BinderModifier : PanelModifier
    {
        [BoxGroup(CONFIGS_CATEGORY)]
        public bool addDefaultProcessor = true;

        [BoxGroup(CONFIGS_CATEGORY)]
        public bool autoBindFromBinder = true;

        public virtual IFuncTargetsProcessor<object, object> DefaultProcessor => null;

        protected IUIPanelObjectsBinder binder;

        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        public readonly RingTargetsProcessorPipeline<object> pipeline = new();

        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        protected readonly Dictionary<object, List<object>> bindTargetsLookup = new();

        private readonly HashSet<object> bindTargets = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            binder = Panel.GetComponent<IUIPanelObjectsBinder>();
            binder.OnBindObjectAdded += OnBindObjectAdded;
            binder.OnBindObjectRemoved += OnBindObjectRemoved;

            Panel.OnOpenEvent += OnOpen;

            pipeline.ClearProcessors();

            var tempProcessors =
                ListPool<(IFuncTargetsProcessor<object, object> processor, int priority)>.Default.Get();
            tempProcessors.Clear();
            GetProcessors(tempProcessors);
            foreach (var info in tempProcessors)
            {
                pipeline.AddProcessor(info.processor, info.priority);
            }

            tempProcessors.ReturnToDefaultPool();
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            if (bindTargetsLookup.Count != 0)
            {
                Debugger.LogError($"{panel} has {bindTargetsLookup.Count} objects bound on OnOpen was called." +
                                  $"This usually means the bind targets were not properly removed on close.");
            }
        }

        protected virtual void GetProcessors(
            ICollection<(IFuncTargetsProcessor<object, object> processor, int priority)> processors)
        {
            if (addDefaultProcessor)
            {
                var defaultProcessor = DefaultProcessor;
                if (defaultProcessor != null)
                {
                    processors.Add((DefaultProcessor, PriorityDefines.HIGH));
                }
            }

            foreach (var processor in GetComponents<IFuncTargetsProcessor<object, object>>())
            {
                processors.Add((processor, PriorityDefines.MEDIUM));
            }
        }

        public virtual void AddBindObject(object obj)
        {
            var targets = ListPool<object>.Default.Get();
            targets.Clear();
            pipeline.Process(obj, targets);

            if (targets.Count == 0)
            {
                targets.ReturnToDefaultPool();
                return;
            }

            foreach (var target in targets)
            {
                OnBindTargetAdded(target);
            }

            bindTargetsLookup[obj] = targets;
        }

        public virtual void RemoveBindObject(object obj)
        {
            if (bindTargetsLookup.Remove(obj, out var targets))
            {
                foreach (var target in targets)
                {
                    OnBindTargetRemoved(target);
                }

                targets.ReturnToDefaultPool();
            }
        }

        protected virtual void OnBindObjectAdded(IUIPanel panel, object obj)
        {
            if (autoBindFromBinder)
            {
                AddBindObject(obj);
            }
        }

        protected virtual void OnBindObjectRemoved(IUIPanel panel, object obj)
        {
            if (autoBindFromBinder)
            {
                RemoveBindObject(obj);
            }
        }

        protected virtual void OnBindTargetAdded(object target)
        {

        }

        protected virtual void OnBindTargetRemoved(object target)
        {

        }

        public IReadOnlyCollection<object> GetBindTargets()
        {
            bindTargets.Clear();

            foreach (var targets in bindTargetsLookup.Values)
            {
                bindTargets.UnionWith(targets);
            }

            return bindTargets;
        }
    }

    public abstract class BinderModifier<TTarget> : BinderModifier
        where TTarget : class
    {
        public override IFuncTargetsProcessor<object, object> DefaultProcessor => new ComponentBindProcessor<TTarget>();

        protected sealed override void OnBindTargetAdded(object target)
        {
            base.OnBindTargetAdded(target);

            if (target is not TTarget typedTarget)
            {
                Debugger.LogWarning($"Target : {target} is not of type {typeof(TTarget).Name}. Skipping.");
                return;
            }

            OnBindTargetAdded(typedTarget);
        }

        protected sealed override void OnBindTargetRemoved(object target)
        {
            base.OnBindTargetRemoved(target);

            if (target is not TTarget typedTarget)
            {
                return;
            }

            OnBindTargetRemoved(typedTarget);
        }

        protected abstract void OnBindTargetAdded(TTarget target);

        protected abstract void OnBindTargetRemoved(TTarget target);
    }
}