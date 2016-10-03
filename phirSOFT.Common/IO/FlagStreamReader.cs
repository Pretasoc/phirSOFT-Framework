// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="FlagStreamReader.cs">
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
    using System;
    using System.IO;

    /// <summary>
    ///     Provides a reader for a flagstream.
    /// </summary>
    [PublicAPI]
    public class FlagStreamReader
    {
        /// <summary>
        ///     Creates a instance of the <see cref="FlagStreamReader" /> class.
        /// </summary>
        /// <param name="innerStream">The stream to read from</param>
        /// <exception cref="ArgumentNullException"><paramref name="innerStream" /> is <see langword="null" />.</exception>
        public FlagStreamReader([NotNull] Stream innerStream)
        {
            if (innerStream == null)
                throw new ArgumentNullException(nameof(innerStream));

            InnerStream = new MemoryStream();
            var sw = new BitStreamWriter(InnerStream);
            var run = true;
            while (run)
            {
                var b = innerStream.ReadByte();
                if (b == -1)
                {
                    run = false;
                }
                else
                {
                    run = (b & (1 << 7)) == 1;

                    for (var i = 0; i < 7; i++)
                        sw.Write(b & (1 << i));
                }
            }

            InnerStream.Seek(0, SeekOrigin.Begin);
        }

        /// <summary>
        ///     Gets the data stream of the flagstream.
        /// </summary>
        [PublicAPI]
        public Stream InnerStream { get; }

        /// <inheritdoc />
        /// <inheritdoc />
        public long Seek(long offset, SeekOrigin origin)
        {
            return InnerStream.Seek(offset, origin);
        }

        /// <inheritdoc />
        public int Read(byte[] buffer, int offset, int count)
        {
            return InnerStream.Read(buffer, offset, count);
        }
    }
}