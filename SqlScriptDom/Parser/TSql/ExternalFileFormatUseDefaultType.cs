//------------------------------------------------------------------------------
// <copyright file="ExternalFileFormatUseDefaultType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The enumeration specifies the external file format use default type option
    /// FALSE (default) or TRUE.
    /// </summary>
    public enum ExternalFileFormatUseDefaultType
    {
        /// <summary>
        /// Treat missing values as NULL.  This is the default.
        /// </summary>
        False = 0,

        /// <summary>
        /// Treat missing values using the default type.
        /// </summary>
        True = 1
    }

#pragma warning restore 1591
}
