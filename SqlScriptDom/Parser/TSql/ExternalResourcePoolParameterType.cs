//------------------------------------------------------------------------------
// <copyright file="ExternalResourcePoolParameterType.cs" company="Microsoft">
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
    /// The types of parameters used in a CREATE/ALTER EXTERNAL RESOURCE POOL statement
    /// </summary>
    [Serializable]
    public enum ExternalResourcePoolParameterType
    {
        Unknown = 0,
        MaxCpuPercent = 1,
        MaxMemoryPercent = 2,
        MaxProcesses = 3,
        Affinity = 4,
    }

#pragma warning restore 1591
}
