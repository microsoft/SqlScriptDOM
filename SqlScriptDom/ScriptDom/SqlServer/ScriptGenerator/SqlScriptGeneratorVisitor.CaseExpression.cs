//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CaseExpression.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(SimpleCaseExpression node)
        {
            bool multiline = BeginCaseExpression();

            GenerateSpaceAndFragmentIfNotNull(node.InputExpression);

            GenerateCaseBody(node.WhenClauses, node.ElseExpression, multiline);

            GenerateSpaceAndCollation(node.Collation);
        }

        public override void ExplicitVisit(SearchedCaseExpression node)
        {
            bool multiline = BeginCaseExpression();

            GenerateCaseBody(node.WhenClauses, node.ElseExpression, multiline);

            GenerateSpaceAndCollation(node.Collation);
        }

        // Writes CASE. With MultilineCaseExpression it first pushes an alignment point at the CASE
        // keyword, which GenerateCaseBody pops after END, so that every line of the body starts
        // from the CASE column. Without it no point is pushed, which keeps the single-line output
        // unchanged.
        private bool BeginCaseExpression()
        {
            bool multiline = _options.MultilineCaseExpression;

            if (multiline)
            {
                MarkAndPushAlignmentPoint(new AlignmentPoint());
            }

            GenerateKeyword(TSqlTokenType.Case);

            return multiline;
        }

        private void GenerateCaseBody<TWhenClause>(IList<TWhenClause> whenClauses, ScalarExpression elseExpression, bool multiline)
            where TWhenClause : WhenClause
        {
            foreach (TWhenClause when in whenClauses)
            {
                GenerateCaseBodyLineStart(multiline);
                GenerateFragmentIfNotNull(when);
            }

            if (elseExpression != null)
            {
                GenerateCaseBodyLineStart(multiline);
                GenerateKeyword(TSqlTokenType.Else);
                GenerateSpaceAndFragmentIfNotNull(elseExpression);
            }

            if (multiline)
            {
                NewLine();
                GenerateKeyword(TSqlTokenType.End);
                PopAlignmentPoint();
            }
            else
            {
                GenerateSpaceAndKeyword(TSqlTokenType.End);
            }
        }

        private void GenerateCaseBodyLineStart(bool multiline)
        {
            if (multiline)
            {
                NewLineAndIndent();
            }
            else
            {
                GenerateSpace();
            }
        }
    }
}
