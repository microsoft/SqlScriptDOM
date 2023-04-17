//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.QueryParenthesis.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(QueryParenthesisExpression node)
        {
            AlignmentPoint clauseBody = GetAlignmentPointForFragment(node, ClauseBody);
            GenerateQueryParenthesisExpression(node, clauseBody, null, null);
        }

        public void GenerateQueryParenthesisExpression(QueryParenthesisExpression node, AlignmentPoint clauseBody, SchemaObjectName intoClause, Identifier filegroupClause = null)
        {
            GenerateSymbol(TSqlTokenType.LeftParenthesis);

            GenerateQueryExpression(node.QueryExpression, clauseBody, intoClause, filegroupClause);

            GenerateSymbol(TSqlTokenType.RightParenthesis);

            if (node.OrderByClause != null)
            {
                GenerateSeparatorForOrderBy();

                GenerateFragmentWithAlignmentPointIfNotNull(node.OrderByClause, clauseBody);
            }

            if (node.OffsetClause != null)
            {
                GenerateSeparatorForOffsetClause();

                GenerateFragmentWithAlignmentPointIfNotNull(node.OffsetClause, clauseBody);
            }

            if (node.ForClause != null)
            {
                NewLine();
                GenerateKeyword(TSqlTokenType.For);
                MarkClauseBodyAlignmentWhenNecessary(true, clauseBody);
                GenerateSpace();

                AlignmentPoint forBody = new AlignmentPoint();
                MarkAndPushAlignmentPoint(forBody);

                GenerateFragmentIfNotNull(node.ForClause);

                PopAlignmentPoint();
            }
        }
    }
}
