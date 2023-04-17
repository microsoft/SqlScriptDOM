//------------------------------------------------------------------------------
// <copyright file="IsolationLevel.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// Isolation levels for SET TRANSACTION ISOLATION LEVEL command
    /// </summary>
    public enum IsolationLevel
    {
        None            = 0,
        ReadCommitted   = 1,
        ReadUncommitted = 2,
        RepeatableRead  = 3,
        Serializable    = 4,
        Snapshot        = 5,
    }

#pragma warning restore 1591
}
