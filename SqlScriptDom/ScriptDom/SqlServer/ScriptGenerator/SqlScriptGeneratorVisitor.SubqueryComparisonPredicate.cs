//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SubqueryComparisonPredicate.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<SubqueryComparisonPredicateType, TokenGenerator> _subqueryComparisonPredicateTypeGenerators = 
            new Dictionary<SubqueryComparisonPredicateType, TokenGenerator>()
        {
            // handled specially
            //{SubqueryComparisonPredicateType.None, new EmptyGenerator()},
            {SubqueryComparisonPredicateType.All, new KeywordGenerator(TSqlTokenType.All)},
            {SubqueryComparisonPredicateType.Any, new KeywordGenerator(TSqlTokenType.Any)},
        };
  
        public override void ExplicitVisit(SubqueryComparisonPredicate node)
        {
            AlignmentPoint expression = new AlignmentPoint();
            MarkAndPushAlignmentPoint(expression);
            GenerateFragmentIfNotNull(node.Expression);
            PopAlignmentPoint();

            GenerateSpace();
            GenerateBinaryOperator(node.ComparisonType);

            if (node.SubqueryComparisonPredicateType != SubqueryComparisonPredicateType.None)
            {
                TokenGenerator generator = GetValueForEnumKey(_subqueryComparisonPredicateTypeGenerators, node.SubqueryComparisonPredicateType);
                if (generator != null)
                {
                    GenerateSpace();
                    GenerateToken(generator);
                }
            }

            AlignmentPoint query = new AlignmentPoint();
            MarkAndPushAlignmentPoint(query);
            GenerateSpaceAndFragmentIfNotNull(node.Subquery);
            PopAlignmentPoint();
        }
    }
}
