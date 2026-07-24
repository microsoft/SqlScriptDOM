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
                // The federation name must be a plain identifier; it cannot be bracketed or recased.
                // (The distribution name below is a normal identifier and is still transformed.)
                GenerateWithoutIdentifierFormatting(() => GenerateSpaceAndFragmentIfNotNull(node.FederationName));
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
