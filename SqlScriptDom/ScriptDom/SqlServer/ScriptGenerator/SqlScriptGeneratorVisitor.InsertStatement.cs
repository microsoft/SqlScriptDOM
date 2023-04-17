//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.InsertStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<InsertOption, TokenGenerator> _insertOptionGenerators =
            new Dictionary<InsertOption, TokenGenerator>()
        {
            { InsertOption.Into, new KeywordGenerator(TSqlTokenType.Into)},
            { InsertOption.None, new KeywordGenerator(TSqlTokenType.Into)},
            { InsertOption.Over, new KeywordGenerator(TSqlTokenType.Over)},
        };

        public override void ExplicitVisit(InsertStatement node)
        {
            AlignmentPoint start = new AlignmentPoint();
            AlignmentPoint clauseBody = new AlignmentPoint(ClauseBody);

            MarkAndPushAlignmentPoint(start);

            if (node.WithCtesAndXmlNamespaces != null)
            {
                GenerateFragmentWithAlignmentPointIfNotNull(node.WithCtesAndXmlNamespaces, clauseBody);

                NewLine();
            }

            GenerateFragmentIfNotNull(node.InsertSpecification);

            GenerateOptimizerHints(node.OptimizerHints);
            PopAlignmentPoint();
        }

        public override void ExplicitVisit(InsertSpecification node)
        {
            AlignmentPoint clauseBody = new AlignmentPoint(ClauseBody);
            AlignmentPoint insertColumns = new AlignmentPoint(InsertColumns);

            GenerateKeyword(TSqlTokenType.Insert);

            MarkClauseBodyAlignmentWhenNecessary(true, clauseBody);

            if (node.TopRowFilter != null)
            {
                GenerateSpaceAndFragmentIfNotNull(node.TopRowFilter);
            }

            GenerateSpaceAndInsertOption(node.InsertOption);

            if (node.Target != null)
            {
                GenerateSpace();
                // could be OpenRowsetDmlTarget
                //          SchemaObjectDmlTarget
                //          VariableDmlTarget
                GenerateFragmentWithAlignmentPointIfNotNull(node.Target, insertColumns);

                if (node.Columns.Count > 0)
                {
                    MarkInsertColumnsAlignmentPointWhenNecessary(insertColumns);
                    GenerateSpace();
                    GenerateParenthesisedCommaSeparatedList(node.Columns);
                }
            }

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
            NewLine();

            if (node.InsertSource != null)
            {
                AddAlignmentPointForFragment(node.InsertSource, clauseBody);
                AddAlignmentPointForFragment(node.InsertSource, insertColumns);

                // to avoid generate semicolon for InsertSource
                Boolean originalValue = _generateSemiColon;
                _generateSemiColon = false;

                // could be ValuesInsertSource
                //          ExecuteInsertSource
                //          SelectInsertSource
                GenerateFragmentIfNotNull(node.InsertSource);

                // restore the original value
                _generateSemiColon = originalValue;

                ClearAlignmentPointsForFragment(node.InsertSource);
            }
        }

        private void GenerateSpaceAndInsertOption(InsertOption insertOption)
        {
            if (insertOption != InsertOption.None)
            {
                TokenGenerator generator = GetValueForEnumKey(_insertOptionGenerators, insertOption);
                if (generator != null)
                {
                    GenerateSpace();
                    GenerateToken(generator);
                }
            }
        }
    }
}