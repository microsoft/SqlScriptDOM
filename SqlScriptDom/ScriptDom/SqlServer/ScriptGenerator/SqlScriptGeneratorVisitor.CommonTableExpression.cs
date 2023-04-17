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

            GenerateQueryExpressionInParentheses(node.QueryExpression);

            PopAlignmentPoint();
        }
    }
}
