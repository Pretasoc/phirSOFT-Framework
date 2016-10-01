// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="InvalidSizeException.cs">
// Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// <summary>
// phirSOFT Package phirSOFT.Common
// 
// Created by:    Philemon Eichin
// Created:       01.10.2016 01:59
// Last Modified: 01.10.2016 02:03
// </summary>
//  
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using phirSOFT.Strings;

namespace phirSOFT.Common
{
    /// <summary>
    ///     Raised when an array has not the required size.
    /// </summary>
    [Serializable]
    [PublicAPI]
    public class InvalidSizeException : ArgumentException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidSizeException" /> class with a given message.
        /// </summary>
        /// <param name="message">The message describing the exception.</param>
        public InvalidSizeException(string message) : base(SR.ex_NotMd5 + string.Format(SR.ex_OriginalMessage, message))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidSizeException" /> class.
        /// </summary>
        public InvalidSizeException() : base(SR.ex_NotMd5)
        {
        }

        /// <exception cref="ArgumentNullException"><paramref name="info" /> is <see langword="null" />.</exception>
        /// <exception cref="SerializationException">The deserialization of the exception failed. See Inner Exception for details.</exception>
        protected InvalidSizeException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));

            try
            {
                ActualSize = info.GetInt32("ActualSize");
                RequestedSize = info.GetInt32("RequestedSize");
            }
            catch (InvalidCastException ex)
            {
                throw new SerializationException(SR.ex_SerialisationFailed, ex);
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidSizeException" /> class
        ///     with a given message and an inner exception, that causes this exception.
        /// </summary>
        /// <param name="message">The message describing the exception.</param>
        /// <param name="innerException">
        ///     The exception that caused this exception.
        /// </param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public InvalidSizeException(string message, Exception innerException)
            : base(SR.ex_NotMd5 + string.Format(SR.ex_OriginalMessage, message), innerException)
        {
        }

        public int RequestedSize { get; }
        public int ActualSize { get; }

        public override string ParamName => "hash";
    }
}