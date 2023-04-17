//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.OutputClause.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(OutputClause node)
        {
            AlignmentPoint start = new AlignmentPoint();
            MarkAndPushAlignmentPoint(start);

            GenerateIdentifier(CodeGenerationSupporter.Output);

            AlignmentPoint clauseBody = GetAlignmentPointForFragment(node, ClauseBody);
            MarkClauseBodyAlignmentWhenNecessary(true, clauseBody);

            GenerateSpace();
            GenerateCommaSeparatedList(node.SelectColumns);
            PopAlignmentPoint();
        }

        public override void ExplicitVisit(OutputIntoClause node)
        {
            AlignmentPoint start = new AlignmentPoint();
            MarkAndPushAlignmentPoint(start);

            GenerateIdentifier(CodeGenerationSupporter.Output);

            AlignmentPoint clauseBody = GetAlignmentPointForFragment(node, ClauseBody);
            MarkClauseBodyAlignmentWhenNecessary(true, clauseBody);

            GenerateSpace();
            GenerateCommaSeparatedList(node.SelectColumns);
            GenerateSpaceAndKeyword(TSqlTokenType.Into);
            GenerateSpaceAndFragmentIfNotNull(node.IntoTable);

            if (node.IntoTableColumns.Count > 0)
            {
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.IntoTableColumns);
            }
            PopAlignmentPoint();
        }
    }
}
