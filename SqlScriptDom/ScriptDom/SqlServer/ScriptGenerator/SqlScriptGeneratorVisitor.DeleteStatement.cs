//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DeleteStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DeleteStatement node)
        {
            AlignmentPoint start = new AlignmentPoint();
            AlignmentPoint clauseBody = new AlignmentPoint(ClauseBody);

            MarkAndPushAlignmentPoint(start);

            if (node.WithCtesAndXmlNamespaces != null)
            {
                GenerateFragmentWithAlignmentPointIfNotNull(node.WithCtesAndXmlNamespaces, clauseBody);

                NewLine();
            }
            GenerateFragmentIfNotNull(node.DeleteSpecification);

            GenerateOptimizerHints(node.OptimizerHints);

            PopAlignmentPoint();
        }

        public override void ExplicitVisit(DeleteSpecification node)
        {
            AlignmentPoint clauseBody = new AlignmentPoint(ClauseBody);

            GenerateKeyword(TSqlTokenType.Delete);

            if (node.TopRowFilter != null)
            {
                MarkClauseBodyAlignmentWhenNecessary(true, clauseBody);
                GenerateSpaceAndFragmentIfNotNull(node.TopRowFilter);
                NewLine(); // Put the FROM on the next line if we had a TOP clause
            }

            MarkClauseBodyAlignmentWhenNecessary(true, clauseBody);
            GenerateSpaceAndFragmentIfNotNull(node.Target);

            if (node.OutputIntoClause != null)
            {
                GenerateSeparatorForOutputClause();

                GenerateFragmentWithAlignmentPointIfNotNull(node.OutputIntoClause, clauseBody);
            }

            if (node.OutputClause != null)
            {
                GenerateSeparatorForOutputClause();

                GenerateFragmentWithAlignmentPointIfNotNull(node.OutputClause, clauseBody);
            }

            GenerateFromClause(node.FromClause, clauseBody);

            if (node.WhereClause != null)
            {
                GenerateSeparatorForWhereClause();

                GenerateFragmentWithAlignmentPointIfNotNull(node.WhereClause, clauseBody);
            }
        }
    }
}