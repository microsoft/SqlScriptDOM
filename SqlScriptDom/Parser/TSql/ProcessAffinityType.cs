//------------------------------------------------------------------------------
// <copyright file="ProcessAffinityType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The type of process affinity in ALTER SERVER CONFIGURATION statement
    /// </summary>   
    [Serializable]
    public enum ProcessAffinityType
    {
        CpuAuto     = 0,
        Cpu         = 1,
        NumaNode    = 2
    }

#pragma warning restore 1591
}
