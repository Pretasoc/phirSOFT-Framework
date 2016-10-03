// phirSOFT Library-phirSOFT.Common
// 
// Created by:    Philemon Eichin
// Last Modified: 06.02.2016 17:35
// 
// File:ConsoleTools.cs

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using static phirSOFT.Strings.SR;
using static System.Console;
using JetBrains.Annotations;
using phirSOFT.Common.Math;

namespace phirSOFT.Common
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Provides functions for console Applications.
    /// </summary>
    [PublicAPI]
    public static class ConsoleTools
    {

        private const char CursorChar = '_';

        /// <summary>
        /// Promps an application logo generated from 
        /// </summary>
        /// <devdoc>
        /// Cannot create a test for this method, because the entry assembly is not accessable.
        /// </devdoc>
        [ExcludeFromCodeCoverage]
        public static void PromptLogo()
        {
            var assembly = Assembly.GetEntryAssembly();

            WriteLine(LogoLine1, assembly.GetCustomAttribute<AssemblyCompanyAttribute>().Company, assembly.GetCustomAttribute<AssemblyTitleAttribute>().Title, assembly.GetCustomAttribute<AssemblyVersionAttribute>().Version);
            WriteLine(assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright);
        }


        /// <summary>
        /// Waits until the user presses any key.
        /// </summary>
        /// <param name="message">A message to display to the user until a key is pressed.</param>
        /// <exception cref="System.IO.IOException" />
        [ExcludeFromCodeCoverage]
        public static void WaitForAnyKey(string message)
        {

            WriteInNewLine(message);
            ReadKey(true);
        }

        /// <summary>
        /// Shows a default message and waits until the user presses a key.
        /// </summary>
        /// <exception cref="System.IO.IOException" />
        [ExcludeFromCodeCoverage]
        public static void WaitForAnyKey()
        {
            WriteInNewLine(pr_PressAnyKeyToContinue);
            ReadKey();
        }

        /// <summary>
        /// Waits until the user presses a specific key.
        /// </summary>
        /// <param name="prompt">A message prompted to the user.</param>
        /// <param name="showKeys">If true the user will get a list of the available keys.</param>
        /// <param name="keys">An array containing all valid keys.</param>
        /// <returns>The index of the key the user pressed</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="keys" /> is <see langword="null" /> or empty.</exception>
        /// <exception cref="System.IO.IOException" />
        /// <exception cref="System.Security.SecurityException" />
        [ExcludeFromCodeCoverage]
        public static int WaitForSpecificKeys(string prompt, bool showKeys, params ConsoleKey[] keys)
        {
            WriteInNewLine(BuildPrompt(prompt, showKeys, keys));
            if ((keys == null) || (keys.Length == -1))
                throw new ArgumentNullException(nameof(keys));

            while (true)
            {
                Write(CursorChar);
                CursorLeft -= 1;
                var c = ReadKey(true);

                for (var i = 0; i < keys.Length; i++)
                {
                    if (keys[i] != c.Key) continue;
                    return i;
                }
                WriteInNewLine(BuildPrompt(prompt, showKeys, keys));
            }
        }

        /// <summary>
        /// Ensures the cursor is at the beginning of a line an writes a text.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <exception cref="System.IO.IOException" />
        [ExcludeFromCodeCoverage]
        public static void WriteInNewLine(string text)
        {
            if (CursorLeft != 0)
                WriteLine();
            Write(text);
        }

        /// <summary>
        /// Ensures the cursor is at the beginning of a line an writes a text and a line terminator.
        /// </summary>
        /// <param name="text">The text to write.</param>
        [ExcludeFromCodeCoverage]
        public static void WriteInNewLineWithBr(string text)
        {
            if (CursorLeft != 0)
                WriteLine();
            WriteLine(text);
        }

        /// <summary>
        /// Asks the user to enter a number (interger).
        /// </summary>
        /// <param name="prompt">The message promted to user.</param>
        /// <returns>The number the user entered.</returns>
        /// <exception cref="System.IO.IOException" />
        /// <exception cref="OutOfMemoryException" />
        /// <exception cref="System.Security.SecurityException" />
        [ExcludeFromCodeCoverage]
        public static int GetNumber(string prompt)
        {
            WriteInNewLine(prompt);
            var isError = true;
            var inputI = 0;

            while (isError)
            {
                var inputS = ReadLine();

                try
                {
                    isError = false;
                    inputI = Convert.ToInt32(inputS, CultureInfo.CurrentCulture);
                }
                catch (InvalidCastException)
                {
                    WriteError(pr_EnterNumber);

                    isError = true;
                }
            }
            return inputI;
        }

        /// <summary>
        /// Asks the user to enter a number within a specific range (interger).
        /// </summary>
        /// <param name="prompt">The message promted to user.</param>
        /// <param name="lower">The lowest allowed number.</param>
        /// <param name="upper">The higthes alloed number.</param>
        /// <returns>The number the user entered.</returns>
        /// <exception cref="OutOfMemoryException" />
        /// <exception cref="System.IO.IOException" />
        /// <exception cref="System.Security.SecurityException" />
        [ExcludeFromCodeCoverage]
        public static int GetNumber(string prompt, int lower, int upper)
        {
            WriteInNewLine(prompt);
            var isError = true;
            var inputI = 0;
            while (isError)
            {
                var inputS = ReadLine();

                try
                {
                    isError = false;
                    inputI = Convert.ToInt32(inputS, CultureInfo.CurrentCulture);

                    if ((inputI < lower) | (inputI > upper))
                    {
                        WriteError(string.Format(CultureInfo.CurrentCulture, msg_OutOfRange, inputI, lower, upper));
                        isError = true;
                    }
                }
                catch (InvalidCastException)
                {
                    WriteError(pr_EnterNumber);
                    isError = true;
                }
            }

            return inputI;
        }


        /// <summary>
        /// Asks the user to enter a number within a specific range (interger).
        /// </summary>
        /// <param name="prompt">The message promted to user.</param>
        /// <param name="range">The allowed range.</param>
        /// <returns>The number the user entered.</returns>
        /// <exception cref="OutOfMemoryException" />
        /// <exception cref="System.IO.IOException" />
        /// <exception cref="System.Security.SecurityException" />
        [ExcludeFromCodeCoverage]
        public static int GetNumber(string prompt, Range<int> range) => GetNumber(prompt, range.Minimum, range.Maximum);

        private static string BuildPrompt(string prompt, bool showKeys, params ConsoleKey[] keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));

            var p = prompt ?? "";
            if (!showKeys) return p;
            p += "(";
            foreach (var key in keys)
            {
                p += key.ToString();
                if (key != keys[keys.LongLength - 1])
                    p += " | ";
            }
            p += ")";
            return p;
        }

        /// <summary>
        /// Writes an error to the console in red color.
        /// </summary>
        /// <param name="value">The text of the message.</param>
        /// <exception cref="System.Security.SecurityException" />
        /// <exception cref="System.IO.IOException" />
        [ExcludeFromCodeCoverage]
        public static void WriteError(string value)
        {
            WriteLine();
            WriteLineColor(value, ConsoleColor.DarkRed);
        }

        /// <summary>
        /// Prompts the user for a respose. (Committed with return)
        /// </summary>
        /// <param name="prompt">The promt to show.</param>
        /// <returns>The text the user entered.</returns>
        /// <exception cref="OutOfMemoryException" />
        /// <exception cref="System.IO.IOException" />
        [ExcludeFromCodeCoverage]
        public static string Prompt(string prompt)
        {
            Write(prompt);
            return ReadLine();
        }

        /// <summary>
        /// Moves the cursor to the specific postion and writes a prompt.
        /// </summary>
        /// <param name="positionX">The x-coordinate of the first char.</param>
        /// <param name="positionY">The y-coordinate of the first char.</param>
        /// <param name="promt">The message to display</param>
        /// <exception cref="System.IO.IOException" />
        /// <exception cref="System.Security.SecurityException" />
        [ExcludeFromCodeCoverage]
        public static void Output(int positionX, int positionY, object promt)
        {
            CursorLeft = positionX;
            CursorTop = positionY;
            Write(promt);
        }

        /// <summary>
        /// Moves the cursor to the specific postion and writes a prompt in a specific color.
        /// </summary>
        /// <param name="positionX">The x-coordinate of the first char.</param>
        /// <param name="positionY">The y-coordinate of the first char.</param>
        /// <param name="promt">The message to display</param>
        /// <param name="color">The color to write the prompt.</param>
        /// <remarks>
        /// The color is not resetted after calling this function.
        /// </remarks>
        /// <exception cref="System.IO.IOException" />
        /// <exception cref="System.Security.SecurityException" />
        [ExcludeFromCodeCoverage]
        public static void Output(int positionX, int positionY, object promt, ConsoleColor color)
        {
            ForegroundColor = color;
            Output(positionX, positionY, promt);
        }

        /// <summary>
        /// Writes text in a specific color.
        /// </summary>
        /// <param name="promt">The text to display</param>
        /// <param name="color">The color to apply to the text.</param>
        /// <remarks>The color is resetted after calling this function</remarks>
        /// <exception cref="System.IO.IOException" />
        /// <exception cref="System.Security.SecurityException" />
        [ExcludeFromCodeCoverage]
        public static void WriteColor(string promt, ConsoleColor color)
        {
            var c1 = ForegroundColor;
            ForegroundColor = color;
            Write(promt);
            ForegroundColor = c1;
        }

        /// <summary>
        /// Writes text and a line terminator in a specific color.
        /// </summary>
        /// <param name="promt">The text to display</param>
        /// <param name="color">The color to apply to the text.</param>
        /// <remarks>The color is resetted after calling this function</remarks>
        /// <exception cref="System.IO.IOException" />
        /// <exception cref="System.Security.SecurityException" />
        [ExcludeFromCodeCoverage]
        public static void WriteLineColor(string promt, ConsoleColor color)
        {
            var c1 = ForegroundColor;
            ForegroundColor = color;
            WriteLine(promt);
            ForegroundColor = c1;
        }
    }
}