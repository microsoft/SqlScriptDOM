//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SelectStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(SelectStatement node)
        {
            AlignmentPoint start = new AlignmentPoint();
            AlignmentPoint clauseBody = new AlignmentPoint(ClauseBody);

            MarkAndPushAlignmentPoint(start);

            if (node.WithCtesAndXmlNamespaces != null)
            {
                // the alignment point should be consumed by WithCommonTableExpressionsAndXmlNamespaces
                //AddFloatingAlignmentPointForCollecting(clauseBody);

                GenerateFragmentWithAlignmentPointIfNotNull(node.WithCtesAndXmlNamespaces, clauseBody);
                NewLine();
            }

            GenerateQueryExpression(node.QueryExpression, clauseBody, node.Into, node.On);

            foreach (ComputeClause clause in node.ComputeClauses)
            {
                NewLine();

                GenerateFragmentWithAlignmentPointIfNotNull(clause, clauseBody);
            }

            GenerateOptimizerHints(node.OptimizerHints);

            PopAlignmentPoint();
        }

        void GenerateQueryExpression(QueryExpression queryExpression, AlignmentPoint clauseBody, SchemaObjectName intoClause, Identifier filegroupClause = null)
        {
            QuerySpecification querySpecification = queryExpression as QuerySpecification;
            if (querySpecification != null)
            {
                GenerateQuerySpecification(querySpecification, clauseBody, intoClause, filegroupClause);
                return;
            }
            BinaryQueryExpression binaryQueryExpression = queryExpression as BinaryQueryExpression;
            if (binaryQueryExpression != null)
            {
                GenerateBinaryQueryExpression(binaryQueryExpression, clauseBody, intoClause, filegroupClause);
                return;
            }
            QueryParenthesisExpression queryParenthesisExpression = queryExpression as QueryParenthesisExpression;
            if (queryParenthesisExpression != null)
            {
                GenerateQueryParenthesisExpression(queryParenthesisExpression, clauseBody, intoClause, filegroupClause);
                return;
            }
        }
    }
}
