//------------------------------------------------------------------------------
// <copyright file="TableSwitchOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible options for alter table switch statement.
    /// </summary>
    public enum TableSwitchOptionKind
    {
        LowPriorityLockWait = 0,
        TruncateTarget = 1
    }

#pragma warning restore 1591
}
