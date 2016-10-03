// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="TextTools.cs">
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
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace phirSOFT.Common
{
    /// <summary>
    ///     Provides tools for strings and text display.
    /// </summary>
    [PublicAPI]
    public static class TextTools
    {
        /// <summary>
        ///     Provides a table that translates <see cref="HorizontalAlignment" /> to <see cref="TextFormatFlags" />.
        /// </summary>
        public static MapTable<HorizontalAlignment, TextFormatFlags> TextFormatTable { get; }

        static TextTools()
        {
            StringAlignmentTable = new MapTable<HorizontalAlignment, StringAlignment>
            {
                [HorizontalAlignment.Left] = StringAlignment.Near,
                [HorizontalAlignment.Right] = StringAlignment.Far,
                [HorizontalAlignment.Center] = StringAlignment.Center
            };

            TextFormatTable = new MapTable<HorizontalAlignment, TextFormatFlags>
            {
                [HorizontalAlignment.Center] = TextFormatFlags.HorizontalCenter,
                [HorizontalAlignment.Left] = TextFormatFlags.Left,
                [HorizontalAlignment.Right] = TextFormatFlags.Right
            };
        }

        /// <summary>
        ///     Provides a alignment Table for translate.
        /// </summary>
        public static MapTable<HorizontalAlignment, StringAlignment> StringAlignmentTable { get; }

        /// <summary>
        ///     Translates a <see cref="HorizontalAlignment" /> to the according <see cref="StringAlignment" />.
        /// </summary>
        /// <param name="alignment">The <see cref="HorizontalAlignment" /> to translate.</param>
        /// <returns>A <see cref="StringAlignment" /> that represents the translation of <paramref name="alignment" />.</returns>
        public static StringAlignment TranslateAlignment(HorizontalAlignment alignment)
        {
            switch (alignment)
            {
                case HorizontalAlignment.Left:
                    return StringAlignment.Near;

                case HorizontalAlignment.Right:
                    return StringAlignment.Far;

                case HorizontalAlignment.Center:
                    return StringAlignment.Center;

                default:
                    return default(StringAlignment);
            }
        }

        /// <summary>
        ///     Translates a <see cref="HorizontalAlignment" /> to the according <see cref="TextFormatFlags" />.
        /// </summary>
        /// <param name="alignment">The <see cref="HorizontalAlignment" /> to translate.</param>
        /// <returns>A <see cref="TextFormatFlags" /> that represents the translation of <paramref name="alignment" />.</returns>
        public static TextFormatFlags TranslateAlignmentToTextFormatFlags(HorizontalAlignment alignment)
        {
            switch (alignment)
            {
                case HorizontalAlignment.Left:
                    return TextFormatFlags.Left;

                case HorizontalAlignment.Right:
                    return TextFormatFlags.Right;

                case HorizontalAlignment.Center:
                    return TextFormatFlags.HorizontalCenter;

                default:
                    return default(TextFormatFlags);
            }
        }

        /// <summary>
        ///     Translates a <see cref="StringTrimming" /> to the according <see cref="TextFormatFlags" />.
        /// </summary>
        /// <param name="trimming">The <see cref="StringTrimming" /> to translate.</param>
        /// <returns>A <see cref="TextFormatFlags" /> that represents the translation of <paramref name="trimming" />.</returns>
        public static TextFormatFlags TranslateTrimmingToTextFormatFlag(this StringTrimming trimming)
        {
            switch (trimming)
            {
                case StringTrimming.EllipsisCharacter:
                    return TextFormatFlags.EndEllipsis;

                case StringTrimming.EllipsisPath:
                    return TextFormatFlags.PathEllipsis;

                case StringTrimming.EllipsisWord:
                    return TextFormatFlags.WordEllipsis;

                case StringTrimming.Word:
                    return TextFormatFlags.WordBreak;

                case StringTrimming.None:
                    return TextFormatFlags.Default;

                case StringTrimming.Character:
                    return TextFormatFlags.Default;

                default:
                    return default(TextFormatFlags);
            }
        }

        /// <summary>
        ///     Creates a <see cref="Regex" /> for an file filter.
        /// </summary>
        /// <param name="filter">The filter string to create a <see cref="Regex" /> for.</param>
        /// <returns>A <see cref="Regex" /> that matches <paramref name="filter" />.</returns>
        /// <remarks>The format of the filter string is the same as in an FileDialog.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="filter" /> is <see langword="null" /> or empty.</exception>
        public static Regex FilterToRegularExpression(string filter)
        {
            if (string.IsNullOrEmpty(filter))
                throw new ArgumentNullException(nameof(filter));
            var filters = filter.Split('|');

            // TODO: Use a stringbuilder here.
            var reg = "(";
            for (var i = 1; i < filters.Length; i += 2)
            {
                var filtersParts = filters[i].Split(';');

                reg =
                    filtersParts.Select(
                            res =>
                                "\\+?|{[()^$.#".Aggregate(res,
                                    (current, x) => current.Replace(x.ToString(CultureInfo.InvariantCulture), @"\" + x)))
                        .Select(
                            re =>
                                ((FormattableString) $"^{re.Replace("*", ".*")}$").ToString(
                                    CultureInfo.InvariantCulture))
                        .Aggregate(reg, (current1, re) => current1 + re + "|");
            }

            reg = reg.Remove(reg.Length - 1) + ")";
            return new Regex(reg, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        /// <summary>
        ///     Determinates the relative path between two absolute paths.
        /// </summary>
        /// <param name="sourcePath">The path used as source.</param>
        /// <param name="targetPath">The path uses as Target</param>
        /// <returns>A string representing the relative path from <paramref name="sourcePath" /> to <paramref name="targetPath" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="sourcePath" /> or <paramref name="targetPath" /> is
        ///     <see langword="null" />.
        /// </exception>
        /// <exception cref="UriFormatException">
        ///     <paramref name="sourcePath" /> or <paramref name="targetPath" /> is a malformed
        ///     Uri.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     This instance represents a relative URI, and this property is valid only
        ///     for absolute URIs.
        /// </exception>
        [NotNull]
        public static string GetRelativePath([NotNull] string sourcePath, [NotNull] string targetPath)
        {
            if (string.IsNullOrEmpty(sourcePath)) throw new ArgumentNullException(nameof(sourcePath));
            if (string.IsNullOrEmpty(targetPath)) throw new ArgumentNullException(nameof(targetPath));

            var fromUri = new Uri(sourcePath);
            var toUri = new Uri(targetPath);
            return fromUri.MakeRelativeUri(toUri).ToString();
        }

        /// <summary>
        ///     Formats an <see cref="Array" /> as <see cref="string" />.
        /// </summary>
        /// <typeparam name="T">The type of the array items.</typeparam>
        /// <param name="values">The Array to format.</param>
        /// <returns>An string representing the array using the default formatter. {item1, ..., itemN}</returns>
        public static string PrintArray<T>(this T[] values)
        {
            return values.PrintArray((item, position) =>
            {
                var s = string.Empty;

                if (position.HasFlag(ItemPositions.First))
                    s += "{";
                s += item.ToString();
                if (position.HasFlag(ItemPositions.Last))
                    s += "}";
                else
                    s += ", ";
                return s;
            });
        }

        /// <summary>
        ///     Formats an <see cref="Array" /> as string using a given formatter.
        /// </summary>
        /// <typeparam name="T">The type of the array items.</typeparam>
        /// <param name="values">The Array to format.</param>
        /// <param name="formatter">A Formatter to format an item to string.</param>
        /// <returns>An string representing the array using <paramref name="formatter" />.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static string PrintArray<T>(this T[] values, Func<T, string> formatter)
        {
            return values.PrintArray((item, position) => formatter(item));
        }

        /// <summary>
        ///     Formats an <see cref="Array" /> as string using a given formatter.
        /// </summary>
        /// <typeparam name="T">The type of the array items.</typeparam>
        /// <param name="values">The Array to format.</param>
        /// <param name="formatter">A Formatter to format an item to string.</param>
        /// <returns>An string representing the array using <paramref name="formatter" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="values" /> or <paramref name="formatter" /> is
        ///     <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="values" /> contains no items.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static string PrintArray<T>([NotNull] this T[] values, [NotNull] Func<T, ItemPositions, string> formatter)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            if (formatter == null)
                throw new ArgumentNullException(nameof(formatter));
            if (values.Length == 0)
                throw new ArgumentException(SR.ex_ValueNoEmptyCollection, nameof(values));

            var sb = new StringBuilder();
            Func<T, T[], ItemPositions> getPos = (item, items) =>
            {
                var p = ItemPositions.Middle;
                if (item.Equals(items.First()))
                    p |= ItemPositions.First;
                if (item.Equals(items.Last()))
                    p |= ItemPositions.Last;
                return p;
            };

            foreach (var item in values)
                sb.Append(formatter(item, getPos(item, values)));
            return sb.ToString();
        }

        /// <summary>
        ///     Prints an array of byte as an number.
        /// </summary>
        /// <param name="values">The numbers to be printed.</param>
        /// <returns>A String containing the formatted array.</returns>
        [NotNull]
        public static string PrintAsNumber(this byte[] values)
        {
            try
            {
                // The only Exception the delegate can throw is an format Exception,
                // but this will not happen, because the format is valid.
                return values.PrintArray(pos => pos.ToString("X", CultureInfo.InvariantCulture));
            }

            // If we catch a more specific exception we get still a warning we cant turn off properly.
            // In fact, there wont be any Exception thrown here.
            // ReSharper disable once CatchAllClause
            catch
            {
                Debug.WriteLine("How could this happen?");

                // Because this never should happen
                // ReSharper disable once AssignNullToNotNullAttribute
                return null;
            }
        }
    }
}