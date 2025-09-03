//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CommonTableExpression.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CommonTableExpression node)
        {
            AlignmentPoint clauseBody = GetAlignmentPointForFragment(node, ClauseBody);
            MarkClauseBodyAlignmentWhenNecessary(true, clauseBody);
            GenerateSpaceAndFragmentIfNotNull(node.ExpressionName);

            if (node.Columns.Count > 0)
            {
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.Columns);
            }

            NewLine();
            GenerateKeyword(TSqlTokenType.As);

            MarkClauseBodyAlignmentWhenNecessary(true, clauseBody);

            GenerateSpace();

            AlignmentPoint subquery = new AlignmentPoint();
            MarkAndPushAlignmentPoint(subquery);

            GenerateSymbol(TSqlTokenType.LeftParenthesis);

            AlignmentPoint queryBody = new AlignmentPoint();
            MarkAndPushAlignmentPoint(queryBody);

            // Generate nested WITH clause if present
            if (node.WithCtesAndXmlNamespaces != null)
            {
                AlignmentPoint clauseBodyNested = new AlignmentPoint(ClauseBody);
                GenerateFragmentWithAlignmentPointIfNotNull(node.WithCtesAndXmlNamespaces, clauseBodyNested);
                NewLine();
            }

            if (node.QueryExpression != null)
            {
                AlignmentPoint clauseBodyQuery = new AlignmentPoint(ClauseBody);
                GenerateFragmentWithAlignmentPointIfNotNull(node.QueryExpression, clauseBodyQuery);
            }

            PopAlignmentPoint();

            GenerateSymbol(TSqlTokenType.RightParenthesis);

            PopAlignmentPoint();
        }
    }
}
