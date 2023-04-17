//------------------------------------------------------------------------------
// <copyright file="LowPriorityLockWaitOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible low priority lock wait options.
    /// </summary>
    public enum LowPriorityLockWaitOptionKind
    {
        MaxDuration = 0,
        AbortAfterWait = 1,
    }

#pragma warning restore 1591
}
