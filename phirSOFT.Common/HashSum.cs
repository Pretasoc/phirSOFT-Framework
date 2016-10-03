// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="HashSum.cs">
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
using phirSOFT.Strings;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Cryptography;

namespace phirSOFT.Common
{
    /// <summary>
    ///     Represents a hash sum for a given <see cref="HashAlgorithm" />.
    /// </summary>
    /// <typeparam name="T">
    ///     The <see cref="HashAlgorithm" /> that computes the hash sums. The algorithm must have a
    ///     parameterless constructor.
    /// </typeparam>
    [PublicAPI]
    [ImmutableObject(true)]
    public struct HashSum<T> : IEquatable<HashSum<T>>, ICloneable where T : HashAlgorithm, new()
    {
        private readonly byte[] _hash;

        [SuppressMessage("ReSharper", "StaticMemberInGenericType",
             Justification = "The different value per type is intendet.")]
        private static readonly int HashSize;

        /// <summary>
        ///     Represents an empty hash sum.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static readonly HashSum<T> Empty = new HashSum<T>(new byte[HashSize]);

        /// <summary>
        ///     Creates a new hash sum by interpreting a byte array as hash sum.
        /// </summary>
        /// <param name="hash"></param>
        /// <returns>A <see cref="HashSum{T}" /> instance where <see cref="Hash" /> is <paramref name="hash" />.</returns>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        [PublicAPI]
        public static HashSum<T> FromBytes(byte[] hash)
        {
            return new HashSum<T>(hash);
        }

        /// <summary>
        ///     Calculates the hash sum for a given object.
        /// </summary>
        /// <param name="data">The object to calculate the hash sum for.</param>
        /// <remarks><paramref name="data" /> has to be serializeable.</remarks>
        /// <returns>A hash sum containing the hash sum of the object</returns>
        /// <exception cref="ArgumentException">Thown, if the serialization of the object failed.</exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        [PublicAPI]
        public static HashSum<T> FromObject([NotNull] object data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            try
            {
                return FromObject(SerializationTools.SerializeObject(data));
            }
            catch (SerializationException ex)
            {
                throw new ArgumentException(SR.ex_SerialisationFailed, nameof(data), ex);
            }
        }

        /// <summary>
        ///     Calculates the hashsum of an given buffer.
        /// </summary>
        /// <param name="buffer">The buffer to calculate the hashsum for</param>
        /// <returns>A hash sum containing the hash sum of the buffer</returns>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        [PublicAPI]
        public static HashSum<T> FromObject(byte[] buffer)
        {
            using (var provider = new T())
            {
                provider.Initialize();
                return new HashSum<T>(provider.ComputeHash(buffer));
            }
        }

        /// <summary>
        ///     Calculates the hashsum of an given stream.
        /// </summary>
        /// <param name="data">The stream containing the buffer to calculate the hashsum for</param>
        /// <returns>A hash sum containing the hash sum of the buffer</returns>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        [PublicAPI]
        public static HashSum<T> FromObject(Stream data)
        {
            using (var provider = new T())
            {
                provider.Initialize();
                return new HashSum<T>(provider.ComputeHash(data));
            }
        }

        /// <summary>
        ///     Calculates the hashsum of an given buffer.
        /// </summary>
        /// <param name="buffer">The buffer to calculate the hashsum for</param>
        /// <param name="offset">The index of the first byte to use. </param>
        /// <param name="count">The count of byte to use for calculation</param>
        /// <returns>A hash sum containing the hash sum of the buffer</returns>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        [PublicAPI]
        public static HashSum<T> FromObject(byte[] buffer, int offset, int count)
        {
            using (var provider = new T())
            {
                provider.Initialize();
                return new HashSum<T>(provider.ComputeHash(buffer, offset, count));
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2207:InitializeValueTypeStaticFieldsInline",
             Justification =
                 "Because HashSum is private a instance constructor is called before the member is accessed.")]
        static HashSum()
        {
            HashSize = new T().HashSize/8;
        }

        private HashSum(byte[] hash)
        {
            if (hash == null)
                throw new ArgumentNullException(nameof(hash));
            if (hash.Length == HashSize)
                _hash = hash;
            else
                throw new ArgumentException(SR.ex_NotAHash, nameof(hash), new InvalidSizeException());
        }

        /// <summary>
        ///     Gets the byte array representing the hash sum.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays",
             Justification = "The array is small enough to keep this a property.")]
        [PublicAPI]
        public byte[] Hash
        {
            get
            {
                var returnValue = new byte[HashSize];

                // Exceptions cannot occur because we know size and type of the source and destination array.
                _hash.CopyTo(returnValue, 0);
                return returnValue;
            }
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is HashSum<T>)) return false;

            try
            {
                return _hash.CompareArrays(((HashSum<T>) obj)._hash);
            }
            catch (RankException)
            {
                Debug.WriteLine("Why do we have two HashSums of the same type with different lengths?");
                return false;
            }
        }

        /// <summary>Determines whether the specified  hash sum is equal to the current  hash sum.</summary>
        /// <returns>true if the specified  hash sum is equal to the current  hash sum; otherwise, false.</returns>
        /// <param name="other">The  hash sum to compare with the current  hash sum. </param>
        public bool Equals(HashSum<T> other)
        {
            if (other.Hash == null)
                return Hash == null;

            try
            {
                return _hash.CompareArrays(other._hash);
            }
            catch (RankException)
            {
                Debug.WriteLine("Why do we have two HashSums of the same type with different lengths?");
                return false;
            }
        }

        /// <summary>
        ///     Gets an hash code for this hash.
        /// </summary>
        /// <returns>The hash code for the hash sum.</returns>
        public override int GetHashCode()
        {
            return _hash.GetHashCode();
        }

        /// <inheritdoc />
        public object Clone()
        {
            return new HashSum<T>(Hash);
        }

        /// <summary>
        ///     Determinates whether <paramref name="left" /> is unequal to <paramref name="right" />.
        /// </summary>
        /// <param name="left">The first <see cref="HashSum{T}" /> to compare.</param>
        /// <param name="right">The second <see cref="HashSum{T}" /> to compare.</param>
        /// <returns>True, if <paramref name="left" /> is not equal to <paramref name="right" />, otherwise false.</returns>
        public static bool operator !=(HashSum<T> left, HashSum<T> right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        ///     Determinates whether <paramref name="left" /> is equal to <paramref name="right" />.
        /// </summary>
        /// <param name="left">The first <see cref="HashSum{T}" /> to compare.</param>
        /// <param name="right">The second <see cref="HashSum{T}" /> to compare.</param>
        /// <returns>True, if <paramref name="left" /> is equal to <paramref name="right" />, otherwise false.</returns>
        public static bool operator ==(HashSum<T> left, HashSum<T> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Umpacks the <see cref="HashSum{T}" /> to an byte array.
        /// </summary>
        /// <param name="hash">The <see cref="HashSum{T}" /> to unpack.</param>
        /// <returns>A byte array tha represents the <see cref="HashSum{T}" />.</returns>
        public static implicit operator byte[](HashSum<T> hash) => hash._hash;

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return _hash.PrintAsNumber();
        }
    }
}