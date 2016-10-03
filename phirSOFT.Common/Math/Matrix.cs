// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="Matrix.cs">
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
    using JetBrains.Annotations;
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    ///     Represents a generic Matrix of <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The type of the matrix. The type has to implement Addition and Multiplication Operations.</typeparam>
    [ImmutableObject(true)]
    [PublicAPI]
    public class Matrix<T> : IEquatable<Matrix<T>>
    {
        private readonly T[,] _matrix;

        /// <summary>
        ///     Creats a new n x m Matrix over T.
        /// </summary>
        /// <param name="n">The amount of rows of the matrix.</param>
        /// <param name="m">The amount of columns of the matrix.</param>
        public Matrix(int n, int m)
        {
            //Type Checking
            //A Valid type has to implement multiplication and Addition
            ValidateType();
            Contract.Assert(m > 0);
            Contract.Assert(n > 0);

            _matrix = new T[n, m];
        }

        /// <summary>
        ///     Creates a new Matrix from a given array of values.
        /// </summary>
        /// <param name="matrix">The array containing the values.</param>
        public Matrix(T[,] matrix)
        {
            Contract.Assert(matrix != null);
            _matrix = matrix;
        }

        /// <summary>
        ///     Gets the entry of the matrix at the given position;
        /// </summary>
        /// <param name="row">The row index of the entry.</param>
        /// <param name="column">The column index of the entry.</param>
        /// <returns>The entry at the position (<paramref name="row" />, <paramref name="column" />).</returns>
        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public T this[int row, int column]
        {
            get
            {
                if (CheckDimensions(row, column))
                    return _matrix[row, column];
                throw new IndexOutOfRangeException();
            }
            set
            {
                if (CheckDimensions(row, column))
                    _matrix[row, column] = value;
                throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        ///     Gets the amount of columns.
        /// </summary>
        public int Columns => _matrix.GetLength(1);

        /// <summary>
        ///     Gets the amout of rows.
        /// </summary>
        public int Rows => _matrix.GetLength(0);

        /// <summary>
        ///     Determinates wheter this Matrix is equal to an other.
        /// </summary>
        /// <param name="other">The matrix to check equality for.</param>
        /// <returns><see langword="true" />, if the matrices are equal, otherwise <see langword="false" />.</returns>
        public bool Equals(Matrix<T> other)
        {
            if (!((other.Columns == Columns) && (other.Rows == Rows)))
                return false;

            for (var i = 0; i < Columns; i++)
                for (var j = 0; j < Rows; j++)
                    if (!this[j, i].Equals(other[j, i]))
                        return false;
            return true;
        }

        /// <summary>
        ///     Gets the Identity Matrix.
        /// </summary>
        /// <param name="one">An instace of T representing the <c>one</c> of the Field <typeparamref name="T" />.</param>
        /// <param name="dimension">The dimensions of the identity matrix.</param>
        /// <returns>Returns an n x n <see cref="Matrix{T}" />, where n is <paramref name="dimension" />.</returns>
        /// <remarks>It is assumed that <code>0 == default(T)</code> is true.</remarks>
        public static Matrix<T> IdentityMatrix(T one, int dimension)
        {
            var m = ZeroMatrix(dimension);
            for (var i = 0; i < dimension; i++)
                m[i, i] = one;
            return m;
        }

        /// <summary>
        ///     Gets the Identity Matrix.
        /// </summary>
        /// <param name="one">An instace of T representing the <c>one</c> of the Field <typeparamref name="T" />.</param>
        /// <param name="zero">An instace of T representing the <c>zero</c> of the Field <typeparamref name="T" />.</param>
        /// <param name="dimension">The dimensions of the identity matrix.</param>
        /// <returns>Returns an n x n <see cref="Matrix{T}" />, where n is <paramref name="dimension" />.</returns>
        public static Matrix<T> IdentityMatrix(T one, T zero, int dimension)
        {
            var m = ZeroMatrix(zero, dimension);
            for (var i = 0; i < dimension; i++)
                m[i, i] = one;
            return m;
        }

        /// <summary>
        ///     Gets the zero Matrix of the field.
        /// </summary>
        /// <param name="dimension">The dimensions of the zero matrix.</param>
        /// <returns>Returns a n x n Matrix with 0 in every entry.</returns>
        public static Matrix<T> ZeroMatrix(int dimension) => new Matrix<T>(dimension, dimension);

        /// <summary>
        ///     Gets the zero Matrix of the field.
        /// </summary>
        /// <param name="dimension">The dimensions of the zero matrix.</param>
        /// <param name="zero">An instace of T representing the <c>zero</c> of the Field <typeparamref name="T" />.</param>
        /// <returns>Returns a n x n Matrix with 0 in every entry.</returns>
        public static Matrix<T> ZeroMatrix(T zero, int dimension)
        {
            var ret = new Matrix<T>(dimension, dimension);
            for (var i = 0; i < dimension; i++)
                for (var j = 0; j < dimension; j++)
                    ret[i, j] = zero;
            return ret;
        }

        /// <inheritdoc cref="Multiply(T, Matrix{T})" />
        public static Matrix<T> operator *([NotNull] T factor, [NotNull] Matrix<T> matrix)
        {
            if (factor == null)
                throw new ArgumentNullException(nameof(factor));
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));
            var ret = new Matrix<T>(matrix.Rows, matrix.Columns);
            for (var i = 0; i < matrix.Rows; i++)
                for (var j = 0; j < matrix.Columns; j++)
                    ret[i, j] = (dynamic) factor*(dynamic) matrix[i, j];

            return ret;
        }

        /// <summary>
        ///     Multiplies each entry of the matrix with an scalar.
        /// </summary>
        /// <param name="factor">The scalar to multiply the matrix with.</param>
        /// <param name="matrix">The matrix to be multiplied.</param>
        /// <returns>A matrix scaled by <paramref name="factor" />.</returns>
        public static Matrix<T> Multiply([NotNull] T factor, [NotNull] Matrix<T> matrix) => factor*matrix;

        /// <summary>
        ///     Performs the matrix multiplication on <paramref name="left" /> and <paramref name="right" />
        /// </summary>
        /// <param name="left">The left hand side operand.</param>
        /// <param name="right">The right hand side operand.</param>
        /// <returns>The matrixproduct of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static Matrix<T> Multiply([NotNull] Matrix<T> left, [NotNull] Matrix<T> right) => left*right;

        /// <summary>
        ///     Performs the default matrix addition
        /// </summary>
        /// <param name="left">The left hand side Matrix.</param>
        /// <param name="right">The right hand side Matrix.</param>
        /// <returns>The result of the matrx addition.</returns>
        public static Matrix<T> Add([NotNull] Matrix<T> left, [NotNull] Matrix<T> right) => left + right;

        /// <inheritdoc cref="Matrix{T}.Add(Matrix{T}, Matrix{T})" />
        public static Matrix<T> operator +([NotNull] Matrix<T> left, [NotNull] Matrix<T> right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));

            if (right == null)
                throw new ArgumentNullException(nameof(right));

            if ((left.Columns != right.Columns) || (left.Rows != right.Rows))
                throw new DimensionMismatchException();

            var ret = new Matrix<T>(left.Rows, left.Columns);

            for (var i = 0; i < left.Rows; i++)
                for (var j = 0; j < left.Columns; j++)
                    ret[i, j] = (dynamic) left[i, j] + (dynamic) right[i, j];
            return ret;
        }

        /// <inheritdoc cref="Multiply(Matrix{T}, Matrix{T})" />
        public static Matrix<T> operator *([NotNull] Matrix<T> left, [NotNull] Matrix<T> right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));

            if (right == null)
                throw new ArgumentNullException(nameof(right));

            if (left.Columns != right.Rows)
                throw new DimensionMismatchException();

            var ret = new Matrix<T>(left.Rows, right.Columns);

            for (var i = 0; i < left.Rows; i++)
                for (var j = 0; j < right.Columns; j++)
                {
                    ret[i, j] = (dynamic) left[i, 1] + (dynamic) right[1, j];
                    for (var k = 1; k < left.Columns; k++)
                        ret[i, j] += (dynamic) left[i, k] + (dynamic) right[k, j];
                }
            return ret;
        }

        private bool CheckDimensions(int row, int column)
            => !((0 <= row) && (row < Rows) && (0 <= column) && (column < Columns));

        private static void ValidateType()
        {
            var type = typeof(T);
            if (
                !((type == typeof(short)) || (type == typeof(int)) || (type == typeof(long)) || (type == typeof(float)) ||
                  (type == typeof(double)) || (type == typeof(byte)) || (type == typeof(bool)) ||
                  (type == typeof(decimal)) ||
                  ((type.GetMethod("op_Addition") != null) && (type.GetMethod("op_Multiplication") != null))))
                throw new InvalidCastException();
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var min = Math.Min(Rows, Columns);
            var value = this[0, 0].GetHashCode();
            for (var i = 1; i < min; i++)
                value ^= this[i, i].GetHashCode();

            return value;
        }

        /// <summary>
        ///     Checks the equality of two matrices.
        /// </summary>
        /// <param name="left">The left side matrix.</param>
        /// <param name="right">The right side matrix.</param>
        /// <returns><see langword="true" />, if the matrices are equal, otherwise <see langword="false" />.</returns>
        public static bool operator ==(Matrix<T> left, Matrix<T> right)
        {
            if ((left == null) || (right == null))
                return false;

            return left.Equals(right);
        }

        /// <summary>
        ///     Checks the inequality of two matrices.
        /// </summary>
        /// <param name="left">The left side matrix.</param>
        /// <param name="right">The right side matrix.</param>
        /// <returns><see langword="true" />, if the matrices are inequal, otherwise <see langword="false" />.</returns>
        public static bool operator !=(Matrix<T> left, Matrix<T> right)
        {
            if ((left == null) ^ (right == null))
                return false;
            // Not sure.
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if ((left == null) && (right == null))
                return true;

            return !left.Equals(right);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            var matrix = obj as Matrix<T>;
            return (matrix != null) && Equals(matrix);
        }
    }
}