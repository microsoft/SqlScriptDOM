//------------------------------------------------------------------------------
// <copyright file="BackupOptions.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of backup options
    /// </summary>       
    public enum BackupOptionKind
    {
        None            = 0,
        // Options without value
        NoRecovery      = 1,
        TruncateOnly    = 2,
        NoLog           = 3,
        NoTruncate      = 4,
        Unload          = 5,
        NoUnload        = 6,
        Rewind          = 7,
        NoRewind        = 8,
        Format          = 9,
        NoFormat        = 10,
        Init            = 11,
        NoInit          = 12,
        Skip            = 13,
        NoSkip          = 14,
        Restart         = 15,
        Stats           = 16, // Can have value
        Differential    = 17,
        Snapshot        = 18,
        Checksum        = 19,
        NoChecksum      = 20,
        ContinueAfterError = 21,
        StopOnError     = 22,
        CopyOnly        = 23,

        // Options with value
        Standby         = 24,
        ExpireDate      = 25,
        RetainDays      = 26,
        Name            = 27,
        Description     = 28,
        Password        = 29,
        MediaName       = 30,
        MediaDescription= 31,
        MediaPassword   = 32,
        BlockSize       = 33,
        BufferCount     = 34,
        MaxTransferSize = 35,
        EnhancedIntegrity = 36,

        // Added in Sql 2008 options (no value)
        Compression     = 37,
        NoCompression   = 38,

        // Added in SQL 2014
        Encryption      = 39
    }


#pragma warning restore 1591
}
