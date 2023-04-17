//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.UserLoginOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(UserLoginOption node)
        {
            switch (node.UserLoginOptionType)
            {
                case UserLoginOptionType.Login:
                    GenerateKeyword(TSqlTokenType.For);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Login);
                    GenerateSpaceAndFragmentIfNotNull(node.Identifier);
                    break;
                case UserLoginOptionType.Certificate:
                    GenerateKeyword(TSqlTokenType.For);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Certificate);
                    GenerateSpaceAndFragmentIfNotNull(node.Identifier);
                    break;
                case UserLoginOptionType.AsymmetricKey:
                    GenerateKeyword(TSqlTokenType.For);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Asymmetric);
                    GenerateSpaceAndKeyword(TSqlTokenType.Key);
                    GenerateSpaceAndFragmentIfNotNull(node.Identifier);
                    break;
                case UserLoginOptionType.External:
                    GenerateKeyword(TSqlTokenType.For);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.External);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Provider);
                    break;
                case UserLoginOptionType.WithoutLogin:
                    GenerateIdentifier(CodeGenerationSupporter.Without);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Login);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }
    }
}
