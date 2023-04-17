//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BooleanParenthesisExpression.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(BooleanParenthesisExpression node)
        {
            GenerateSymbol(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.Expression);
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
