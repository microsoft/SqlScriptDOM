//------------------------------------------------------------------------------
// <copyright file="GlobalConstants.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    /// <summary>
    /// These are the globals that define the relative paths.
    /// </summary>
    public static class GlobalConstants
    {
        public const string TSqlNameSpace = @"SqlStudio.Tests.UTSqlScriptDom";
        /// <summary>
        /// Path to the TSql scripts that tests execute.
        /// </summary>
        public const string TestScriptsNameSpace = TSqlNameSpace + @".TestScripts";
        /// <summary>
        /// Path to the TSql scripts that the output of parser should match.
        /// </summary>
        public const string BaselinePrettyPrintingNameSpace = TSqlNameSpace + @".BaselinePrettyPrintingFiles";
        /// <summary>
        /// Path to the TSql scripts that the output of parser should match.
        /// </summary>
        public const string PhaseOneTestScriptsFilesNameSpace = TSqlNameSpace + @".PhaseOneTestScripts";

        public const int DefaultTestTimeout = 1000 * 60 * 10;
    }
}
