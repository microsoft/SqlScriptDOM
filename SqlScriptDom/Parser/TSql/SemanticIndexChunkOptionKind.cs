//------------------------------------------------------------------------------
// <copyright file="SemanticIndexChunkOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible values for semantic index chunk options.
    /// </summary>              
    public enum SemanticIndexChunkOptionKind
    {
        Type = 0,
        Size = 1,
        Overlap = 2
    }

#pragma warning restore 1591
}
