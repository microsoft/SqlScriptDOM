//------------------------------------------------------------------------------
// <copyright file="SecurityPredicateActionType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

#pragma warning disable 1591
    /// <summary>
    /// The types of security predicate options
    /// </summary>
    public enum SecurityPredicateActionType
    {
        Create = 0,
        Alter = 1,
        Drop = 2
    }
#pragma warning restore 1591
}
