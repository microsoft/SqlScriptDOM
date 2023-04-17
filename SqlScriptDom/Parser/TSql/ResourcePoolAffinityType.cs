//------------------------------------------------------------------------------
// <copyright file="ResourcePoolAffinityType.cs" company="Microsoft">
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
    public enum ResourcePoolAffinityType
    {   
        None = 0,
        Scheduler = 1,
        NumaNode = 2,
    }

#pragma warning restore 1591
}
