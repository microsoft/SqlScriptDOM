//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.QuerySpecification.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(QuerySpecification node)
        {
            AlignmentPoint clauseBody = GetAlignmentPointForFragment(node, ClauseBody);
            GenerateQuerySpecification(node, clauseBody, null, null);
        }

        protected void GenerateQuerySpecification(QuerySpecification node, AlignmentPoint clauseBody, SchemaObjectName intoClause, Identifier filegroupClause = null)
        {
            AlignmentPoint start = new AlignmentPoint();
            MarkAndPushAlignmentPoint(start);

#if !PIMODLANGUAGE
            Debug.Assert(clauseBody != null, "Alignment point for clause body is null");
#endif

            GenerateKeyword(TSqlTokenType.Select);

            MarkClauseBodyAlignmentWhenNecessary(true, clauseBody);

            GenerateUniqueRowFilter(node.UniqueRowFilter, true);

            GenerateSpaceAndFragmentIfNotNull(node.TopRowFilter);

            GenerateSpace();
            GenerateSelectElementsList(node.SelectElements);

            if (intoClause != null)
            {
                NewLine();
                GenerateKeyword(TSqlTokenType.Into);
                MarkClauseBodyAlignmentWhenNecessary(true, clauseBody);
                GenerateSpaceAndFragmentIfNotNull(intoClause);
            }

            if (filegroupClause != null)
            {
                NewLine();
                GenerateKeyword(TSqlTokenType.On);
                MarkClauseBodyAlignmentWhenNecessary(true, clauseBody);
                GenerateSpaceAndFragmentIfNotNull(filegroupClause);
            }

            GenerateFromClause(node.FromClause, clauseBody);

            if (node.WhereClause != null)
            {
                GenerateSeparatorForWhereClause();

                GenerateFragmentWithAlignmentPointIfNotNull(node.WhereClause, clauseBody);
            }

            if (node.GroupByClause != null)
            {
                GenerateSeparatorForGroupByClause();

                GenerateFragmentWithAlignmentPointIfNotNull(node.GroupByClause, clauseBody);
            }

            if (node.HavingClause != null)
            {
                GenerateSeparatorForHavingClause();

                GenerateFragmentWithAlignmentPointIfNotNull(node.HavingClause, clauseBody);
            }

            if (node.WindowClause != null)
            {
                GenerateSeparatorForWindowClause();

                GenerateFragmentWithAlignmentPointIfNotNull(node.WindowClause, clauseBody);
            }

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

            PopAlignmentPoint();
        }
    }
}
