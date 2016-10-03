// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="BitStreamWriter.cs">
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

using System.Globalization;
using System.Text;

namespace phirSOFT.Common.IO
{
    using JetBrains.Annotations;
    using Strings;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    /// <summary>
    ///     Provides the ability to write bitwise into streams.
    /// </summary>
    [PublicAPI]
    public class BitStreamWriter : IDisposable
    {
        private readonly BinaryWriter _baseWriter;
        private readonly bool _closeStream;
        private int _bufferPosition;
        private Encoding _encoding;

        private byte _mBuffer;

        /// <summary>
        ///     Creates a new instance of the <see cref="BitStreamWriter" /> class.
        /// </summary>
        /// <param name="stream">The stream to write the data to.</param>
        /// <exception cref="ArgumentException">The stream does not support writing or is already closed. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="stream" /> is null. </exception>
        [SuppressMessage("ReSharper", "IntroduceOptionalParameters.Global",
             Justification = "We want to avoid optional parameters.")]
        public BitStreamWriter(Stream stream) : this(stream, new UTF8Encoding(), true)
        {
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="BitStreamWriter" /> class.
        /// </summary>
        /// <param name="stream">The stream to write the data to.</param>
        /// <param name="closeStream">
        ///     If true the base stream will be closed, when the writer is disposed. If false the stream
        ///     remains open. True is the default value.
        /// </param>
        /// <exception cref="ArgumentException">The stream does not support writing or is already closed. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="stream" /> is null. </exception>
        public BitStreamWriter(Stream stream, bool closeStream) : this(stream, new UTF8Encoding(), closeStream)
        {
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="BitStreamWriter" /> class.
        /// </summary>
        /// <param name="stream">The stream to write the data to.</param>
        /// <param name="encoding">The encoding, that should be used for writing strings and chars.</param>
        /// <exception cref="ArgumentException">The stream does not support writing or is already closed. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="stream" /> is null. </exception>
        [SuppressMessage("ReSharper", "IntroduceOptionalParameters.Global",
             Justification = "We want to avoid optional parameters.")]
        public BitStreamWriter(Stream stream, Encoding encoding) : this(stream, encoding, true)
        {
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="BitStreamWriter" /> class.
        /// </summary>
        /// <param name="stream">The stream to write the data to.</param>
        /// <param name="encoding">The encoding, that should be used for writing strings and chars.</param>
        /// <param name="closeStream">
        ///     If true the base stream will be closed, when the writer is disposed. If false the stream
        ///     remains open. True is the default value.
        /// </param>
        /// <exception cref="ArgumentException">The stream does not support writing or is already closed. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="stream" /> is null. </exception>
        public BitStreamWriter(Stream stream, Encoding encoding, bool closeStream)
        {
            _closeStream = closeStream;
            _baseWriter = new BinaryWriter(stream);
            BufferPosition = 0;
            Encoding = encoding;
        }

        /// <summary>
        ///     Gets or sets the encoding used for writing strings and chars.
        /// </summary>
        public Encoding Encoding
        {
            get { return _encoding; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _encoding = value;
            }
        }

        private int BufferPosition
        {
            get { return _bufferPosition; }
            set
            {
                if ((-1 < value) && (value < 8))
                {
                    _bufferPosition = value;
                    if (BufferFull && AutoBufferFlush)
                        FlushBuffer();
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(value),
                        value,
                        string.Format(CultureInfo.CurrentUICulture, SR.ex_IndexOutOfRange, value, 0, 7));
                }
            }
        }

        private bool BufferFull => BufferPosition == 8;

        /// <summary>
        ///     Gets the state of the buffer.
        /// </summary>
        /// <seealso cref="IO.BufferState" />
        protected BufferState BufferState
        {
            get
            {
                switch (BufferPosition)
                {
                    case 0:
                        return BufferState.Empty;

                    case 8:
                        return BufferState.Full;

                    default:
                        return BufferState.Filled;
                }
            }
        }

        /// <summary>
        ///     Gets wheter the stream flushes automatically, if the buffer is full.
        /// </summary>
        [SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global",
             Justification = "There may will be implementations which uses this.")]
        protected virtual bool AutoBufferFlush => true;

        /// <summary>
        ///     Gets or sets the value of the given bit in the buffer.
        /// </summary>
        /// <param name="index">The index of the flag to get or set.</param>
        /// <returns>The value of the given flag.</returns>
        protected bool this[int index]
        {
            get { return (_mBuffer & (byte) (1 << index)) != 0; }
            set
            {
                if (value) _mBuffer |= (byte) (1 << index);
                else _mBuffer &= (byte) ~(1 << index);
            }
        }

        /// <summary>
        ///     Disposes the <see cref="BitStreamWriter" />.
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Writes the buffer into the stream.
        /// </summary>
        protected virtual void FlushBuffer()
        {
            _baseWriter.Write(_mBuffer);
            BufferPosition = 0;
            _mBuffer = 0;
        }

        /// <summary>
        ///     Writes an array of boolean values into the stream. Each item will use a space of one bit.
        /// </summary>
        /// <param name="value">The bits to write to the stream.</param>
        [PublicAPI]
        public void Write(params bool[] value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            foreach (var t in value)
                WriteBit(t);
        }

        /// <summary>
        ///     Writes a single bit into the stream.
        /// </summary>
        /// <param name="value">The bit to write.</param>
        public void Write(bool value)
        {
            WriteBit(value);
        }

        /// <summary>
        ///     Writes a char into the stream.
        /// </summary>
        /// <param name="value">The char to write. The binary representation depends on the current <see cref="Encoding" />.</param>
        [PublicAPI]
        public void Write(char value)
        {
            Write(Encoding.GetBytes(new[] {value}));
        }

        /// <summary>
        ///     Writes a chars into the stream.
        /// </summary>
        /// <param name="buffer">The chars to write. The binary representation depends on the current <see cref="Encoding" />.</param>
        public void Write(char[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            foreach (var c in buffer)
                Write(c);
        }

        /// <summary>
        ///     Writes a byte into the stream.
        /// </summary>
        /// <param name="value">The value to write. The byte is written in the little endian representation.</param>
        [PublicAPI]
        public void Write(byte value)
        {
            for (var i = 0; i < 7; i++)
                Write((value & (1 << i)) == 1);
        }

        /// <summary>
        ///     Writes a bytes into the stream.
        /// </summary>
        /// <param name="buffer">The values to write. The bytes are written in the little endian representation.</param>
        public void Write(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            foreach (var c in buffer)
                Write(c);
        }

        /// <summary>
        ///     Writes a string into the stream.
        /// </summary>
        /// <param name="value">The stream to write. The binary representation depends on the current <see cref="Encoding" />.</param>
        public void Write(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            Write(Encoding.GetBytes(value));
            foreach (var character in value)
                Write(character);
        }

        /// <summary>
        ///     Writes a integer into the stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(int value)
        {
            Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        ///     Writes a long into the stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(long value)
        {
            Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        ///     Writes a bit into the stream
        /// </summary>
        /// <param name="bit">The bit to write</param>
        protected void WriteBit(bool bit)
        {
            if (BufferFull)
                throw new OverflowException();
            this[BufferPosition++] = bit;
        }

        /// <summary>
        ///     Disposes all managed resources.
        /// </summary>
        /// <param name="disposing">Set this to false, if the object was already disposed and is now destroyed.</param>
        [SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global")]
        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (BufferState != BufferState.Empty)
                FlushBuffer();
            if (_closeStream)
                _baseWriter.Close();
        }

        /// <summary>
        ///     Destroys the object.
        /// </summary>
        ~BitStreamWriter()
        {
            Dispose(false);
        }
    }
}