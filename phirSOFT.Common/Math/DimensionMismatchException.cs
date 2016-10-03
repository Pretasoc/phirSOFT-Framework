// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="DimensionMismatchException.cs">
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
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    ///     Trown if the Dimensions of two matrices did not match for the requested matrix operation.
    /// </summary>
    [Serializable]
    public class DimensionMismatchException : RankException
    {
        /// <inheritdoc cref="Exception(string)" />
        public DimensionMismatchException(string message) : base(message)
        {
        }

        /// <inheritdoc cref="Exception(string,Exception)" />
        public DimensionMismatchException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <inheritdoc cref="Exception()" />
        public DimensionMismatchException()
        {
        }

        /// <inheritdoc cref="Exception(SerializationInfo,StreamingContext)" />
        /// <exception cref="ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="SerializationException">
        ///     The class name is null or <see cref="P:System.Exception.HResult" /> is zero
        ///     (0).
        /// </exception>
        protected DimensionMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}