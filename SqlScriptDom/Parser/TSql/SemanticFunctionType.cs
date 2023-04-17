//------------------------------------------------------------------------------
// <copyright file="SemanticFunctionType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The semantic function types.
    /// </summary>
    public enum SemanticFunctionType
    {
        /// <summary>
        /// Nothing was specified.
        /// </summary>
        None = 0,
        /// <summary>
        /// SEMANTICKEYPHRASETABLE keyword.
        /// </summary>
        SemanticKeyPhraseTable = 1,
        /// <summary>
        /// SEMANTICSIMILARITYTABLE keyword.
        /// </summary>
        SemanticSimilarityTable = 2,
        /// <summary>
        /// SEMANTICSIMILARITYDETAILSTABLE keyword.
        /// </summary>
        SemanticSimilarityDetailsTable = 3,
    }
}