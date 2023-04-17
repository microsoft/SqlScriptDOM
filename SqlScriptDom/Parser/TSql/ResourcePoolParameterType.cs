//------------------------------------------------------------------------------
// <copyright file="ResourcePoolParameterType.cs" company="Microsoft">
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
    /// The types of paramters used in a CREATE/ALTER RESOURCE POOL statement
    /// </summary>            
    [Serializable]
    public enum ResourcePoolParameterType
    {   
        Unknown = 0,
        MaxCpuPercent = 1,
        MaxMemoryPercent = 2,
        MinCpuPercent = 3,
        MinMemoryPercent = 4,
        CapCpuPercent = 5,
        TargetMemoryPercent = 6,
        MinIoPercent = 7,
        MaxIoPercent = 8,
        CapIoPercent = 9,
        Affinity = 10,
        MinIopsPerVolume = 11,
        MaxIopsPerVolume = 12,
    }

#pragma warning restore 1591
}
