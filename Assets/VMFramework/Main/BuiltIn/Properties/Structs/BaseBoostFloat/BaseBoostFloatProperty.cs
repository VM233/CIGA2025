using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using VMFramework.OdinExtensions;

namespace VMFramework.Properties
{
    /// <summary>
    /// 用来表示小数类型带增益的属性，其值 = 基值 * 增益
    /// </summary>
    [PreviewComposite]
    public class BaseBoostFloatProperty : IProperty<BaseBoostFloat>, IFormattable
    {
        public object Owner { get; private set; }

        public BaseBoostFloat Value
        {
            get => GetValue();
            set => SetValue(value, initial: false);
        }

        private float baseValue;

        /// <summary>
        /// 基值
        /// </summary>
        [ShowInInspector]
        [DelayedProperty]
        public float BaseValue
        {
            get => baseValue;
            set => SetValue(value, boostValue, initial: false);
        }

        private float boostValue;

        /// <summary>
        /// 增益
        /// </summary>
        [ShowInInspector]
        [DelayedProperty]
        public float BoostValue
        {
            get => boostValue;
            set => SetValue(baseValue, value, initial: false);
        }

        public event PropertyDirtyHandler OnDirty;
        public event PropertyChangedHandler<BaseBoostFloat> OnChanged;

        public BaseBoostFloatProperty(float baseValue, float boostValue = 0)
        {
            this.baseValue = baseValue;
            this.boostValue = boostValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetOwner(object owner)
        {
            Owner = owner;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(BaseBoostFloat value, bool initial)
        {
            var oldBaseValue = baseValue;
            var oldBoostValue = boostValue;
            baseValue = value.baseValue;
            boostValue = value.boostValue;
            OnChanged?.Invoke(Owner, previous: new(oldBaseValue, oldBoostValue), current: new(BaseValue, BoostValue),
                initial);
            OnDirty?.Invoke(Owner, initial);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(float? baseValue, float? boostValue, bool initial)
        {
            baseValue ??= this.baseValue;
            boostValue ??= this.boostValue;
            var oldBaseValue = this.baseValue;
            var oldBoostValue = this.boostValue;
            this.baseValue = baseValue.Value;
            this.boostValue = boostValue.Value;
            OnChanged?.Invoke(Owner, previous: new(oldBaseValue, oldBoostValue), current: new(BaseValue, BoostValue),
                initial);
            OnDirty?.Invoke(Owner, initial);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BaseBoostFloat GetValue()
        {
            return new(BaseValue, BoostValue);
        }

        #region To String

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return GetValue().Value.ToString(format, formatProvider);
        }

        public override string ToString()
        {
            return GetValue().Value.ToString(CultureInfo.InvariantCulture);
        }

        #endregion

        public static implicit operator float(BaseBoostFloatProperty property) => property.GetValue().Value;

        public static implicit operator BaseBoostFloat(BaseBoostFloatProperty property) =>
            new(property.BaseValue, property.BoostValue);
    }
}