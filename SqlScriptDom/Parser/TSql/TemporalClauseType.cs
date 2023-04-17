//------------------------------------------------------------------------------
// <copyright file="TemporalClauseType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Possible values for temporal clause in 'FROM' part of a query.
    /// </summary>
    public enum TemporalClauseType
    {
        /// <summary>
        /// FOR SYSTEM_TIME AS OF ...
        /// </summary>
        AsOf,

        /// <summary>
        /// FOR SYSTEM_TIME FROM ... TO ...
        /// </summary>
        FromTo,

        /// <summary>
        /// FOR SYSTEM_TIME BETWEEN ... AND ...
        /// </summary>
        Between,

        /// <summary>
        /// FOR SYSTEM_TIME CONTAINED IN (..., ...)
        /// </summary>
        ContainedIn,

        /// <summary>
        /// FOR SYSTEM_TIME ALL
        /// </summary>
        TemporalAll,
    }
}