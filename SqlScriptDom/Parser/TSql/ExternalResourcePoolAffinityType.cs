//------------------------------------------------------------------------------
// <copyright file="ExternalResourcePoolAffinityType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of paramters used in a CREATE/ALTER EXTERNAL RESOURCE POOL statement
    /// </summary>
    [Serializable]
    public enum ExternalResourcePoolAffinityType
    {
        None = 0,
        Cpu = 1,
        NumaNode = 2,
    }

#pragma warning restore 1591
}
