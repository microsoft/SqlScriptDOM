//------------------------------------------------------------------------------
// <copyright file="TableOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

#pragma warning disable 1591
    /// <summary>
    /// The possible Table Options
    /// </summary>
    public enum TableOptionKind
    {
        LockEscalation  = 0,
        FileStreamOn    = 1,
        DataCompression = 2,
        FileTableDirectory = 3,
        FileTableCollateFileName = 4,
        FileTablePrimaryKeyConstraintName = 5,
        FileTableStreamIdUniqueConstraintName = 6,
        FileTableFullPathUniqueConstraintName = 7,
        MemoryOptimized = 8,
        Durability = 9,
        RemoteDataArchive = 10,
        Distribution = 11,
        Partition = 12,
        Location = 13,
        DataRetention = 14,
        XmlCompression = 15
    }

#pragma warning restore 1591
}
