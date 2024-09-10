//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.GroupByClause.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(GroupByClause node)
        {
            AlignmentPoint start = new AlignmentPoint();
            MarkAndPushAlignmentPoint(start);

            GenerateKeyword(TSqlTokenType.Group);
            GenerateSpaceAndKeyword(TSqlTokenType.By);

            if (_options.NewLineAfterGroupByKeyword)
            {
                NewLineAndIndent();
            }

            if (node.All)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.All);
            }

            AlignmentPoint clauseBody = GetAlignmentPointForFragment(node, ClauseBody);
            MarkClauseBodyAlignmentWhenNecessary(_options.NewLineBeforeGroupByClause, clauseBody);

            GenerateSpace();
            GenerateCommaSeparatedList(node.GroupingSpecifications);

            if (node.GroupByOption != GroupByOption.None)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With); 
                GenerateSpace();
                GroupByOptionHelper.Instance.GenerateSourceForOption(_writer, node.GroupByOption);
            }

            PopAlignmentPoint();
        }

        public override void ExplicitVisit(ExpressionGroupingSpecification node)
        {
            GenerateFragmentIfNotNull(node.Expression);

            if (node.DistributedAggregation)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With);
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateIdentifier(CodeGenerationSupporter.DistributedAgg);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
        }

        public override void ExplicitVisit(CompositeGroupingSpecification node)
        {
            GenerateParenthesisedCommaSeparatedList(node.Items);
        }

        public override void ExplicitVisit(CubeGroupingSpecification node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Cube);
            GenerateParenthesisedCommaSeparatedList(node.Arguments);
        }

        public override void ExplicitVisit(RollupGroupingSpecification node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Rollup);
            GenerateParenthesisedCommaSeparatedList(node.Arguments);
        }

        public override void ExplicitVisit(GrandTotalGroupingSpecification node)
        {
            GenerateSymbol(TSqlTokenType.LeftParenthesis);
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }

        public override void ExplicitVisit(GroupingSetsGroupingSpecification node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Grouping);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Sets);
            GenerateParenthesisedCommaSeparatedList(node.Sets);
        }
    }
}
