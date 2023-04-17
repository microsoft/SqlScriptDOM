//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AsymmetricKeyStatements.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateAsymmetricKeyStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateAsymmetricKeyName(node.Name);

            // owner
            GenerateOwnerIfNotNull(node.Owner);

            // key source
            if (node.KeySource != null)
            {
                NewLineAndIndent();
                GenerateSpaceAndKeyword(TSqlTokenType.From);
                GenerateSpaceAndFragmentIfNotNull(node.KeySource);
            }

            // encryption
            if (node.EncryptionAlgorithm != EncryptionAlgorithm.None)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With); 
                GenerateSpace();

                GenerateTokenAndEqualSign(CodeGenerationSupporter.Algorithm);
                GenerateSpace();
                EncryptionAlgorithmsHelper.Instance.GenerateSourceForOption(_writer, node.EncryptionAlgorithm);
            }

            // password
            if (node.Password != null)
            {
                NewLineAndIndent();
                GenerateIdentifier(CodeGenerationSupporter.Encryption);
                GenerateSpaceAndKeyword(TSqlTokenType.By);
                GenerateSpace();

                GenerateNameEqualsValue(CodeGenerationSupporter.Password, node.Password);
            }
        }

        public override void ExplicitVisit(AlterAsymmetricKeyStatement node)
        {
            GenerateKeyword(TSqlTokenType.Alter);
            GenerateAsymmetricKeyName(node.Name);

            GenerateSpace();
            switch (node.Kind)
            {
                case AlterCertificateStatementKind.RemovePrivateKey:
                    GenerateRemovePrivateKey();
                    break;
                case AlterCertificateStatementKind.RemoveAttestedOption:
                    GenerateRemoteAttestedOption();
                    break;
                case AlterCertificateStatementKind.AttestedBy:
                    GenerateAttestedBy(node.AttestedBy);
                    break;
                case AlterCertificateStatementKind.WithPrivateKey:
                    GenerateWithPrivateKey(null, node.EncryptionPassword, node.DecryptionPassword);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }

        public override void ExplicitVisit(DropAsymmetricKeyStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);

            GenerateAsymmetricKeyName(node.Name);

            GenerateRemoveProviderKeyOpt(node.RemoveProviderKey);
        }

        private void GenerateRemoveProviderKeyOpt(bool generate)
        {
            if (generate)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Remove);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Provider);
                GenerateSpaceAndKeyword(TSqlTokenType.Key);
            }
        }

        private void GenerateAsymmetricKeyName(Identifier name)
        {
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Asymmetric);
            GenerateSpaceAndKeyword(TSqlTokenType.Key);

            GenerateSpaceAndFragmentIfNotNull(name);
        }
    }
}
