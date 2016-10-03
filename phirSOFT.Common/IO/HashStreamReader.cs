// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="HashStreamReader.cs">
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
    using System.Security.Cryptography;
    using System.Threading.Tasks;

    /// <summary>
    ///     Provides a stream reader, that can be read a buffer from a stream identified by a <see cref="HashSum{T}" />.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="HashSum{T}" /></typeparam>
    public class HashStreamReader<T> where T : HashAlgorithm, new()
    {
        /// <summary>
        ///     Creates a new instance of a <see cref="HashStreamReader{T}" /> for a given stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public HashStreamReader(Stream stream)
        {
            BaseStream = stream;
        }

        /// <summary>
        ///     Gets the base stream of the <see cref="HashStreamReader{T}" />
        /// </summary>
        [PublicAPI]
        public Stream BaseStream { get; }

        /// <summary>
        ///     Reads the next block from the stream.
        /// </summary>
        /// <param name="hash">The <see cref="HashSum{T}" /> that identifies the block.</param>
        /// <returns>The block, that is identified by the hash.</returns>
        /// <exception cref="EndOfStreamException">Thrown, if the end of the stream is reached and the hash could not be found.</exception>
        [PublicAPI]
        public byte[] Read(HashSum<T> hash)
        {
            using (var ms = new MemoryStream())
            {
                while (HashSum<T>.FromObject(ms.GetBuffer()) != hash)
                {
                    var value = BaseStream.ReadByte();

                    if (value == -1)
                        throw new EndOfStreamException();

                    ms.WriteByte((byte) value);
                }

                return ms.GetBuffer();
            }
        }

        /// <summary>
        ///     Reads the next block asynchronous from the stream.
        /// </summary>
        /// <param name="hash">The <see cref="HashSum{T}" /> that identifies the block.</param>
        /// <returns>The block, that is identified by the hash.</returns>
        /// <exception cref="EndOfStreamException">Thrown, if the end of the stream is reached and the hash could not be found.</exception>
        [PublicAPI]
        public async Task<byte[]> ReadAsync(HashSum<T> hash)
        {
            return await Task.Run(() => Read(hash)).ConfigureAwait(true);
        }
    }
}