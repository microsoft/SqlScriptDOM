//------------------------------------------------------------------------------
// <copyright file="SecurityPredicateOperation.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591
    /// <summary>
    /// The types of security predicate operations
    /// </summary>
    public enum SecurityPredicateOperation
    {
        All = 0,
        AfterInsert = 1,
        AfterUpdate = 2,
        BeforeUpdate = 3,
        BeforeDelete = 4
    }
#pragma warning restore 1591
}
