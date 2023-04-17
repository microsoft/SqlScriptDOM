//------------------------------------------------------------------------------
// <copyright file="ExternalTableRejectType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The enumeration specifies the external table reject types
    /// VALUE (default) or PERCENTAGE.
    /// </summary>
    public enum ExternalTableRejectType
    {
        /// <summary>
        /// Value reject type format.  This is the default.
        /// </summary>
        Value = 0,

        /// <summary>
        /// Percentage reject type format.
        /// </summary>
        Percentage = 1
    }

#pragma warning restore 1591
}
