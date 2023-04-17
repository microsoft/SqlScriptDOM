//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.TSEqualCall.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(TSEqualCall node)
        {
            GenerateKeyword(TSqlTokenType.TSEqual);

            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.FirstExpression);
            GenerateSymbol(TSqlTokenType.Comma);
            GenerateSpaceAndFragmentIfNotNull(node.SecondExpression);
            GenerateSymbol(TSqlTokenType.RightParenthesis); 
        }
    }
}
