// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="FlagStreamWriter.cs">
// Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// <summary>
// phirSOFT Package phirSOFT.Common
// 
// Created by:    Philemon Eichin
// Created:       03.10.2016 13:33
// Last Modified: 03.10.2016 14:41
// </summary>
//  
// --------------------------------------------------------------------------------------------------------------------

namespace phirSOFT.Common.IO
{
    using JetBrains.Annotations;
    using System.IO;

    /// <summary>
    ///     Provides a implementation of a Flagstream writer.
    /// </summary>
    [PublicAPI]
    public class FlagStreamWriter : BitStreamWriter
    {
        /// <summary>
        ///     Creates a new instance of the flagstream writer.
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        public FlagStreamWriter(Stream stream) : base(stream)
        {
        }

        /// <inheritdoc />
        protected override void FlushBuffer()
        {
            var overflow = BufferState == BufferState.Full;
            var last = base[7]; //Buffer[7];
            base[7] = overflow;

            base.FlushBuffer();

            if (overflow)
                WriteBit(last);
        }
    }
}