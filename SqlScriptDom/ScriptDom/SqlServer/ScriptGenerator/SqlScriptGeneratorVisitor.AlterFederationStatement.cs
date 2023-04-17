//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterFederationStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterFederationStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Alter);
            GenerateIdentifier(CodeGenerationSupporter.Federation);

            GenerateSpaceAndFragmentIfNotNull(node.Name);

            switch (node.Kind)
            {
                case AlterFederationKind.Split:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Split);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.At);
                    GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
                    break;

                case AlterFederationKind.DropLow:
                    GenerateSpaceAndKeyword(TSqlTokenType.Drop);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.At);
                    GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
                    GenerateIdentifier(CodeGenerationSupporter.Low);
                    GenerateSpace();
                    break;

                case AlterFederationKind.DropHigh:
                    GenerateSpaceAndKeyword(TSqlTokenType.Drop);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.At);
                    GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
                    GenerateIdentifier(CodeGenerationSupporter.High);
                    GenerateSpace();
                    break;
            }

            GenerateFragmentIfNotNull(node.DistributionName);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpaceAndFragmentIfNotNull(node.Boundary);
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
