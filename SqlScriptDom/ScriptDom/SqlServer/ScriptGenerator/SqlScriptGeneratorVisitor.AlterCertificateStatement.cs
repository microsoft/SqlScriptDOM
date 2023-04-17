//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterCertificateStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterCertificateStatement node)
        {
            GenerateSpaceSeparatedTokens(TSqlTokenType.Alter, CodeGenerationSupporter.Certificate);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);
            GenerateSpace();

            switch (node.Kind)
            {
                case AlterCertificateStatementKind.RemoveAttestedOption:
                    GenerateRemoteAttestedOption();
                    break;
                case AlterCertificateStatementKind.RemovePrivateKey:
                    GenerateRemovePrivateKey();
                    break;
                case AlterCertificateStatementKind.WithActiveForBeginDialog:
                    GenerateKeyword(TSqlTokenType.With);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Active);
                    GenerateSpaceAndKeyword(TSqlTokenType.For);
                    GenerateSpace();
                    GenerateOptionStateWithEqualSign(CodeGenerationSupporter.BeginDialog, node.ActiveForBeginDialog);
                    break;
                case AlterCertificateStatementKind.AttestedBy:
                    GenerateAttestedBy(node.AttestedBy);
                    break;
                case AlterCertificateStatementKind.WithPrivateKey:
                    GenerateWithPrivateKey(node.PrivateKeyPath, node.EncryptionPassword, node.DecryptionPassword);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }
    }
}
