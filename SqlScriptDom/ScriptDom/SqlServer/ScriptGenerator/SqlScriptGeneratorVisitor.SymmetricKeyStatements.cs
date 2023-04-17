//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateSymmetricKeyStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateSymmetricKeyStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);

            GenerateSymmetricKeyName(node.Name);

            // owner
            GenerateOwnerIfNotNull(node.Owner);

            if (node.Provider != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.From);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Provider);
                GenerateSpaceAndFragmentIfNotNull(node.Provider);
            }

            // key_options
            if (node.KeyOptions.Count > 0)
            {
                NewLineAndIndent();
                GenerateKeywordAndSpace(TSqlTokenType.With);
                GenerateCommaSeparatedList(node.KeyOptions);
            }

            // encryption by
            if (node.EncryptingMechanisms.Count > 0)
            {
                NewLineAndIndent();
                GenerateIdentifier(CodeGenerationSupporter.Encryption);
                GenerateSpaceAndKeyword(TSqlTokenType.By);
                GenerateSpace();

                GenerateCommaSeparatedList(node.EncryptingMechanisms);
            }
        }

        public override void ExplicitVisit(AlterSymmetricKeyStatement node)
        {
            GenerateKeyword(TSqlTokenType.Alter);

            GenerateSymmetricKeyName(node.Name);

            GenerateSpaceAndKeyword(node.IsAdd ? TSqlTokenType.Add : TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Encryption);
            GenerateSpaceAndKeyword(TSqlTokenType.By);

            GenerateSpace();
            GenerateCommaSeparatedList(node.EncryptingMechanisms);
        }

        public override void ExplicitVisit(DropSymmetricKeyStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSymmetricKeyName(node.Name);

            GenerateRemoveProviderKeyOpt(node.RemoveProviderKey);
        }

        public override void ExplicitVisit(AlgorithmKeyOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == KeyOptionKind.Algorithm);
            GenerateTokenAndEqualSign(CodeGenerationSupporter.Algorithm);
            GenerateSpace();
            EncryptionAlgorithmsHelper.Instance.GenerateSourceForOption(_writer, node.Algorithm);
        }

        public override void ExplicitVisit(IdentityValueKeyOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == KeyOptionKind.IdentityValue);
            GenerateNameEqualsValue(CodeGenerationSupporter.IdentityValue, node.IdentityPhrase);
        }

        public override void ExplicitVisit(KeySourceKeyOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == KeyOptionKind.KeySource);
            GenerateNameEqualsValue(CodeGenerationSupporter.KeySource, node.PassPhrase);
        }

        public override void ExplicitVisit(ProviderKeyNameKeyOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == KeyOptionKind.ProviderKeyName);
            GenerateNameEqualsValue(CodeGenerationSupporter.ProviderKeyName, node.KeyName);
        }

        public override void ExplicitVisit(CreationDispositionKeyOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == KeyOptionKind.CreationDisposition);
            GenerateTokenAndEqualSign(CodeGenerationSupporter.CreationDisposition);
            GenerateSpaceAndIdentifier(node.IsCreateNew ? 
                CodeGenerationSupporter.CreateNew : CodeGenerationSupporter.OpenExisting);
        }

        private void GenerateSymmetricKeyName(Identifier name)
        {
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Symmetric);
            GenerateSpaceAndKeyword(TSqlTokenType.Key);
            GenerateSpaceAndFragmentIfNotNull(name);
        }
    }
}
