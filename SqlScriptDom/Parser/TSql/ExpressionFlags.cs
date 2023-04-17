//------------------------------------------------------------------------------
// <copyright file="ExpressionFlags.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

#pragma warning disable 1591
    /// <summary>
    /// Used to keep track of various syntax allowed depending on context in expressions inside Parser
    /// </summary>
    [Flags]
    internal enum ExpressionFlags
    {
        None                        = 0x0,
        ScalarSubqueriesDisallowed  = 0x1,
        MatchClauseAllowed          = 0x2,
    }

#pragma warning restore 1591
}
