//------------------------------------------------------------------------------
// <copyright file="TemporalRetentionPeriodUnit.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Possible values for units in HISTORY_RETENTION_PERIOD clause.
    /// </summary>
    public enum TemporalRetentionPeriodUnit
    {
        /// <summary>
        /// Retention period is expressed in days.
        /// </summary>
        Day,

        /// <summary>
        /// Retention period is expressed in days.
        /// </summary>
        Days,

        /// <summary>
        /// Retention period is expressed in weeks.
        /// </summary>
        Week,

        /// <summary>
        /// Retention period is expressed in weeks.
        /// </summary>
        Weeks,

        /// <summary>
        /// Retention period is expressed in months.
        /// </summary>
        Month,

        /// <summary>
        /// Retention period is expressed in months.
        /// </summary>
        Months,

        /// <summary>
        /// Retention period is expressed in years.
        /// </summary>
        Year,

        /// <summary>
        /// Retention period is expressed in years.
        /// </summary>
        Years,
    }
}