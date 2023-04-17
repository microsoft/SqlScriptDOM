//------------------------------------------------------------------------------
// <copyright file="NonTransactedFileStreamAccess.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The possible non transacted filestream access values.
    /// </summary>           
    public enum NonTransactedFileStreamAccess
    {
        /// <summary>
        /// No non-transacted access.
        /// </summary>
        Off       = 0,
        /// <summary>
        /// Read-only access.
        /// </summary>
        ReadOnly  = 1,
        /// <summary>
        /// Full access.
        /// </summary>
        Full      = 2,
    }
}
