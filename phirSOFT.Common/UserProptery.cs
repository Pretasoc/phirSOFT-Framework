using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace phirSOFT.Common
{
    /// <summary>
    /// Provides Property that can be overriden by the user. The original value can still be restored, by calling <see cref="Reset"/> or setting value to null.
    /// </summary>
    /// <typeparam name="T">The type of the Property</typeparam>
    public class UserPropterty<T>
    {
        private readonly bool nullable;

        private readonly Func<T> getter;

        private bool valuePresent;
        private T value;


        public UserPropterty(T defaultValue) : this(() => defaultValue)
        {

        }

        public UserPropterty([NotNull] Func<T> defaultValueGetter)
        {
            if (defaultValueGetter == null) throw new ArgumentNullException(nameof(defaultValueGetter));
            getter = defaultValueGetter;
            nullable = !typeof(T).IsValueType || (Nullable.GetUnderlyingType(typeof(T)) != null);
        }

        [PublicAPI]
        public void Reset()
        {
            valuePresent = false;
        }

        [PublicAPI]
        public bool ShouldSerializeValue()
        {
            return valuePresent;
        }

        [PublicAPI]
        public T Value
        {
            get { return valuePresent ? value : getter.Invoke(); }
            set
            {
                if (nullable && (dynamic)value == null)
                    valuePresent = false;
                else
                    this.value = value;

            }
        }

    }

}
