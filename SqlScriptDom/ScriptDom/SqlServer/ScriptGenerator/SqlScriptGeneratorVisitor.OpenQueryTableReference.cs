//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.OpenQueryTableSource.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(OpenQueryTableReference node)
        {
            GenerateKeyword(TSqlTokenType.OpenQuery);

            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis); 
            GenerateFragmentIfNotNull(node.LinkedServer);
            GenerateSymbol(TSqlTokenType.Comma);

            GenerateSpaceAndFragmentIfNotNull(node.Query);
            GenerateSymbol(TSqlTokenType.RightParenthesis);

            GenerateSpaceAndAlias(node.Alias);
        }
    }
}
