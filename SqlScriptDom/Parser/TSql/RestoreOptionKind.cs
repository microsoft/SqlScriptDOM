//------------------------------------------------------------------------------
// <copyright file="RestoreOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of restore options
    /// </summary>            
    [Serializable]
    public enum RestoreOptionKind
    {
        // Options without value
        NoLog                   = 1,
        Checksum                = 2,
        NoChecksum              = 3,
        ContinueAfterError      = 4,
        StopOnError             = 5,
        Unload                  = 6,
        NoUnload                = 7,
        Rewind                  = 8,
        NoRewind                = 9,
        NoRecovery              = 10,
        Recovery                = 11,
        Replace                 = 12,
        Restart                 = 13,
        Verbose                 = 14,
        LoadHistory             = 15,
        DboOnly                 = 16,
        RestrictedUser          = 17,
        Partial                 = 18,
        Snapshot                = 19,
        KeepReplication         = 20,
        Online                  = 21,
        CommitDifferentialBase  = 22,
        SnapshotImport          = 23,
            // Service broker options
        EnableBroker            = 24,
        NewBroker               = 25,
        ErrorBrokerConversations= 26,

        Stats                   = 27, // Can also have value

        // Options with value
        File                    = 28,
        StopAt                  = 29,
        MediaName               = 30,
        MediaPassword           = 31,
        Password                = 32,
        BlockSize               = 33,
        BufferCount             = 34,
        MaxTransferSize         = 35,
        Standby                 = 36,
        EnhancedIntegrity       = 37,
        SnapshotRestorePhase    = 38,

        Move                    = 39,
        Stop                    = 40,
        FileStream              = 50,
        KeepTemporalRetention   = 51,
    }

#pragma warning restore 1591
}
