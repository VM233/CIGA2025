using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using VMFramework.OdinExtensions;

namespace VMFramework.Properties
{
    [PreviewComposite]
    public class SimpleProperty<TValue> : IProperty<TValue>
    {
        public delegate bool CanSetValueHandler(object owner, TValue previous, TValue next, bool initial);

        public object Owner { get; private set; }

        protected TValue value;

        [ShowInInspector]
        [DelayedProperty]
        public TValue Value
        {
            get => value;
            set => SetValue(value, initial: false);
        }

        public event PropertyDirtyHandler OnDirty;
        public event PropertyChangedHandler<TValue> OnChanged;

        protected CanSetValueHandler canSetValueFunc;

        public void Initialize(CanSetValueHandler canSetValueFunc = null)
        {
            this.canSetValueFunc = canSetValueFunc;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetOwner(object owner)
        {
            Owner = owner;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SetValue(TValue value, bool initial)
        {
            if (canSetValueFunc != null && canSetValueFunc(Owner, previous: this.value, next: value, initial) == false)
            {
                return;
            }

            var previous = this.value;
            this.value = value;
            OnChanged?.Invoke(Owner, previous, this.value, initial);
            OnDirty?.Invoke(Owner, initial);
        }

        public virtual TValue GetValue()
        {
            return value;
        }

        #region To String

        public override string ToString()
        {
            return value.ToString();
        }

        #endregion

        public static implicit operator TValue(SimpleProperty<TValue> property) => property.value;
    }
}