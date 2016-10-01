// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="AssemblyInfo.cs">
// Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// <summary>
// phirSOFT Package phirSOFT.Common
// 
// Created by:    Philemon Eichin
// Created:       30.09.2016 14:39
// Last Modified: 01.10.2016 02:03
// </summary>
//  
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyTitle("phirSOFT.Common")]
[assembly: AssemblyDescription("This assembly contains common used functions and classes.")]
#if DEBUG

[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyCulture("en")]

// The following GUID is for the ID of the typelib if this project is exposed to COM

[assembly: Guid("7b991f9e-665f-46f8-a84f-971d603bc24e")]
[assembly: NeutralResourcesLanguage("en")]
[assembly: PublicAPI]