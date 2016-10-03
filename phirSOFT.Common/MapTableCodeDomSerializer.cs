// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="MapTableCodeDomSerializer.cs">
// Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// <summary>
// phirSOFT Package phirSOFT.Common
// 
// Created by:    Philemon Eichin
// Created:       01.10.2016 17:16
// Last Modified: 03.10.2016 12:58
// </summary>
//  
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.CodeDom;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;

namespace phirSOFT.Common
{
    /// <summary>
    ///     Provides a <see cref="CodeDomSerializer" /> for a <see cref="MapTable{TIn,TOut}" />.
    /// </summary>
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

                                // All exception cannot happen, because we know the exact type.
                                // This should be proved through a text, in case that the API changes
                                // the reflection might not work any more.
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