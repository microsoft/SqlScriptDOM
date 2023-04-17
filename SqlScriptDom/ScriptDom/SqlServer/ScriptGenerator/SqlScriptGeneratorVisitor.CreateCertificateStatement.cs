//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateCertificateStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.ObjectModel;

using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateCertificateStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Certificate);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // owner
            GenerateOwnerIfNotNull(node.Owner);

            // certificate source: FROM existing keys
            if (node.CertificateSource != null)
            {
                NewLineAndIndent();
                GenerateKeywordAndSpace(TSqlTokenType.From);
                GenerateFragmentIfNotNull(node.CertificateSource);

                // WITH PRIVATE KEY
                if (node.PrivateKeyPath != null)
                {
                    NewLineAndIndent();
                    GenerateKeyword(TSqlTokenType.With);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Private);
                    GenerateSpaceAndKeyword(TSqlTokenType.Key);

                    GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);

                    GenerateNameEqualsValue(TSqlTokenType.File, node.PrivateKeyPath);

                    if (node.DecryptionPassword != null)
                    {
                        GenerateSymbol(TSqlTokenType.Comma);
                        GenerateSpaceAndIdentifier(CodeGenerationSupporter.Decryption);
                        GenerateSpaceAndKeyword(TSqlTokenType.By);

                        GenerateSpace();
                        GenerateNameEqualsValue(CodeGenerationSupporter.Password, node.DecryptionPassword);
                    }

                    if (node.EncryptionPassword != null)
                    {
                        GenerateSymbol(TSqlTokenType.Comma);
                        GenerateSpace();
                        GenerateEncryptionPassword(node.EncryptionPassword);
                    }

                    GenerateSymbol(TSqlTokenType.RightParenthesis);
                }
            }
            else // generate new keys
            {
                // if we have an encryption password, generate it
                if (node.EncryptionPassword != null)
                {
                    NewLineAndIndent();
                    GenerateEncryptionPassword(node.EncryptionPassword);
                }

                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpace();

                GenerateCommaSeparatedList(node.CertificateOptions);
            }

            if (node.ActiveForBeginDialog != OptionState.NotSet)
            {
                NewLineAndIndent();
                GenerateIdentifier(CodeGenerationSupporter.Active);
                GenerateSpaceAndKeyword(TSqlTokenType.For);
                GenerateSpace();

                GenerateOptionStateWithEqualSign(CodeGenerationSupporter.BeginDialog, node.ActiveForBeginDialog);
            }
        }

        private void GenerateEncryptionPassword(Literal password)
        {
            if (password != null)
            {
                GenerateIdentifier(CodeGenerationSupporter.Encryption);
                GenerateSpaceAndKeyword(TSqlTokenType.By);

                GenerateSpace();
                GenerateNameEqualsValue(CodeGenerationSupporter.Password, password);
            }
        }
    }
}
