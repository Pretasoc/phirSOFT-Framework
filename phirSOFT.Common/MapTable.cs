// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="MapTable.cs">
// Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// <summary>
// phirSOFT Package phirSOFT.Common
// 
// Created by:    Philemon Eichin
// Created:       30.09.2016 13:54
// Last Modified: 01.10.2016 02:03
// </summary>
//  
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.CodeDom;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
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

    public class MapTableCodeDomSerializer : CodeDomSerializer
    {
        /// <exception cref="AmbiguousMatchException">More than one indexer was found.</exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="manager" /> or <paramref name="value" /> is
        ///     <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="value" /> is not a <see cref="MapTable{TIn,TOut}" />.</exception>
        public override object Serialize(IDesignerSerializationManager manager, object value)
        {
            if (manager == null) throw new ArgumentNullException(nameof(manager));
            if (value == null) throw new ArgumentNullException(nameof(value));

            try
            {
                if (value.GetType().BaseType ==
                    typeof(MapTable<,>).MakeGenericType(value.GetType().GenericTypeArguments))
                {
                    var csc = new CodeStatementCollection();
                    var enumIn = value.GetType().GetGenericArguments()[0];
                    var enumOut = value.GetType().GetGenericArguments()[1];


                    var indexerProperty = value.GetType().GetGenericTypeDefinition()?.GetProperty("Item");

                    Debug.Assert(indexerProperty != null);

                    var enumInSerializer = GetSerializer(manager, enumIn);
                    if (enumInSerializer == null) throw new ArgumentNullException(nameof(enumInSerializer));
                    var enumOutSerializer = GetSerializer(manager, enumOut);
                    if (enumOutSerializer == null) throw new ArgumentNullException(nameof(enumOutSerializer));

                    var expression = GetExpression(manager, value);

                    if (expression != null)
                        foreach (var key in Enum.GetValues(enumIn))
                        {
                            var keyStatement = (CodeExpression) enumInSerializer.Serialize(manager, key);
                            var valueStatement =
                                (CodeExpression)

                                //All exception cannot happen, because we know the exact type.
                                //This should be proved through a text, in case that the API changes
                                //the reflection might not work any more.
                                enumOutSerializer.Serialize(manager, indexerProperty.GetValue(value, new[] {key}));


                            var targetObject = new CodeIndexerExpression(expression, keyStatement);

                            csc.Add(new CodeAssignStatement(targetObject, valueStatement));
                        }
                    return csc;
                }
            }
            catch (NotSupportedException ex)
            {
                throw new ArgumentException("Value is not a valid MapTable<>", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException("Value is not a valid MapTable<>", ex);
            }
            return base.Serialize(manager, value);
        }
    }
}