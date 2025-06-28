namespace VMFramework.Properties
{
    public class ReadOnlyWrapperProperty<TValue> : IReadOnlyProperty<TValue>, IProperty
    {
        public IReadOnlyProperty<TValue> WrappedProperty { get; protected set; }

        public object Owner { get; protected set; }

        public object ObjectValue => WrappedProperty.ObjectValue;

        public TValue DefaultValue { get; set; } = default;

        public event PropertyDirtyHandler OnDirty;

        public event PropertyChangedHandler<TValue> OnChanged;

        protected readonly PropertyChangedHandler<TValue> onWrapperChangedFunc;

        protected TValue lastValue;

        public ReadOnlyWrapperProperty()
        {
            onWrapperChangedFunc = OnWrapperChanged;
        }

        public void SetOwner(object owner)
        {
            Owner = owner;
        }

        public void Set(IReadOnlyProperty<TValue> property)
        {
            bool isDirty = false;
            TValue currentValue = DefaultValue;

            if (WrappedProperty != null)
            {
                isDirty = true;
                WrappedProperty.OnChanged -= onWrapperChangedFunc;
            }

            WrappedProperty = property;

            if (property != null)
            {
                currentValue = property.GetValue();
                property.OnChanged += onWrapperChangedFunc;
                isDirty = true;
            }

            if (isDirty)
            {
                OnDirty?.Invoke(Owner, true);
                OnChanged?.Invoke(Owner, lastValue, currentValue, true);
            }
        }

        protected void OnWrapperChanged(object owner, TValue previous, TValue current, bool initial)
        {
            OnDirty?.Invoke(Owner, initial);
            OnChanged?.Invoke(Owner, previous, current, initial);
            lastValue = current;
        }

        public TValue GetValue()
        {
            return WrappedProperty.GetValue();
        }
    }
}