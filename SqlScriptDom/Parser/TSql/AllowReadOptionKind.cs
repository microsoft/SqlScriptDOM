//------------------------------------------------------------------------------
// <copyright file="AllowReadOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The allow read options.
    /// </summary>       
    public enum AllowConnectionsOptionKind
    {
        /// <summary>
        /// No connections allowed
        /// </summary>
        No                      = 0,
        /// <summary>
        /// Read-only connections allowed
        /// </summary>
        ReadOnly     = 1,

        /// <summary>
        /// Read-write connections allowed.
        /// </summary>
        ReadWrite   = 2,

        /// <summary>
        /// All connections allowed.
        /// </summary>
        All          = 3,
    }
}
