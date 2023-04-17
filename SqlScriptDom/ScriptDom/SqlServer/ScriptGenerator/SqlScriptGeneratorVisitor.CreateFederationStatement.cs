//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateFederationStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateFederationStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Create);
            GenerateIdentifier(CodeGenerationSupporter.Federation);

            GenerateSpaceAndFragmentIfNotNull(node.Name);

            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.DistributionName);
            GenerateSpaceAndFragmentIfNotNull(node.DataType);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Range);
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
