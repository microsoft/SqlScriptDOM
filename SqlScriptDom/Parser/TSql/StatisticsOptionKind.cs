//------------------------------------------------------------------------------
// <copyright file="StatisticsOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible Statistics options.
    /// </summary>
    [Serializable]
    public enum StatisticsOptionKind
    {
        FullScan = 0,
        SamplePercent = 1,
        SampleRows = 2,
        StatsStream = 3,
        NoRecompute = 4,
        Resample = 5,
        RowCount = 6,
        PageCount = 7,
        All = 8,
        Columns = 9,
        Index = 10,
        Rows = 11,
        Incremental = 12,
        AutoDrop = 13,
    }

#pragma warning restore 1591
}
