//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BackupDatabaseStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Diagnostics;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(BackupDatabaseStatement node)
        {
            GenerateKeyword(TSqlTokenType.Backup);
            GenerateSpaceAndKeyword(TSqlTokenType.Database);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.DatabaseName);

            if (node.Files != null && node.Files.Count > 0)
            {
                NewLineAndIndent();
                GenerateCommaSeparatedList(node.Files);
            }

            GenerateDeviceAndOption(node);
        }

        public override void ExplicitVisit(BackupOption node)
        {
            if (BackupOptionsWithValueHelper.Instance.TryGenerateSourceForOption(_writer, node.OptionKind))
            {
                if (node.Value != null)
                {
                    GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                    GenerateSpaceAndFragmentIfNotNull(node.Value);
                }
            }
            else
                BackupOptionsNoValueHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
        }

        public override void ExplicitVisit(BackupEncryptionOption node)
        {
            // BackupEncryptionOption is introduced in SQL 2014
            if (_options.SqlVersion < SqlVersion.Sql120)
            {
                return;
            }

            // Generate: ENCRYPTION '(' ALGORITHM = <algorithm_name>, SERVER (CERTIFICATE|ASYMMETRIC KEY) = <key_name> ')'

            GenerateIdentifier(CodeGenerationSupporter.Encryption);
            GenerateSymbol(TSqlTokenType.LeftParenthesis);

            GenerateIdentifier(CodeGenerationSupporter.Algorithm);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpace();
            EncryptionAlgorithmsHelper.Instance.GenerateSourceForOption(_writer, node.Algorithm);
            
            GenerateSymbol(TSqlTokenType.Comma);

            if (node.Encryptor != null)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Server);
                GenerateSpace();

                // Currently only either Certificate or AsymmetricKey is supported
                switch (node.Encryptor.CryptoMechanismType)
                {
                    case CryptoMechanismType.Certificate:
                        GenerateIdentifier(CodeGenerationSupporter.Certificate);
                        break;
                    case CryptoMechanismType.AsymmetricKey:
                        GenerateIdentifier(CodeGenerationSupporter.Asymmetric);
                        GenerateSpaceAndKeyword(TSqlTokenType.Key);
                        break;
                    default:
                        Debug.Assert(
                            false,
                            "Unexpected CryptoMechanismType value.",
                            "'{0}' is not supported value for the property 'CryptoMechanismType' of BackupEncryptionOption.Encryptor.",
                            node.Encryptor.CryptoMechanismType.ToString());
                        break;
                }

                GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                GenerateSpaceAndFragmentIfNotNull(node.Encryptor.Identifier);
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
