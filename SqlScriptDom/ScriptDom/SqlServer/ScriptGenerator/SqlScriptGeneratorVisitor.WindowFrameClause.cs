//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.WindowFrameClause.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(WindowFrameClause node)
        {
            switch (node.WindowFrameType)
            {
                case WindowFrameType.Rows:
                    GenerateIdentifier(CodeGenerationSupporter.Rows);
                    break;
                case WindowFrameType.Range:
                    GenerateIdentifier(CodeGenerationSupporter.Range);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "shouldn't get here");
                    break;
            }

            if (node.Bottom != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Between);
                GenerateSpaceAndFragmentIfNotNull(node.Top);
                GenerateSpaceAndKeyword(TSqlTokenType.And);
                GenerateSpaceAndFragmentIfNotNull(node.Bottom);
            }
            else
            {
                GenerateSpaceAndFragmentIfNotNull(node.Top);
            }
        }

        public override void ExplicitVisit(WindowDelimiter node)
        {
            switch (node.WindowDelimiterType)
            {
                case WindowDelimiterType.CurrentRow:
                    GenerateKeyword(TSqlTokenType.Current);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Row);
                    break;
                case WindowDelimiterType.UnboundedPreceding:
                    GenerateIdentifier(CodeGenerationSupporter.Unbounded);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Preceding);
                    break;
                case WindowDelimiterType.UnboundedFollowing:
                    GenerateIdentifier(CodeGenerationSupporter.Unbounded);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Following);
                    break;
                case WindowDelimiterType.ValuePreceding:
                    GenerateFragmentIfNotNull(node.OffsetValue);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Preceding);
                    break;
                case WindowDelimiterType.ValueFollowing:
                    GenerateFragmentIfNotNull(node.OffsetValue);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Following);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "shouldn't get here");
                    break;
            }
        }
    }
}
