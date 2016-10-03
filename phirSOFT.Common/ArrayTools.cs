// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="ArrayTools.cs">
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
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using static phirSOFT.Strings.SR;

namespace phirSOFT.Common
{
    /// <summary>
    ///     Provides tools for arrays.
    /// </summary>
    [PublicAPI]
    public static class ArrayTools
    {
        /// <summary>
        ///     Compares to arrays of equality.
        /// </summary>
        /// <param name="array1">The First array to compare.</param>
        /// <param name="array2">The second array to compare</param>
        /// <returns>True, if the i-th item of <paramref name="array1" /> is equal to the i-th item of <paramref name="array2" />.</returns>
        /// <remarks>The arrays have to be of the same length.</remarks>
        /// <exception cref="ArgumentNullException">Thrown, if one of the arrays is <see langword="null" /></exception>
        /// <exception cref="RankException">
        ///     Thrown, when the length of <paramref name="array1" /> is not equal to the
        ///     length of <paramref name="array2" />
        /// </exception>
        /// <typeparam name="T">The type of the array items.</typeparam>
        [DebuggerStepThrough]
        public static bool CompareArrays<T>(this T[] array1, T[] array2)
        {
            if (array1 == null)
                throw new ArgumentNullException(nameof(array1));
            if (array2 == null)
                throw new ArgumentNullException(nameof(array2));
            if (array1.Length != array2.Length)
                throw new RankException(ex_LenghtNotEqual);

            return !array1.Where((t, i) => !t.Equals(array2[i])).Any();
        }

        /// <summary>
        ///     Joins all strings in an <see cref="Array" />.
        /// </summary>
        /// <param name="array">An <see cref="Array" /> containing strings.</param>
        /// <returns>A <see cref="string" />, thats the composition </returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="array" /> is <see langword="null" /></exception>
        [Obsolete("Use string.Join() instead.")]
        public static string ConcatArray(this object[] array) => string.Join(string.Empty, array);

        /// <summary>
        ///     Reads a part of an generic <see cref="Array" /> and returns it.
        /// </summary>
        /// <param name="array">The <see cref="Array" /> to extract the subarray from.</param>
        /// <param name="start">The zero based index to start extracting data.</param>
        /// <param name="elementCount">The amount of elements in the target <see cref="Array" />.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown, if <paramref name="start" /> is not in [0 - <paramref name="array.Lenght" />],
        ///     or <paramref name="elementCount" /> is not in [1 -  (<paramref name="array.Lenght" /> - <paramref name="start" />
        ///     )].
        /// </exception>
        /// <returns>An array containing the subarray.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="array" /> is <see langword="null" />.</exception>
        /// <typeparam name="T">The type of the array items.</typeparam>
        [DebuggerStepThrough]
        public static T[] GetArrayPart<T>(this T[] array, long start, long elementCount)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if ((start < 0) || (start >= array.LongLength))
                throw new ArgumentOutOfRangeException(nameof(start),
                    start,
                    string.Format(CultureInfo.InvariantCulture, ex_IndexOutOfRange, start, 0, array.LongLength - 1));

            if ((elementCount <= 0) || (start + elementCount > array.LongLength))
                throw new ArgumentOutOfRangeException(nameof(elementCount),
                    elementCount,
                    string.Format(CultureInfo.InvariantCulture,
                        ex_ValueOutOfRange,
                        elementCount,
                        0,
                        array.LongLength - 1 - start));

            var ret = new T[elementCount];
            for (var i = 0; i < elementCount; i++)
                ret[i] = array[i + start];
            return ret;
        }

        /// <summary>
        ///     Joins to <see cref="Array">arrays</see> of the same type.
        /// </summary>
        /// <typeparam name="T">The types of the array </typeparam>
        /// <param name="array1">The first array.</param>
        /// <param name="array2">The second array.</param>
        /// <returns>A generic array of <typeparamref name="T" /> containing the joined array.</returns>
        /// <exception cref="ArgumentNullException"> Thrown, if both arrays are null.</exception>
        /// <remarks>If one if the arrays is <see langword="null" /> the other is returned.</remarks>
        /// <exception cref="InvalidCastException" />
        /// <exception cref="RankException" />
        /// <exception cref="ArrayTypeMismatchException" />
        public static T[] JoinArrays<T>(this T[] array1, T[] array2)
        {
            if ((array1 == null) && (array2 == null))
                throw new ArgumentNullException(nameof(array1));
            if (array1 == null)
                return array2;
            if (array2 == null)
                return array1;

            var ret = new T[array1.LongLength + array2.LongLength];
            array1.CopyTo(ret, 0);
            array2.CopyTo(ret, array1.LongLength);
            return ret;
        }

        /// <summary>
        ///     Insert one array into an other.
        /// </summary>
        /// <typeparam name="T">The type of the array</typeparam>
        /// <param name="array">The array the items are inserted.</param>
        /// <param name="insertionArray">The array containing the inserted items.</param>
        /// <param name="position">The position in <paramref name="array" /> where to insert the items.</param>
        /// <returns>An array containing the items of both arrays.</returns>
        /// <exception cref="RankException" />
        /// <exception cref="ArrayTypeMismatchException" />
        /// <exception cref="InvalidCastException" />
        public static T[] InsertArray<T>(this T[] array, T[] insertionArray, int position)
        {
            var leftPart = array.GetArrayPart(0, position);
            var rightPArt = array.GetArrayPart(position, array.LongLength - position);

            return leftPart.JoinArrays(insertionArray).JoinArrays(rightPArt);
        }
    }
}