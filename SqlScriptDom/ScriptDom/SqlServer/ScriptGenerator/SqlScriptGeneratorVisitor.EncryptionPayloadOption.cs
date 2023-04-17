//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.EncryptionPayloadOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {

        protected static Dictionary<EndpointEncryptionSupport, TokenGenerator> _endpointEncryptionSupportGenerators = new Dictionary<EndpointEncryptionSupport, TokenGenerator>()
        {
            {EndpointEncryptionSupport.Disabled, new IdentifierGenerator(CodeGenerationSupporter.Disabled)},
            {EndpointEncryptionSupport.NotSpecified, new EmptyGenerator()},
            {EndpointEncryptionSupport.Required, new IdentifierGenerator(CodeGenerationSupporter.Required)},
            {EndpointEncryptionSupport.Supported, new IdentifierGenerator(CodeGenerationSupporter.Supported)},
        };
  
        public override void ExplicitVisit(EncryptionPayloadOption node)
        {
            // ENCRYPTION = { DISABLED | SUPPORTED | REQUIRED } 
            GenerateTokenAndEqualSign(CodeGenerationSupporter.Encryption);
            TokenGenerator generator = GetValueForEnumKey(_endpointEncryptionSupportGenerators, node.EncryptionSupport);
            if (generator != null)
            {
                GenerateSpace();
                GenerateToken(generator);
            }

            // ALGORITHM { RC4 | AES | AES RC4 | RC4 AES } 
            if (node.EncryptionSupport != EndpointEncryptionSupport.Disabled && node.EncryptionSupport != EndpointEncryptionSupport.NotSpecified)
            {
                if (node.AlgorithmPartOne != EncryptionAlgorithmPreference.NotSpecified
                    || node.AlgorithmPartTwo != EncryptionAlgorithmPreference.NotSpecified)
                {
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Algorithm); 
                }

                GenerateSpaceAndAlgorithm(node.AlgorithmPartOne);
                GenerateSpaceAndAlgorithm(node.AlgorithmPartTwo);
            }
        }

        private void GenerateSpaceAndAlgorithm(EncryptionAlgorithmPreference alg)
        {
            switch (alg)
            {
                case EncryptionAlgorithmPreference.Aes:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Aes); 
                    break;
                case EncryptionAlgorithmPreference.Rc4:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.RC4); 
                    break;
                case EncryptionAlgorithmPreference.NotSpecified:
                default:
                    break;
            }
        }
    }
}
