using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using VMFramework.Properties;

namespace VMFramework.UI
{
    public abstract class SingleBinderModifier : BinderModifier
    {
        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        public object BindTarget { get; private set; }

        protected sealed override void OnBindTargetAdded(object target)
        {
            base.OnBindTargetAdded(target);

            SetBindTarget(target);
        }

        protected sealed override void OnBindTargetRemoved(object target)
        {
            base.OnBindTargetRemoved(target);

            if (BindTarget == null)
            {
                return;
            }

            if (BindTarget == target)
            {
                SetBindTarget(null);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetBindTarget(object newBindTarget)
        {
            if (Panel.IsOpened == false && newBindTarget != null)
            {
                throw new InvalidOperationException($"Cannot bind {newBindTarget} before panel is opened!");
            }

            ProcessTargetUnbind(BindTarget);

            BindTarget = newBindTarget;

            ProcessTargetBind(BindTarget);
        }

        protected virtual void ProcessTargetBind([MaybeNull] object target)
        {

        }

        protected virtual void ProcessTargetUnbind([MaybeNull] object target)
        {

        }
    }

    public abstract class SingleBinderModifier<TTarget> : BinderModifier<TTarget>
        where TTarget : class
    {
        [BoxGroup(RUNTIME_DATA_CATEGORY)]
        [ShowInInspector]
        public TTarget BindTarget => bindTargetProperty.GetValue();
        
        protected readonly SimpleProperty<TTarget> bindTargetProperty = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            bindTargetProperty.SetOwner(this);
        }

        protected sealed override void OnBindTargetAdded(TTarget target)
        {
            SetBindTarget(target);
        }

        protected sealed override void OnBindTargetRemoved(TTarget target)
        {
            if (BindTarget == null)
            {
                return;
            }

            if (BindTarget == target)
            {
                SetBindTarget(null);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetBindTarget(TTarget newBindTarget)
        {
            if (Panel.IsOpened == false && newBindTarget != null)
            {
                throw new InvalidOperationException(
                    $"Cannot bind {typeof(TTarget).Name}: {newBindTarget} before panel is opened!");
            }

            ProcessTargetUnbind(BindTarget);

            bindTargetProperty.SetValue(newBindTarget, initial: false);

            ProcessTargetBind(BindTarget);
        }

        protected virtual void ProcessTargetBind([MaybeNull] TTarget target)
        {

        }

        protected virtual void ProcessTargetUnbind([MaybeNull] TTarget target)
        {

        }
    }
}