//------------------------------------------------------------------------------
// <copyright file="SimpleAlterFulltextIndexActionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of alter fulltext index actions
    /// </summary>            
    public enum SimpleAlterFullTextIndexActionKind
    {
        None                    = 0,
        Enable                  = 1,
        Disable                 = 2,
        SetChangeTrackingManual = 3,
        SetChangeTrackingAuto   = 4,
        SetChangeTrackingOff    = 5,
        StartFullPopulation     = 6,
        StartIncrementalPopulation = 7,
        StartUpdatePopulation   = 8,
        StopPopulation          = 9,
        PausePopulation         = 10,
        ResumePopulation        = 11
    }

#pragma warning restore 1591
}
