//------------------------------------------------------------------------------
// <copyright file="OdbcLiteralType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// Represents the Odbc Literal Types
    /// </summary>
    public enum OdbcLiteralType
    {
        /// <summary>
        /// Of the format {t [N]''}
        /// </summary>
        Time,
        /// <summary>
        /// Of the format {d [N]''}
        /// </summary>
        Date,
        /// <summary>
        /// Of the format {ts [N]''}
        /// </summary>
        Timestamp,
        /// <summary>
        /// Of the format {guid [N]''}
        /// </summary>
        Guid,
    }

#pragma warning restore 1591
}
