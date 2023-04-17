//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SemanticTableReference.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(SemanticTableReference node)
        {
            AlignmentPoint start = new AlignmentPoint();
            MarkAndPushAlignmentPoint(start);

            switch (node.SemanticFunctionType)
            {
                case SemanticFunctionType.SemanticKeyPhraseTable:
                    GenerateKeyword(TSqlTokenType.SemanticKeyPhraseTable);
                    break;
                case SemanticFunctionType.SemanticSimilarityTable:
                    GenerateKeyword(TSqlTokenType.SemanticSimilarityTable);
                    break;
                case SemanticFunctionType.SemanticSimilarityDetailsTable:
                    GenerateKeyword(TSqlTokenType.SemanticSimilarityDetailsTable);
                    break;
            }

            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);

            GenerateFragmentIfNotNull(node.TableName);
            GenerateSymbolAndSpace(TSqlTokenType.Comma);

            switch (node.Columns.Count)
            {
                case 1:
                    GenerateFragmentIfNotNull(node.Columns[0]);
                    break;
                default:
                    GenerateParenthesisedCommaSeparatedList(node.Columns);
                    break;
            }

            if (node.SourceKey != null)
            {
                GenerateSymbol(TSqlTokenType.Comma);
                GenerateSpaceAndFragmentIfNotNull(node.SourceKey);
            }

            if (node.MatchedColumn != null)
            {
                GenerateSymbol(TSqlTokenType.Comma);
                GenerateSpaceAndFragmentIfNotNull(node.MatchedColumn);
            }

            if (node.MatchedKey != null)
            {
                GenerateSymbol(TSqlTokenType.Comma);
                GenerateSpaceAndFragmentIfNotNull(node.MatchedKey);
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis);

            GenerateSpaceAndAlias(node.Alias);

            PopAlignmentPoint();
        }
    }
}