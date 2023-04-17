//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterFulltextCatalogStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(MergeActionClause node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.When);
            switch (node.Condition)
            {
                case MergeCondition.Matched:
                    GenerateIdentifier(CodeGenerationSupporter.Matched);
                    break;
                case MergeCondition.NotMatched:
                    GenerateKeyword(TSqlTokenType.Not);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Matched);
                    break;
                case MergeCondition.NotMatchedBySource:
                    GenerateKeyword(TSqlTokenType.Not);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Matched);
                    GenerateSpaceAndKeyword(TSqlTokenType.By);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Source);
                    break;
                case MergeCondition.NotMatchedByTarget:
                    GenerateKeyword(TSqlTokenType.Not);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Matched);
                    GenerateSpaceAndKeyword(TSqlTokenType.By);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Target);
                    break;
                case MergeCondition.NotSpecified:
                default:
                    break;
            }

            if (node.SearchCondition != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.And);
                GenerateSpaceAndFragmentIfNotNull(node.SearchCondition);
            }

            GenerateSpaceAndKeyword(TSqlTokenType.Then);
            GenerateSpaceAndFragmentIfNotNull(node.Action);
        }
    }
}
