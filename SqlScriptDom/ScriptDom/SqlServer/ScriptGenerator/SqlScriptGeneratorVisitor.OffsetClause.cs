//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.OffsetClause.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(OffsetClause node)
        {
            AlignmentPoint start = new AlignmentPoint();
            MarkAndPushAlignmentPoint(start);

            GenerateIdentifier(CodeGenerationSupporter.Offset);
            GenerateSpaceAndFragmentIfNotNull(node.OffsetExpression);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Rows);

            if (node.FetchExpression != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Fetch);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Next);
                GenerateSpaceAndFragmentIfNotNull(node.FetchExpression);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Rows);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Only);
            }

            PopAlignmentPoint();
        }
    }
}
