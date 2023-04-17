//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.NullIfExpression.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(NullIfExpression node)
        {
            GenerateKeyword(TSqlTokenType.NullIf);

            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.FirstExpression);
            GenerateSymbolAndSpace(TSqlTokenType.Comma);
            GenerateFragmentIfNotNull(node.SecondExpression);
            GenerateSymbol(TSqlTokenType.RightParenthesis);

			GenerateSpaceAndCollation(node.Collation);
        }
    }
}
