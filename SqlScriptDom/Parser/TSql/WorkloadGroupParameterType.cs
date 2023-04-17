//------------------------------------------------------------------------------
// <copyright file="WorkloadGroupParameterType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

#pragma warning disable 1591

    /// <summary>
    /// The types of workload resource paramters
    /// </summary>
    [Serializable]
    public enum WorkloadGroupParameterType
    {
        Importance                      = 0,
        RequestMaxMemoryGrantPercent    = 1,
        RequestMaxCpuTimeSec            = 2,
        RequestMemoryGrantTimeoutSec    = 3,
        MaxDop                          = 4,
        GroupMaxRequests                = 5,
        GroupMinMemoryPercent           = 6,
        MinPercentageResource           = 7,
        CapPercentageResource           = 8,
        RequestMinResourceGrantPercent  = 9,
        RequestMaxResourceGrantPercent  = 10,
        QueryExecutionTimeoutSec        = 11,
    }

#pragma warning restore 1591

}
