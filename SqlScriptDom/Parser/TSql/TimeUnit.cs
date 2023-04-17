//------------------------------------------------------------------------------
// <copyright file="TimeUnit.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of time units used in various statements.
    /// More time units can be added as necessary.
    /// </summary>            
    [Serializable]
    public enum TimeUnit
    {
        Seconds        = 0,
        Minutes        = 1,
        Hours          = 2,
        Days           = 3,
    }

#pragma warning restore 1591
}

