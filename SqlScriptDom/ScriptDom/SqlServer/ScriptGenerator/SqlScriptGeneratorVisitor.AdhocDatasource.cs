//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AdhocDatasource.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AdHocDataSource node)
        {
            GenerateKeyword(TSqlTokenType.OpenDataSource);
            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.ProviderName);
            GenerateSymbolAndSpace(TSqlTokenType.Comma);
            GenerateFragmentIfNotNull(node.InitString);
            GenerateSymbol(TSqlTokenType.RightParenthesis); 
        }
    }
}
