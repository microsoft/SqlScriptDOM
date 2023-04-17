//------------------------------------------------------------------------------
// <copyright file="ResultSetsOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

#pragma warning disable 1591
    /// <summary>
    /// Indicates whether the Result Sets are defined on Execute Statements
    /// </summary>
    public enum ResultSetsOptionKind
    {
        Undefined   = 0,
        None        = 1,
        ResultSetsDefined  = 2,
    }

#pragma warning restore 1591
}
