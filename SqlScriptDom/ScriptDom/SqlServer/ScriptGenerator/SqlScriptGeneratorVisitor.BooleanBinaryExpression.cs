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
        public override void ExplicitVisit(BooleanBinaryExpression node)
        {
            AlignmentPoint start = new AlignmentPoint();
            MarkAndPushAlignmentPoint(start);

            GenerateFragmentIfNotNull(node.FirstExpression);

            Boolean insertNewline = RightPredicateOnNewline(node);

            GenerateNewLineOrSpace(insertNewline && _options.NewLineBeforeBinaryBooleanExpresson);

            GenerateBinaryOperator(node.BinaryExpressionType);

            GenerateNewLineOrSpace(insertNewline && _options.NewLineAfterBinaryBooleanExpresson);

            GenerateFragmentIfNotNull(node.SecondExpression);

            PopAlignmentPoint();
        }

        private Boolean RightPredicateOnNewline(BooleanBinaryExpression node)
        {
            // If:
            //  * Multi-Line Where Predicates are enabled, AND
            //  * A Newline was inserted before the WHERE clause, AND
            //  * The Binary Expression is an AND or an OR expression.
            Boolean insertNewline =
                    _options.MultilineWherePredicatesList &&
                    _options.NewLineBeforeWhereClause &&
                    (node.BinaryExpressionType == BooleanBinaryExpressionType.And || node.BinaryExpressionType == BooleanBinaryExpressionType.Or);

            return insertNewline;
        }

    }
}
