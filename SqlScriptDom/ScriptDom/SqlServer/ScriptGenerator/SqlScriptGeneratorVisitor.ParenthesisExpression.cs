//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ParenthesisExpression.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ParenthesisExpression node)
        {
            GenerateSymbol(TSqlTokenType.LeftParenthesis); 
            GenerateFragmentIfNotNull(node.Expression);
            GenerateSymbol(TSqlTokenType.RightParenthesis);

			GenerateSpaceAndCollation(node.Collation);
        }
    }
}
