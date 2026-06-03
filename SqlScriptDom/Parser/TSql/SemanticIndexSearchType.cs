//------------------------------------------------------------------------------
// <copyright file="SemanticIndexSearchType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible values for semantic index search type.
    /// </summary>              
    public enum SemanticIndexSearchType
    {
        NotSpecified = 0,
        Vector = 1,
        Fulltext = 2,
        Hybrid = 3
    }

#pragma warning restore 1591
}
