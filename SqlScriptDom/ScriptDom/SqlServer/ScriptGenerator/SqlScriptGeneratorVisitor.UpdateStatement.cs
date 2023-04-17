//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.UpdateStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(UpdateStatement node)
        {
            AlignmentPoint start = new AlignmentPoint();
            AlignmentPoint clauseBody = new AlignmentPoint(ClauseBody);

            MarkAndPushAlignmentPoint(start);

            if (node.WithCtesAndXmlNamespaces != null)
            {
                GenerateFragmentWithAlignmentPointIfNotNull(node.WithCtesAndXmlNamespaces, clauseBody);

                NewLine();
            }

            GenerateFragmentIfNotNull(node.UpdateSpecification);

            GenerateOptimizerHints(node.OptimizerHints);

            PopAlignmentPoint();
        }

        public override void ExplicitVisit(UpdateSpecification node)
        {
            AlignmentPoint clauseBody = new AlignmentPoint(ClauseBody);
            GenerateKeyword(TSqlTokenType.Update);

			MarkClauseBodyAlignmentWhenNecessary(true, clauseBody);

            if (node.TopRowFilter != null)
            {
                GenerateSpaceAndFragmentIfNotNull(node.TopRowFilter);
                NewLine();
            }

            // could be OpenRowsetDmlTarget
            //          SchemaObjectDmlTarget
            //          VariableDmlTarget
            GenerateSpaceAndFragmentIfNotNull(node.Target);

            GenerateSetClauses(node.SetClauses, clauseBody);

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