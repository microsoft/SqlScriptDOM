//------------------------------------------------------------------------------
// <copyright file="ExternalDataSourcePushdown.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The enumeration specifies the external data source type for external tables
    /// Currently, we support HADOOP, RDBMS, and SHARD_MAP_MANAGER.
    /// </summary>
    public enum ExternalDataSourcePushdownOption
    {
        /// <summary>
        /// PUSDOWN = ON
        /// </summary>
        ON = 0,

        /// <summary>
        /// PUSHDOWN = OFF
        /// </summary>
        OFF = 1,
    }

#pragma warning restore 1591
}
