//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DerivedTable.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(QueryDerivedTable node)
        {
            GenerateQueryExpressionInParentheses(node.QueryExpression);

            GenerateTableAndColumnAliases(node);
        }

        public override void ExplicitVisit(InlineDerivedTable node)
        {
            GenerateSymbol(TSqlTokenType.LeftParenthesis);
            GenerateSymbolAndSpace(TSqlTokenType.Values);
            GenerateCommaSeparatedList(node.RowValues);
            GenerateSymbol(TSqlTokenType.RightParenthesis);

            GenerateTableAndColumnAliases(node);
        }
    }
}
