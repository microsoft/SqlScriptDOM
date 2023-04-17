//------------------------------------------------------------------------------
// <copyright file="DbccOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of dbcc options
    /// </summary>           
    public enum DbccOptionKind
    {
        AllErrorMessages    = 0,
        CountRows           = 1,
        NoInfoMessages      = 2,
        TableResults        = 3,
        TabLock             = 4,
        StatHeader          = 5,
        DensityVector       = 6,
        HistogramSteps      = 7,
        EstimateOnly        = 8,
        Fast                = 9,
        AllLevels           = 10,
        AllIndexes          = 11,
        PhysicalOnly        = 12,
        AllConstraints      = 13,
        StatsStream         = 14,
        Histogram           = 15,
        DataPurity          = 16,
        MarkInUseForRemoval = 17,
        ExtendedLogicalChecks = 18,
    }

#pragma warning restore 1591
}
