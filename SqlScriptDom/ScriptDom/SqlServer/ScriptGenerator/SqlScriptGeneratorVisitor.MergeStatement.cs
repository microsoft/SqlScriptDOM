//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterAuthorizationStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(MergeStatement node)
        {
            AlignmentPoint start = new AlignmentPoint();
            AlignmentPoint clauseBody = new AlignmentPoint(ClauseBody);
            //AlignmentPoint insertColumns = new AlignmentPoint(InsertColumns);

            MarkAndPushAlignmentPoint(start);

            if (node.WithCtesAndXmlNamespaces != null)
            {
                GenerateFragmentWithAlignmentPointIfNotNull(node.WithCtesAndXmlNamespaces, clauseBody);

                NewLine();
            }

            GenerateFragmentIfNotNull(node.MergeSpecification);

            GenerateOptimizerHints(node.OptimizerHints);

            PopAlignmentPoint();

        }

        public override void ExplicitVisit(MergeSpecification node)
        {
            AlignmentPoint clauseBody = new AlignmentPoint(ClauseBody);

            GenerateKeyword(TSqlTokenType.Merge);

            MarkClauseBodyAlignmentWhenNecessary(false, clauseBody);

            if (node.TopRowFilter != null)
            {
                GenerateSpaceAndFragmentIfNotNull(node.TopRowFilter);
            }

            GenerateSpace();
            GenerateKeyword(TSqlTokenType.Into);

            if (node.Target != null)
            {
                GenerateSpace();
                // could be OpenRowsetDmlTarget
                //          SchemaObjectDmlTarget
                //          VariableDmlTarget
                GenerateFragmentIfNotNull(node.Target);
                NewLine();
            }

            if (node.TableAlias != null)
            {
                GenerateSpace();
                GenerateKeyword(TSqlTokenType.As);
                GenerateSpace();
                GenerateFragmentIfNotNull(node.TableAlias);
            }
            NewLine();

            GenerateIdentifier(CodeGenerationSupporter.Using);
            GenerateSpace();
            GenerateFragmentIfNotNull(node.TableReference);
            GenerateSpace();
            GenerateKeyword(TSqlTokenType.On);
            GenerateSpace();
            GenerateFragmentIfNotNull(node.SearchCondition);
            NewLine();

            if (node.ActionClauses != null)
                GenerateList(node.ActionClauses, delegate() { NewLine(); });

            if (node.OutputIntoClause != null)
            {
                GenerateSeparatorForOutputClause();

                GenerateFragmentWithAlignmentPointIfNotNull(node.OutputIntoClause, clauseBody);
            }

            if (node.OutputClause != null)
            {
                AddAlignmentPointForFragment(node.OutputClause, clauseBody);
                GenerateSpace();
                GenerateFragmentIfNotNull(node.OutputClause);
            }
        }

        public override void ExplicitVisit(UpdateMergeAction node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Update);

            AlignmentPoint clauseBody = new AlignmentPoint(ClauseBody);
            GenerateSetClauses(node.SetClauses, clauseBody);
        }

        public override void ExplicitVisit(DeleteMergeAction node)
        {
            GenerateKeyword(TSqlTokenType.Delete);
        }

        public override void ExplicitVisit(InsertMergeAction node)
        {
            AlignmentPoint insertColumns = new AlignmentPoint(InsertColumns);
            GenerateKeyword(TSqlTokenType.Insert);
            AlignmentPoint clauseBody = new AlignmentPoint(ClauseBody);
            AddAlignmentPointForFragment(node.Source, clauseBody);
            if (node.Columns.Count > 0)
            {
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.Columns);
            }

            if (node.Source != null)
            {
                GenerateSpace();
                GenerateFragmentWithAlignmentPointIfNotNull(node.Source, insertColumns);
            }
        }

    }
}