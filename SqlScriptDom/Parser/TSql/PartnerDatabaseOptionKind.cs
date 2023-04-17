//------------------------------------------------------------------------------
// <copyright file="PartnerDatabaseOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of partner alter database options
    /// </summary>            
    public enum PartnerDatabaseOptionKind
    {
        None                        = 0,
        PartnerServer               = 1,
        Failover                    = 2,
        ForceServiceAllowDataLoss   = 3,
        Off                         = 4,
        Resume                      = 5,
        SafetyFull                  = 6,
        SafetyOff                   = 7,
        Suspend                     = 8,
        Timeout                     = 9,
    }

#pragma warning restore 1591
}
