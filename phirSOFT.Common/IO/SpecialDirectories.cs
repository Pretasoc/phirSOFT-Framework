// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="SpecialDirectories.cs">
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

using static System.Environment;

namespace phirSOFT.Common.IO
{
    using JetBrains.Annotations;

    /// <summary>
    ///     Provides paths to special Directories
    /// </summary>
    [PublicAPI]
    public static class SpecialDirectories
    {
        /// <inheritdoc cref="SpecialFolder.AdminTools" />
        public static string AdminTool => GetFolderPath(SpecialFolder.AdminTools);

        /// <inheritdoc cref="SpecialFolder.ApplicationData" />
        public static string ApplicationData => GetFolderPath(SpecialFolder.ApplicationData);

        /// <inheritdoc cref="SpecialFolder.CDBurning" />
        // ReSharper disable once InconsistentNaming
        public static string CDBurning => GetFolderPath(SpecialFolder.CDBurning);

        /// <inheritdoc cref="SpecialFolder.CommonAdminTools" />
        public static string CommonAdminTools => GetFolderPath(SpecialFolder.CommonAdminTools);

        /// <inheritdoc cref="SpecialFolder.CommonApplicationData" />
        public static string CommonApplicationData => GetFolderPath(SpecialFolder.CommonApplicationData);

        /// <inheritdoc cref="SpecialFolder.CommonDesktopDirectory" />
        public static string CommonDesktopDirectory => GetFolderPath(SpecialFolder.CommonDesktopDirectory);

        /// <inheritdoc cref="SpecialFolder.CommonDocuments" />
        public static string CommonDocuments => GetFolderPath(SpecialFolder.CommonDocuments);

        /// <inheritdoc cref="SpecialFolder.CommonMusic" />
        public static string CommonMusic => GetFolderPath(SpecialFolder.CommonMusic);

        /// <inheritdoc cref="SpecialFolder.CommonOemLinks" />
        public static string CommonOemLinks => GetFolderPath(SpecialFolder.CommonOemLinks);

        /// <inheritdoc cref="SpecialFolder.CommonPictures" />
        public static string CommonPictures => GetFolderPath(SpecialFolder.CommonPictures);

        /// <inheritdoc cref="SpecialFolder.CommonProgramFiles" />
        public static string CommonProgramFiles => GetFolderPath(SpecialFolder.CommonProgramFiles);

        /// <inheritdoc cref="SpecialFolder.CommonProgramFilesX86" />
        public static string CommonProgramFilesX86 => GetFolderPath(SpecialFolder.CommonProgramFilesX86);

        /// <inheritdoc cref="SpecialFolder.CommonPrograms" />
        public static string CommonPrograms => GetFolderPath(SpecialFolder.CommonPrograms);

        /// <inheritdoc cref="SpecialFolder.CommonStartMenu" />
        public static string CommonStartMenu => GetFolderPath(SpecialFolder.CommonStartMenu);

        /// <inheritdoc cref="SpecialFolder.CommonStartup" />
        public static string CommonStartup => GetFolderPath(SpecialFolder.CommonStartup);

        /// <inheritdoc cref="SpecialFolder.CommonTemplates" />
        public static string CommonTemplates => GetFolderPath(SpecialFolder.CommonTemplates);

        /// <inheritdoc cref="SpecialFolder.CommonVideos" />
        public static string CommonVideos => GetFolderPath(SpecialFolder.CommonVideos);

        /// <inheritdoc cref="SpecialFolder.Cookies" />
        public static string Cookies => GetFolderPath(SpecialFolder.Cookies);

        /// <inheritdoc cref="SpecialFolder.Desktop" />
        public static string Desktop => GetFolderPath(SpecialFolder.Desktop);

        /// <inheritdoc cref="SpecialFolder.DesktopDirectory" />
        public static string DesktopDirectory => GetFolderPath(SpecialFolder.DesktopDirectory);

        /// <inheritdoc cref="SpecialFolder.Favorites" />
        public static string Favorites => GetFolderPath(SpecialFolder.Favorites);

        /// <inheritdoc cref="SpecialFolder.Fonts" />
        public static string Fonts => GetFolderPath(SpecialFolder.Fonts);

        /// <inheritdoc cref="SpecialFolder.History" />
        public static string History => GetFolderPath(SpecialFolder.History);

        /// <inheritdoc cref="SpecialFolder.InternetCache" />
        public static string InternetCache => GetFolderPath(SpecialFolder.InternetCache);

        /// <inheritdoc cref="SpecialFolder.LocalApplicationData" />
        public static string LocalApplicationData => GetFolderPath(SpecialFolder.LocalApplicationData);

        /// <inheritdoc cref="SpecialFolder.LocalizedResources" />
        public static string LocalizedResources => GetFolderPath(SpecialFolder.LocalizedResources);

        /// <inheritdoc cref="SpecialFolder.MyComputer" />
        public static string MyComputer => GetFolderPath(SpecialFolder.MyComputer);

        /// <inheritdoc cref="SpecialFolder.MyDocuments" />
        public static string MyDocuments => GetFolderPath(SpecialFolder.MyDocuments);

        /// <inheritdoc cref="SpecialFolder.MyMusic" />
        public static string MyMusic => GetFolderPath(SpecialFolder.MyMusic);

        /// <inheritdoc cref="SpecialFolder.MyPictures" />
        public static string MyPictures => GetFolderPath(SpecialFolder.MyPictures);

        /// <inheritdoc cref="SpecialFolder.MyVideos" />
        public static string MyVideos => GetFolderPath(SpecialFolder.MyVideos);

        /// <inheritdoc cref="SpecialFolder.NetworkShortcuts" />
        public static string NetworkShortcuts => GetFolderPath(SpecialFolder.NetworkShortcuts);

        /// <inheritdoc cref="SpecialFolder.Personal" />
        public static string Personal => GetFolderPath(SpecialFolder.Personal);

        /// <inheritdoc cref="SpecialFolder.PrinterShortcuts" />
        public static string PrinterShortcuts => GetFolderPath(SpecialFolder.PrinterShortcuts);

        /// <inheritdoc cref="SpecialFolder.ProgramFiles" />
        public static string ProgramFiles => GetFolderPath(SpecialFolder.ProgramFiles);

        /// <inheritdoc cref="SpecialFolder.ProgramFilesX86" />
        public static string ProgramFilesX86 => GetFolderPath(SpecialFolder.ProgramFilesX86);

        /// <inheritdoc cref="SpecialFolder.Programs" />
        public static string Programs => GetFolderPath(SpecialFolder.Programs);

        /// <inheritdoc cref="SpecialFolder.Recent" />
        public static string Recent => GetFolderPath(SpecialFolder.Recent);

        /// <inheritdoc cref="SpecialFolder.Resources" />
        public static string Resources => GetFolderPath(SpecialFolder.Resources);

        /// <inheritdoc cref="SpecialFolder.SendTo" />
        public static string SendTo => GetFolderPath(SpecialFolder.SendTo);

        /// <inheritdoc cref="SpecialFolder.StartMenu" />
        public static string StartMenu => GetFolderPath(SpecialFolder.StartMenu);

        /// <inheritdoc cref="SpecialFolder.System" />
        public static string System => GetFolderPath(SpecialFolder.System);

        /// <inheritdoc cref="SpecialFolder.SystemX86" />
        public static string SystemX86 => GetFolderPath(SpecialFolder.SystemX86);

        /// <inheritdoc cref="SpecialFolder.Templates" />
        public static string Templates => GetFolderPath(SpecialFolder.Templates);

        /// <inheritdoc cref="SpecialFolder.UserProfile" />
        public static string UserProfile => GetFolderPath(SpecialFolder.UserProfile);

        /// <inheritdoc cref="SpecialFolder.Windows" />
        public static string Windows => GetFolderPath(SpecialFolder.Windows);
    }
}