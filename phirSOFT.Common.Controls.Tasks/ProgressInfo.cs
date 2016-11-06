// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="ProgressInfo.cs">
// Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// <summary>
// phirSOFT Package phirSOFT.Common.Controls.Tasks
// 
// Created by:    Philemon Eichin
// Created:       05.11.2016 13:52
// Last Modified: 05.11.2016 13:52
// </summary>
//  
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace phirSOFT.Common.Controls.Tasks
{
    public class ProgressInfo
    {
        private double _percentComplete;

        /// <summary>
        ///     Gets or sets the
        /// </summary>
        public double PercentComplete
        {
            get { return _percentComplete; }
            set
            {
                if ((value < 0) || (value > 1))
                    throw new ArgumentException("The value has to be within [0, 1].", nameof(value));

                _percentComplete = value;
            }
        }

        public ProgressState State { get; set; }
    }

    public enum ProgressState
    {
        Unknown,
        Pending,
        Progressing,
        Paused,
        Finished,
        Error
    }
}