//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BooleanComparisonExpression.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(BooleanComparisonExpression node)
        {
            AlignmentPoint start = new AlignmentPoint();
            MarkAndPushAlignmentPoint(start);

            GenerateFragmentIfNotNull(node.FirstExpression);

            GenerateSpace();

            GenerateBinaryOperator(node.ComparisonType);

            GenerateSpaceAndFragmentIfNotNull(node.SecondExpression);

            PopAlignmentPoint();
        }
    }
}
