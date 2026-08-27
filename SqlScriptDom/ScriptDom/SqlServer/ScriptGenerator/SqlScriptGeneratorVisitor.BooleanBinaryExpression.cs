//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BinaryExpression.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        private Boolean? _multilinePredicates;

        public override void ExplicitVisit(BooleanBinaryExpression node)
        {
            AlignmentPoint start = new AlignmentPoint();
            MarkAndPushAlignmentPoint(start);

            GenerateFragmentIfNotNull(node.FirstExpression);

            Boolean insertNewline = RightPredicateOnNewline(node);

            GenerateNewLineOrSpace(insertNewline);

            GenerateBinaryOperator(node.BinaryExpressionType);

            GenerateSpaceAndFragmentIfNotNull(node.SecondExpression);

            PopAlignmentPoint();
        }

        private Boolean RightPredicateOnNewline(BooleanBinaryExpression node)
        {
            // If:
            //  * Multi-Line Where Predicates are enabled, AND
            //  * A Newline was inserted before the WHERE clause, AND
            //  * The Binary Expression is an AND or an OR expression.
            Boolean insertNewline =
                    (_multilinePredicates ??
                        (_options.MultilineWherePredicatesList && _options.NewLineBeforeWhereClause)) &&
                    (node.BinaryExpressionType == BooleanBinaryExpressionType.And || node.BinaryExpressionType == BooleanBinaryExpressionType.Or);

            return insertNewline;
        }

        private void GeneratePredicate(TSqlFragment predicate, Boolean multiline)
        {
            Boolean? previousMultilinePredicates = _multilinePredicates;
            _multilinePredicates = multiline;

            try
            {
                GenerateFragmentIfNotNull(predicate);
            }
            finally
            {
                _multilinePredicates = previousMultilinePredicates;
            }
        }

    }
}
