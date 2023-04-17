//------------------------------------------------------------------------------
// <copyright file="FullTextIndexOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible FullText Index Options
    /// </summary>
    public enum FullTextIndexOptionKind
    {
        ChangeTracking  = 0,
        StopList        = 1,
        SearchPropertyList = 2,
    }

#pragma warning restore 1591
}
