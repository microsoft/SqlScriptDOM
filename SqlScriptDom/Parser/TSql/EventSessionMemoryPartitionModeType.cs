//------------------------------------------------------------------------------
// <copyright file="EventSessionMemoryPartitionModeType.cs" company="Microsoft">
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
    /// The types of memory partition mode
    /// </summary>        
    [Serializable]
    public enum EventSessionMemoryPartitionModeType
    {
        Unknown = 0,
        None = 1,
        PerNode = 2,
        PerCpu = 3,
    }

#pragma warning restore 1591
}
