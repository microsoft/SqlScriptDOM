//------------------------------------------------------------------------------
// <copyright file="TableHintKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

#pragma warning disable 1591
    /// <summary>
    /// The table hints.
    /// </summary>
    [Serializable]
    public enum TableHintKind
    {
        /// <summary>
        /// Nothing was used.
        /// </summary>
        None                = 0,
        /// <summary>
        /// FASTFIRSTROW keyword.
        /// </summary>
        FastFirstRow        = 1,
        /// <summary>
        /// HOLDLOCK keyword.
        /// </summary>
        HoldLock            = 2,
        /// <summary>
        /// NOLOCK keyword.
        /// </summary>
        NoLock              = 3,
        /// <summary>
        /// PAGLOCK keyword.
        /// </summary>
        PagLock             = 4,
        /// <summary>
        /// READCOMMITTED keyword.
        /// </summary>
        ReadCommitted       = 5,
        /// <summary>
        /// READPAST keyword.
        /// </summary>
        ReadPast            = 6,
        /// <summary>
        /// READUNCOMMITTED keyword.
        /// </summary>
        ReadUncommitted     = 7,
        /// <summary>
        /// REPEATABLEREAD keyword.
        /// </summary>
        RepeatableRead      = 8,
        /// <summary>
        /// ROWLOCK keyword.
        /// </summary>
        Rowlock             = 9,
        /// <summary>
        /// SERIALIZABLE keyword.
        /// </summary>
        Serializable        = 10,
        /// <summary>
        /// TABLOCK keyword.
        /// </summary>
        TabLock             = 11,
        /// <summary>
        /// TABLOCKX keyword.
        /// </summary>
        TabLockX            = 12,
        /// <summary>
        /// UPDLOCK keyword.
        /// </summary>
        UpdLock             = 13,
        /// <summary>
        /// XLOCK keyword.
        /// </summary>
        XLock               = 14,
        /// <summary>
        /// NOEXPAND keyword.
        /// </summary>
        NoExpand            = 15,
        /// <summary>
        /// NOWAIT keyword.
        /// </summary>
        NoWait              = 16,
        /// <summary>
        /// READCOMMITTEDLOCK keyword.
        /// </summary>
        ReadCommittedLock   = 17,
        /// <summary>
        /// KEEPIDENTITY keyword.
        /// </summary>
        KeepIdentity        = 18,
        /// <summary>
        /// KEEPDEFAULTS keyword.
        /// </summary>
        KeepDefaults        = 19,
        /// <summary>
        /// IGNORE_CONSTRAINTS keyword.
        /// </summary>
        IgnoreConstraints   = 20,
        /// <summary>
        /// IGNORE_TRIGGERS keyword.
        /// </summary>
        IgnoreTriggers      = 21,
        /// <summary>
        /// FORCESEEK keyword.
        /// </summary>        
        ForceSeek           = 22,
        /// <summary>
        /// INDEX table hint
        /// </summary>
        Index               = 23,
        /// <summary>
        /// SPATIAL_WINDOW_MAX_CELLS table hint
        /// </summary>
        SpatialWindowMaxCells = 24,
        /// <summary>
        /// FORCESCAN keyword.
        /// </summary>        
        ForceScan           = 25,
        /// <summary>
        /// SNAPSHOT keyword
        /// </summary>
        Snapshot            = 26,
    }
}
