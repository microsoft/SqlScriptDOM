//------------------------------------------------------------------------------
// <copyright file="SecurityPredicateType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591
    /// <summary>
    /// The types of security predicates
    /// </summary>
    public enum SecurityPredicateType
    {
        Filter = 0,
        Block = 1
    }
#pragma warning restore 1591
}
