//------------------------------------------------------------------------------
// <copyright file="WorkloadClassifierOptionType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The workload classifier options.
    /// </summary>
    public enum WorkloadClassifierOptionType
    {
        WorkloadGroup = 0,
        MemberName = 1,
        WlmLabel = 2,
        WlmContext = 3,
        StartTime = 4,
        EndTime = 5,
        Importance = 6
    }

#pragma warning restore 1591
}
