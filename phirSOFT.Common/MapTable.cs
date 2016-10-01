// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="MapTable.cs">
// Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// <summary>
// phirSOFT Package phirSOFT.Common
// Created by:    Philemon Eichin
// Created:       01.10.2016 14:41
// Last Modified: 01.10.2016 16:12
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace phirSOFT.Common
{
    /// <summary>
    ///     Provides a class that maps elements from one <see cref="Enum" /> space to an other.
    /// </summary>
    /// <typeparam name="TIn">The Enum to map item from.</typeparam>
    /// <typeparam name="TOut">The Enum to map Items to.</typeparam>
    [DesignerSerializer(typeof(MapTableCodeDomSerializer), typeof(CodeDomSerializer))]
    [PublicAPI]
    public class MapTable<TIn, TOut> : Component where TIn : struct where TOut : struct
    {
        private readonly Hashtable _table;

        /// <summary>
        ///     Creates a new Instance of the <see cref="MapTable{TIn,Tout}" /> class.By default the table maps every item to the
        ///     default value of <typeparamref name="TOut" />.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown, if either <typeparamref name="TIn" /> or <typeparamref name="TOut" /> is
        ///     not an enum.
        /// </exception>
        public MapTable()
        {
            if (!typeof(TIn).IsEnum)
                throw new ArgumentException(nameof(TIn));

            if (!typeof(TOut).IsEnum)
                throw new ArgumentException(nameof(TIn));

            _table = new Hashtable();
        }

        /// <summary>
        ///     Gets or sets the image of <paramref name="x" />.
        /// </summary>
        /// <param name="x">The entity to get or set the image of.</param>
        /// <returns>The image of <paramref name="x" />.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public TOut this[TIn x]
        {
            get
            {
                var y = _table[x];
                if (y == null)
                    return default(TOut);
                return (TOut) y;
            }

            set { _table[x] = value; }
        }
    }
}