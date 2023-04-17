//------------------------------------------------------------------------------
// <copyright file="IndexOptionType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible index options.
    /// </summary>
    public enum IndexOptionKind
    {
        PadIndex            = 0,
        FillFactor          = 1,
        SortInTempDB        = 2,
        IgnoreDupKey        = 3,
        StatisticsNoRecompute = 4,
        DropExisting        = 5,
        Online              = 6,
        AllowRowLocks       = 7,
        AllowPageLocks      = 8,
        MaxDop              = 9,
        LobCompaction       = 10,
        FileStreamOn        = 11,
        DataCompression     = 12,
        MoveTo              = 13,
        BucketCount         = 14,
        StatisticsIncremental = 15,
        Order               = 16,
        CompressAllRowGroups = 17,
        CompressionDelay    = 18,
        Resumable           = 19,
        MaxDuration         = 20,
        WaitAtLowPriority   = 21,
        OptimizeForSequentialKey = 22,
        XmlCompression      = 23,
    }

#pragma warning restore 1591
}
