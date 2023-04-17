//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.WhenClause.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(SimpleWhenClause node)
        {
            GenerateKeyword(TSqlTokenType.When);
            GenerateSpaceAndFragmentIfNotNull(node.WhenExpression);
            GenerateSpaceAndKeyword(TSqlTokenType.Then);
            GenerateSpaceAndFragmentIfNotNull(node.ThenExpression);
        }

        public override void ExplicitVisit(SearchedWhenClause node)
        {
            GenerateKeyword(TSqlTokenType.When);
            GenerateSpaceAndFragmentIfNotNull(node.WhenExpression);
            GenerateSpaceAndKeyword(TSqlTokenType.Then);
            GenerateSpaceAndFragmentIfNotNull(node.ThenExpression);
        }

    }
}
