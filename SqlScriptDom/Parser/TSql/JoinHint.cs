//------------------------------------------------------------------------------
// <copyright file="JoinHint.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{   
    /// <summary>
    /// The types of join hints.
    /// </summary>
    public enum JoinHint
    {
        /// <summary>
        /// Nothing.
        /// </summary>
        None = 0,
        /// <summary>
        /// LOOP was used.
        /// </summary>
        Loop = 1,
        /// <summary>
        /// HASH was used.
        /// </summary>
        Hash = 2,
        /// <summary>
        /// MERGE was used.
        /// </summary>
        Merge = 3,
        /// <summary>
        /// REMOTE was used.
        /// </summary>
        Remote = 4
    }
}
