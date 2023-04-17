//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CaseExpression.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(SimpleCaseExpression node)
        {
            GenerateKeyword(TSqlTokenType.Case);

            GenerateSpaceAndFragmentIfNotNull(node.InputExpression);

            foreach (SimpleWhenClause when in node.WhenClauses)
            {
                GenerateSpaceAndFragmentIfNotNull(when);
            }

            if (node.ElseExpression != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Else);
                GenerateSpaceAndFragmentIfNotNull(node.ElseExpression);
            }

            GenerateSpaceAndKeyword(TSqlTokenType.End);

            GenerateSpaceAndCollation(node.Collation);
        }

        public override void ExplicitVisit(SearchedCaseExpression node)
        {
            GenerateKeyword(TSqlTokenType.Case);

            foreach (SearchedWhenClause when in node.WhenClauses)
            {
                GenerateSpaceAndFragmentIfNotNull(when);
            }

            if (node.ElseExpression != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Else);
                GenerateSpaceAndFragmentIfNotNull(node.ElseExpression);
            }

            GenerateSpaceAndKeyword(TSqlTokenType.End);

            GenerateSpaceAndCollation(node.Collation);
        }

    }
}
