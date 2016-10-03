// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="RangeTypeConverter.cs">
// Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// <summary>
// phirSOFT Package phirSOFT.Common
// 
// Created by:    Philemon Eichin
// Created:       02.10.2016 19:41
// Last Modified: 03.10.2016 12:58
// </summary>
//  
// --------------------------------------------------------------------------------------------------------------------

namespace phirSOFT.Common.Math
{
    using Strings;
    using System;
    using System.ComponentModel;
    using System.Globalization;

    /// <summary>
    ///     Provides a type converter for Ranges. It requires an Typeconverter for the generic type to work.
    /// </summary>
    public class RangeTypeConverter : TypeConverter
    {
        private static readonly Type RangeType = typeof(Range<>);

        private readonly TypeConverter _innerTypeConverter;
        private readonly Type _specificRangeType;

        /// <summary>
        ///     Creates a new Instance of the Typeconverter.
        /// </summary>
        /// <param name="type">The Range{T} type to convert.</param>
        /// <exception cref="ArgumentException"></exception>
        public RangeTypeConverter(Type type)
        {
            if ((type?.IsGenericType ?? false) && (type.GetGenericTypeDefinition() == typeof(Range<>)) &&
                (type.GetGenericArguments().Length == 1))
            {
                var innerType = type.GetGenericArguments()[0];
                _innerTypeConverter = TypeDescriptor.GetConverter(innerType);
                _specificRangeType = RangeType.MakeGenericType(innerType);
            }
            else
            {
                throw new ArgumentException(SR.ex_IncompatibleType, nameof(type));
            }
        }

        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var sv = value as string;
            if (sv == null) return base.ConvertFrom(context, culture, value);

            sv = sv.Trim('[', ']');

            var parts = sv.Split('-');

            if (parts.Length != 2) return base.ConvertFrom(context, culture, value);
            var low = _innerTypeConverter.ConvertFrom(parts[0]);
            var upper = _innerTypeConverter.ConvertFrom(parts[1]);

            return Activator.CreateInstance(_specificRangeType, low, upper);
        }
    }
}