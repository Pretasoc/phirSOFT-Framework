// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="SerializationTools.cs">
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

using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace phirSOFT.Common
{
    /// <summary>
    ///     Provides tools for serializing and deserializing objects.
    /// </summary>
    [PublicAPI]
    public static class SerializationTools
    {
        /// <summary>
        ///     Performs a bulk serialization of multiple objects
        /// </summary>
        /// <param name="objects">A collection of all objects to serialize</param>
        /// <returns>An <see cref="IEnumerable{T}" /> that enumerates the serialized objects.</returns>
        /// <remarks>The objects are serialized when the serialized form is requested.</remarks>
        /// <exception cref="SerializationException">
        ///     An error has occurred during serialization. Maybe one of the
        ///     <paramref name="objects" /> is not serializeable.
        /// </exception>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="ArgumentNullException"><paramref name="objects" /> is <see langword="null" />.</exception>
        [DebuggerStepThrough]
        public static IEnumerable<byte[]> BulkSerialize(params object[] objects)
        {
            if (objects == null)
                throw new ArgumentNullException(nameof(objects));

            foreach (var item in objects)
                yield return SerializeObject(item);
        }

        /// <summary>
        ///     Performs a bulk deserialization of multiple objects.
        /// </summary>
        /// <param name="objects">A collection of all objects to deserialize.</param>
        /// <returns>An <see cref="IEnumerable" /> that enumerates the deserialized objects.</returns>
        /// <remarks>The objects are deserialized when the deserialized form is requested.</remarks>
        /// <exception cref="SerializationException">An object could not be deserialized.</exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="objects" /> is <see langword="null" />. Or one item of
        ///     <paramref name="objects" /> is null
        /// </exception>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        [DebuggerStepThrough]
        public static IEnumerable BulkDeserialize(params byte[][] objects)
        {
            if (objects == null)
                throw new ArgumentNullException(nameof(objects));
            foreach (var item in objects)
                yield return DeserializeObject(item);
        }

        /// <summary>
        ///     Serializes an <paramref name="value" /> to a byte array using a <see cref="BinaryFormatter" />.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <returns>A byte array representing the serialized object</returns>
        /// <remarks>
        ///     <list type="bullet">
        ///         <items>
        ///             <description><paramref name="value" /> has to be serializeable.</description>
        ///             <description>The value can be recreated using <see cref="DeserializeObject(byte[])" /></description>
        ///         </items>
        ///     </list>
        /// </remarks>
        /// <exception cref="SerializationException">
        ///     An error has occurred during serialization, such as if an object in the
        ///     <paramref name="value" /> parameter is not marked as serializable.
        /// </exception>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
        [DebuggerStepThrough]
        public static byte[] SerializeObject([NotNull] object value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                SerializeObject(value, memoryStream, binaryFormatter);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        ///     Serializes a <paramref name="value" /> into a <paramref name="stream" /> using a given
        ///     <paramref name="formatter" />.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <param name="stream">The stream to write the data to.</param>
        /// <param name="formatter">The formatter used to serialize the data</param>
        /// <remarks>
        ///     <list type="bullet">
        ///         <items>
        ///             <description><paramref name="value" /> has to be serializeable.</description>
        ///             <description>
        ///                 The value can be recreated using <see cref="DeserializeObject(Stream,IFormatter)" />
        ///             </description>
        ///         </items>
        ///     </list>
        /// </remarks>
        /// <exception cref="SerializationException">
        ///     An error has occurred during serialization, such as if an object in the
        ///     <paramref name="value" /> parameter is not marked as serializable.
        /// </exception>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        public static void SerializeObject(object value, Stream stream, IFormatter formatter)
        {
            formatter.Serialize(stream, value);
        }

        /// <summary>
        ///     Deserializes an object from a byte array using a <see cref="BinaryFormatter" />.
        /// </summary>
        /// <param name="bufffer">The byte array representing the serialized object.</param>
        /// <returns>The deserialized object.</returns>
        /// <exception cref="SerializationException">The object could not be deserialized.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="bufffer" /> is <see langword="null" />.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        /// <seealso cref="SerializeObject(object)" />
        [DebuggerStepThrough]
        [PublicAPI]
        public static object DeserializeObject([NotNull] byte[] bufffer)
        {
            if (bufffer == null) throw new ArgumentNullException(nameof(bufffer));
            object ret;

            using (var memoryStream = new MemoryStream(bufffer))
            {
                memoryStream.Position = 0;
                var binaryFormatter = new BinaryFormatter();
                ret = DeserializeObject(memoryStream, binaryFormatter);
            }

            return ret;
        }

        /// <summary>
        ///     Deserializes a object from a <paramref name="stream" /> using a given <paramref name="formatter" />.
        /// </summary>
        /// <param name="stream">The stream to read the object from.</param>
        /// <param name="formatter">The formatter used to deserialize the data</param>
        /// <returns>The deserialized object.</returns>
        /// <exception cref="SerializationException">
        ///     An error has occurred during deserialization.
        /// </exception>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        /// <seealso cref="SerializeObject(object,Stream,IFormatter)" />
        public static object DeserializeObject(Stream stream, IFormatter formatter)
        {
            return formatter.Deserialize(stream);
        }
    }
}