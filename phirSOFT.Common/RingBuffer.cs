// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="Buffer.cs">
// Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// <summary>
// phirSOFT Package phirSOFT.Common
// 
// Created by:    Philemon Eichin
// Created:       01.10.2016 18:30
// Last Modified: 01.10.2016 19:26
// </summary>
//  
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using JetBrains.Annotations;

namespace phirSOFT.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     Provides a Ringbuffer with a fixed size.
    /// </summary>
    /// <typeparam name="T">The items to store</typeparam>
    [PublicAPI]
    public class RingBuffer<T> : ICollection<T>
    {
        private T[] _items;
        private long _positon;
        private RemoveStrategies.RemoveStrategy _removeStrategy;


        /// <summary>
        ///     Creates a new instance of the <see cref="RingBuffer{T}" /> class.
        /// </summary>
        /// <param name="length">The amount of items this this buffer can store.</param>
        public RingBuffer(long length)
        {
            IsReadOnly = false;
            Capacity = length;
            Clear();
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="RingBuffer{T}" /> class and filles it up with <paramref name="value" />.
        /// </summary>
        /// <param name="length">The amount of items this this buffer can store.</param>
        /// <param name="value">The values to fill into the buffer.</param>
        public RingBuffer(long length, IEnumerable<T> value) : this(length)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            foreach (var item in value)
                Add(item);
        }


        /// <summary>
        ///     Gets the capacity of items that can be stored in the buffer.
        /// </summary>
        public long Capacity { get; }


        /// <summary>
        ///     Gets or sets the strategy for removing an item. This delegate is called by <see cref="Remove(T)"/>
        /// </summary>
        /// <remarks>
        ///     This property is never <see langword="null" />. If set to <see langword="null" /> the default strategy is restored.
        ///     The default strategy removes the item without filling up the gap.
        /// </remarks>
        /// <seealso cref="RemoveStrategies"/>
        /// <seealso cref="RemoveStrategies.RemoveStrategy"/>
        [NotNull]
        public RemoveStrategies.RemoveStrategy RemoveStrategy
        {
            get { return _removeStrategy ?? RemoveStrategies.DefaultStrategy; }
            set { _removeStrategy = value; }
        }


        /// <inheritdoc />
        public void Add(T obj)
        {
            _items[_positon] = obj;
            _positon = (_positon + 1) % Capacity;
        }

        /// <inheritdoc />
        public void Clear()
        {
            _items = new T[Capacity];
            _positon = 0;
        }

        /// <inheritdoc />
        public bool Contains(T item) => _items.Contains(item);

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove(T item)
        {
            return RemoveStrategy(_items, item, _positon);
        }

        /// <inheritdoc />
        public int Count => (int)Capacity;

        /// <inheritdoc />
        public bool IsReadOnly { get; }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return new RingEnumerator(this);
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class RingEnumerator : IEnumerator<T>
        {
            private readonly RingBuffer<T> _ringBuffer;
            private readonly long _zero;
            private long _position;

            public RingEnumerator(RingBuffer<T> ringBuffer)
            {
                _ringBuffer = ringBuffer;
                _zero = ringBuffer._positon;
                _position = 0;
            }


            /// <inheritdoc />
            public void Dispose()
            {
            }

            /// <inheritdoc />
            public bool MoveNext()
            {
                if (_position == _ringBuffer.Capacity)
                    return false;

                _position++;
                return true;
            }

            /// <inheritdoc />
            public void Reset()
            {
                _position = 0;
            }

            /// <inheritdoc />
            public T Current => _ringBuffer._items[(_position + _zero) % _ringBuffer.Capacity];

            /// <inheritdoc />
            object IEnumerator.Current => Current;
        }

        /// <summary>
        /// Contains some strategies how to remove items from an <see cref="RingBuffer{T}"/>
        /// </summary>
        public static class RemoveStrategies
        {
            /// <summary>
            /// Defines the delegate type for removing an item from an <see cref="RingBuffer{T}"/>
            /// </summary>
            /// <param name="data">An array representing the <see cref="RingBuffer{T}"/>.</param>
            /// <param name="item">The item to remove from the buffer.</param>
            /// <param name="start">The index of the item, where to start to search for the item.</param>
            /// <inheritdoc cref="ICollection{T}.Remove(T)" select="return"/>
            public delegate bool RemoveStrategy(T[] data, T item, long start);

            /// <summary>
            /// Defines the default strategy, which sets the item to the default value.
            /// </summary>
            /// <param name="array">An array representing the <see cref="RingBuffer{T}"/>.</param>
            /// <param name="item">The item to remove from the buffer.</param>
            /// <param name="start">The index of the item, where to start to search for the item.</param>
            /// <inheritdoc cref="ICollection{T}.Remove(T)" select="return"/>
            /// <remarks>
            /// This strategy does not close the gap resulting in the deletion. For example:
            /// <para>
            /// Assumming you have the following buffer [A, B, C, D, E, F, 0, 0, 0] and you're going to remove B this would result in
            /// [A, 0, C, D, E, F, 0, 0, 0]
            /// </para>
            /// </remarks>
            public static bool DefaultStrategy(T[] array, T item, long start)
            {
                if (item.Equals(default(T)))
                    return false;

                for (long i = 0; i < array.LongLength; i++)
                    if (!array[(i + start) % array.LongLength].Equals(item))
                    {
                        array[(i + start) % array.LongLength] = default(T);
                        return true;
                    }


                return false;
            }

            /// <summary>
            ///  Defines the a strategy, which sets the item to the default value and closes the gap.
            /// </summary>
            /// <param name="array">An array representing the <see cref="RingBuffer{T}"/>.</param>
            /// <param name="item">The item to remove from the buffer.</param>
            /// <param name="start">The index of the item, where to start to search for the item.</param>
            /// <inheritdoc cref="ICollection{T}.Remove(T)" select="return"/>
            /// <remarks>
            /// This strategy does close the gap resulting in the deletion. For example:
            /// <para>
            /// Assumming you have the following buffer [A, B, C, D, E, F, 0, 0, 0] and you're going to remove B this would result in
            /// [A, C, D, E, F, 0, 0, 0, 0]
            /// </para>
            /// </remarks>
            public static bool ShiftStrategy(T[] array, T item, long start)
            {
                var shift = false;
                for (long i = 0; i < array.LongLength; i++)
                    if (!shift)
                    {
                        if (!array[(i + start) % array.LongLength].Equals(item)) continue;
                        array[(i + start) % array.LongLength] = array[(i + start + 1) % array.LongLength];
                        shift = true;
                    }
                    else if (i + 1 < array.LongLength)
                        array[(i + start) % array.LongLength] = array[(i + start + 1) % array.LongLength];
                return shift;
            }
        }
    }
}