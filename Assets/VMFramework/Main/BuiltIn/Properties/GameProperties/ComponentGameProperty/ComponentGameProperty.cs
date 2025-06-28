using VMFramework.Core;

namespace VMFramework.Properties
{
    public abstract class ComponentGameProperty<TComponent> : GameProperty
    {
        public override bool CanGetValueString(object target)
        {
            return TryGetComponent(target, out _);
        }

        public sealed override void GetValueAndNameString(object target, out string nameString, out string valueString, out object source)
        {
            if (TryGetComponent(target, out TComponent component))
            {
                GetValueAndNameString(component, out nameString, out valueString);
                source = component;
            }
            else
            {
                nameString = null;
                valueString = null;
                source = null;
            }
        }

        protected virtual void GetValueAndNameString(TComponent component, out string nameString,
            out string valueString)
        {
            nameString = Name;
            valueString = null;
        }

        protected virtual bool TryGetComponent(object target, out TComponent component)
        {
            return target.TryGetComponent(out component);
        }
    }
}