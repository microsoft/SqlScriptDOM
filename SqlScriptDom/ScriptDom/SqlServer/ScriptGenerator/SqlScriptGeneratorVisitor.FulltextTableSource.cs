//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.FulltextTableSource.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(FullTextTableReference node)
        {
            AlignmentPoint start = new AlignmentPoint();
            MarkAndPushAlignmentPoint(start);

            switch (node.FullTextFunctionType)
            {
                case FullTextFunctionType.FreeText:
                    GenerateKeyword(TSqlTokenType.FreeTextTable);
                    break;
                case FullTextFunctionType.Contains:
                    GenerateKeyword(TSqlTokenType.ContainsTable);
                    break;
            }

            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);

            GenerateFragmentIfNotNull(node.TableName);
            GenerateSymbolAndSpace(TSqlTokenType.Comma);
            if (node.PropertyName != null)
            {
                GenerateIdentifier(CodeGenerationSupporter.Property);
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateCommaSeparatedList(node.Columns);
                GenerateSymbol(TSqlTokenType.Comma);
                GenerateSpaceAndFragmentIfNotNull(node.PropertyName);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
            else
            {
                switch (node.Columns.Count)
                {
                    case 1:
                        GenerateFragmentIfNotNull(node.Columns[0]);
                        break;
                    default:
                        GenerateParenthesisedCommaSeparatedList(node.Columns);
                        break;
                }
            }

            GenerateSymbol(TSqlTokenType.Comma);

            GenerateSpaceAndFragmentIfNotNull(node.SearchCondition);

            if (node.Language != null)
            {
                GenerateSymbol(TSqlTokenType.Comma);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Language);
                GenerateSpaceAndFragmentIfNotNull(node.Language);
            }

            if (node.TopN != null)
            {
                GenerateSymbol(TSqlTokenType.Comma);
                GenerateSpaceAndFragmentIfNotNull(node.TopN);
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis);

            GenerateSpaceAndAlias(node.Alias);

            PopAlignmentPoint();
        }
    }
}
