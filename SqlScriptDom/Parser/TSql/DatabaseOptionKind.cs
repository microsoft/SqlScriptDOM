//------------------------------------------------------------------------------
// <copyright file="DatabaseOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of database options
    /// </summary>   
    public enum DatabaseOptionKind
    {
        // Simple options
        Online                      = 0,
        Offline                     = 1,
        Emergency                   = 2,
        SingleUser                  = 3,
        RestrictedUser              = 4,
        MultiUser                   = 5,
        ReadOnly                    = 6,
        ReadWrite                   = 7,
        EnableBroker                = 8,
        DisableBroker               = 9,
        NewBroker                   = 10,
        ErrorBrokerConversations    = 11,

        // On/Off options
        DBChaining                  = 12,
        Trustworthy                 = 13,
        CursorCloseOnCommit         = 14,
        AutoClose                   = 15,
        AutoCreateStatistics        = 16,
        AutoShrink                  = 17,
        AutoUpdateStatistics        = 18,
        AutoUpdateStatisticsAsync   = 19,
        AnsiNullDefault             = 20,
        AnsiNulls                   = 21,
        AnsiPadding                 = 22,
        AnsiWarnings                = 23,
        ArithAbort                  = 24,
        ConcatNullYieldsNull        = 25,
        NumericRoundAbort           = 26,
        QuotedIdentifier            = 27,
        RecursiveTriggers           = 28,
        TornPageDetection           = 29,
        DateCorrelationOptimization = 30,
        AllowSnapshotIsolation      = 31,
        ReadCommittedSnapshot       = 32,

        // T-SQL 100 on/off options
        Encryption                  = 33,
        HonorBrokerPriority         = 34,
        VarDecimalStorageFormat     = 35,

        SupplementalLogging         = 36,

        CompatibilityLevel          = 37,

        CursorDefault               = 38,
        Recovery                    = 39,
        PageVerify                  = 40,
        Partner                     = 41,
        Witness                     = 42,
        Parameterization            = 43,
        ChangeTracking              = 44,

        DefaultLanguage             = 45, 
        DefaultFullTextLanguage     = 46, 
        NestedTriggers              = 47, 
        TransformNoiseWords         = 48, 
        TwoDigitYearCutoff          = 49, 
        Containment                 = 50, 
        Hadr                        = 51,
        FileStream                  = 52,

        Edition                     = 53,
        MaxSize                     = 54,
        TargetRecoveryTime          = 55,
        MemoryOptimizedData         = 56,
        DelayedDurability           = 57,
        MemoryOptimizedElevateToSnapshot           = 58,
        ServiceObjective            = 59,
        RemoteDataArchive           = 60,
        QueryStore                  = 61,
        MixedPageAllocation         = 62,
        TemporalHistoryRetention    = 63,
        CatalogCollation            = 64,
        AutomaticTuning             = 65,
        AcceleratedDatabaseRecovery = 66,
        
        // T-SQL 150 On/Off options
        DataRetention               = 67, 
        Ledger                      = 68,

        ManualCutover               = 69,
        PerformCutover              = 70
    }

#pragma warning restore 1591
}
