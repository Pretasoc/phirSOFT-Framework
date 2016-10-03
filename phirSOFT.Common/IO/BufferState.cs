// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="BufferState.cs">
// Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// <summary>
// phirSOFT Package phirSOFT.Common
// 
// Created by:    Philemon Eichin
// Created:       03.10.2016 13:57
// Last Modified: 03.10.2016 14:41
// </summary>
//  
// --------------------------------------------------------------------------------------------------------------------

using JetBrains.Annotations;

namespace phirSOFT.Common.IO
{
    /// <summary>
    ///     Gets information about a buffer.
    /// </summary>
    [PublicAPI]
    public enum BufferState
    {
        /// <summary>
        ///     The current state is unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     The buffer is empty.
        /// </summary>
        Empty = 1,

        /// <summary>
        ///     There are items in the buffer.
        /// </summary>
        Filled = 2,

        /// <summary>
        ///     The buffer is filled.
        /// </summary>
        Full = 3
    }
}