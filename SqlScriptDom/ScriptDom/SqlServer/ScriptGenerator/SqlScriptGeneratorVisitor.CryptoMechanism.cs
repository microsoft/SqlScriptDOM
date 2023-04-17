//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CryptoMechanism.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CryptoMechanism node)
        {
            switch (node.CryptoMechanismType)
            {
                case CryptoMechanismType.Certificate:
                    GenerateIdentifier(CodeGenerationSupporter.Certificate);
                    GenerateIdentifierWithPassword(node);
                    break;
                case CryptoMechanismType.AsymmetricKey:
                    GenerateIdentifier(CodeGenerationSupporter.Asymmetric);
                    GenerateSpaceAndKeyword(TSqlTokenType.Key);
                    GenerateIdentifierWithPassword(node);
                    break;
                case CryptoMechanismType.SymmetricKey:
                    GenerateIdentifier(CodeGenerationSupporter.Symmetric);
                    GenerateSpaceAndKeyword(TSqlTokenType.Key);
                    GenerateIdentifierWithPassword(node);
                    break;
                case CryptoMechanismType.Password:
                    GenerateNameEqualsValue(CodeGenerationSupporter.Password, node.PasswordOrSignature);
                    break;
            }
        }

        private void GenerateIdentifierWithPassword(CryptoMechanism node)
        {
            GenerateSpaceAndFragmentIfNotNull(node.Identifier);
            
            if (node.PasswordOrSignature != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With);
                String name = 
                    node.PasswordOrSignature.LiteralType == LiteralType.Binary 
                    ? CodeGenerationSupporter.Signature 
                    : CodeGenerationSupporter.Password;
                GenerateSpace(); 
                GenerateNameEqualsValue(name, node.PasswordOrSignature);
            }
        }
    }
}
