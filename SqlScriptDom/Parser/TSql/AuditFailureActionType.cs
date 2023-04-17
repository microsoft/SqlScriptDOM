//------------------------------------------------------------------------------
// <copyright file="AuditFailureActionType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The action to take on audit failure.
    /// </summary>
    public enum AuditFailureActionType
    {
        /// <summary>
        /// Continue.
        /// </summary>
        Continue        = 0,
        /// <summary>
        /// Shutdown the server.
        /// </summary>
        Shutdown        = 1,
        /// <summary>
        /// Fail the operation.
        /// </summary>
        FailOperation   = 2,
    }
}
