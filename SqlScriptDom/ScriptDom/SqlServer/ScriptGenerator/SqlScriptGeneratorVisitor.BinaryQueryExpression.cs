//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BinaryQueryExpression.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<BinaryQueryExpressionType, TokenGenerator> _binaryQueryExpressionTypeGenerators =
            new Dictionary<BinaryQueryExpressionType, TokenGenerator>()
        {
            {BinaryQueryExpressionType.Except, new KeywordGenerator(TSqlTokenType.Except)},
            {BinaryQueryExpressionType.Intersect, new KeywordGenerator(TSqlTokenType.Intersect)},
            {BinaryQueryExpressionType.Union, new KeywordGenerator(TSqlTokenType.Union)},
        };
        public override void ExplicitVisit(BinaryQueryExpression node)
        {
            AlignmentPoint clauseBody = GetAlignmentPointForFragment(node, ClauseBody);
            GenerateBinaryQueryExpression(node, clauseBody, null, null);
        }

        public void GenerateBinaryQueryExpression(BinaryQueryExpression node, AlignmentPoint clauseBody, SchemaObjectName intoClause, Identifier filegroupClause = null)
        {
            AlignmentPoint start = new AlignmentPoint();
            MarkAndPushAlignmentPoint(start);

            GenerateQueryExpression(node.FirstQueryExpression, clauseBody, intoClause, filegroupClause);

            TokenGenerator generator = GetValueForEnumKey(_binaryQueryExpressionTypeGenerators, node.BinaryQueryExpressionType);
            if (generator != null)
            {
                NewLine();
                GenerateToken(generator);
            }

            if (node.All)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.All);
            }

            if (node.SecondQueryExpression != null)
            {
                NewLine();

                AlignmentPoint secondExpr = new AlignmentPoint();
                MarkAndPushAlignmentPoint(secondExpr);

                GenerateFragmentWithAlignmentPointIfNotNull(node.SecondQueryExpression, clauseBody);

                PopAlignmentPoint();
            }

            PopAlignmentPoint();

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
