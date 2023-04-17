//------------------------------------------------------------------------------
// <copyright file="MigrationState.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of migration state
    /// </summary>        
    public enum MigrationState
    {
        Paused         = 0,
        Outbound       = 1,
        Inbound        = 2
    }

#pragma warning restore 1591
}
