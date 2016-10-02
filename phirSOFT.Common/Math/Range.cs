using JetBrains.Annotations;

namespace phirSOFT.Common.Math
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    /// <summary>
    ///     Represents a Range defined by left minimum value and left maximum value.
    /// </summary>
    /// <typeparam name="T">The type of the range.</typeparam>
    [TypeConverter(typeof(RangeTypeConverter))]
    [ImmutableObject(true)]
    [PublicAPI]
    public struct Range<T> : IEquatable<Range<T>> where T : IComparable
    {
        /// <summary>
        ///     Gets the minimum value for this range.
        /// </summary>
        public T Minimum { get; }

        /// <summary>
        ///     Gets the maximum value for this range.
        /// </summary>
        public T Maximum { get; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is Range<T>)
                return (((Range<T>) obj).Minimum.CompareTo(Minimum) == 0) &&
                       (0 == ((Range<T>) obj).Maximum.CompareTo(Maximum));

            return false;
        }

        /// <inheritdoc />
        public bool Equals(Range<T> other)
            => (other.Minimum.CompareTo(Minimum) == 0) && (0 == other.Maximum.CompareTo(Maximum));

        /// <inheritdoc />
        public static bool operator ==(Range<T> left, Range<T> right) => left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(Range<T> left, Range<T> right) => !left.Equals(right);

        /// <inheritdoc />
        public override int GetHashCode() => Minimum.GetHashCode() ^ Maximum.GetHashCode();

        /// <summary>
        ///     Creates a new range over <typeparamref name="T" />.
        /// </summary>
        /// <param name="minimum">The minimum Value</param>
        /// <param name="maximum">The maximum Value</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="maximum" /> &lt; <paramref name="minimum" /></exception>
        public Range(T minimum, T maximum)
        {
            if (minimum.CompareTo(maximum) > 0) throw new ArgumentException(nameof(minimum) + ", " + nameof(maximum));

            Minimum = minimum;
            Maximum = maximum;
        }

        /// <summary>
        ///     Determinates wheter an value lies inside of the range or not.
        /// </summary>
        /// <param name="value">The value to test</param>
        /// <returns>True, if value is in Range, false if not.</returns>
        public bool IsInside(T value) => value.CompareTo(Minimum)*value.CompareTo(Maximum) <= 0;

        /// <inheritdoc />
        public override string ToString()
            => ((FormattableString) $"[{Minimum}-{Maximum}]").ToString(CultureInfo.InvariantCulture);
    }
}