//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.VectorSearchTableReference.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        /// <summary>
        /// Script generator visitor for VECTOR_SEARCH function: https://learn.microsoft.com/sql/t-sql/functions/vector-search-transact-sql
        /// Syntax:
        /// VECTOR_SEARCH(
        ///     TABLE = object[AS source_table_alias],
        ///     COLUMN = vector_column,
        ///     SIMILAR_TO = query_vector,
        ///     METRIC = { 'cosine' | 'dot' | 'euclidean' },
        ///     TOP_N = k
        /// ) [AS result_table_alias]
        /// </summary>
        public override void ExplicitVisit(VectorSearchTableReference node)
        {
            AlignmentPoint start = new AlignmentPoint();
            MarkAndPushAlignmentPoint(start);

            GenerateIdentifier(CodeGenerationSupporter.VectorSearch);
            GenerateSymbol(TSqlTokenType.LeftParenthesis);

            NewLineAndIndent();
            GenerateNameEqualsValue(TSqlTokenType.Table, node.Table);
            GenerateSymbol(TSqlTokenType.Comma);

            NewLineAndIndent();
            GenerateNameEqualsValue(TSqlTokenType.Column, node.Column);
            GenerateSymbol(TSqlTokenType.Comma);

            NewLineAndIndent();
            GenerateNameEqualsValue(CodeGenerationSupporter.SimilarTo, node.SimilarTo);
            GenerateSymbol(TSqlTokenType.Comma);

            NewLineAndIndent();
            GenerateNameEqualsValue(CodeGenerationSupporter.Metric, node.Metric);
            GenerateSymbol(TSqlTokenType.Comma);

            NewLineAndIndent();
            GenerateNameEqualsValue(CodeGenerationSupporter.TopN, node.TopN);

            NewLine();
            GenerateSymbol(TSqlTokenType.RightParenthesis);
            GenerateSpaceAndAlias(node.Alias);

            PopAlignmentPoint();
        }
    }
}
