﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="ItemPositions.cs">
// Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// <summary>
// phirSOFT Package phirSOFT.Common
// Created by:    Philemon Eichin
// Created:       01.10.2016 14:41
// Last Modified: 01.10.2016 16:12
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using JetBrains.Annotations;

namespace phirSOFT.Common
{
    /// <summary>
    ///     Represents a position of an item in an array.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum ItemPositions
    {
        /// <summary>
        ///     The item is in the Middle
        /// </summary>
        Middle = 4,

        /// <summary>
        ///     The item is the First item.
        /// </summary>
        First = 1,

        /// <summary>
        ///     The item is the Last item.
        /// </summary>
        Last = 2,


        /// <summary>
        ///     Default value.
        /// </summary>
        None = 0,

        /// <summary>
        ///     The item is the first or the last item.
        /// </summary>
        Border = 3
    }
}