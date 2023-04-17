//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.UseFederationStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(UseFederationStatement node)
        {
            GenerateKeyword(TSqlTokenType.Use);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Federation);

            if (node.FederationName == null)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Root);
                GenerateSpaceAndKeyword(TSqlTokenType.With);
            }
            else
            {
                GenerateSpaceAndFragmentIfNotNull(node.FederationName);
                GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
                GenerateFragmentIfNotNull(node.DistributionName);
                GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                GenerateSpaceAndFragmentIfNotNull(node.Value);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
                GenerateSpaceAndKeyword(TSqlTokenType.With);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Filtering);
                GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                if (node.Filtering)
                {
                    GenerateSpaceAndKeyword(TSqlTokenType.On);
                }
                else
                {
                    GenerateSpaceAndKeyword(TSqlTokenType.Off);
                }
                GenerateSymbol(TSqlTokenType.Comma);
            }
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Reset);
        }
    }
}
