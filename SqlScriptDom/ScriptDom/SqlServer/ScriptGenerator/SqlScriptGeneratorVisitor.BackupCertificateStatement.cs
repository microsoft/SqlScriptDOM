//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BackupCertificateStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(BackupCertificateStatement node)
        {
            GenerateKeyword(TSqlTokenType.Backup);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Certificate);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            GenerateSpaceAndKeyword(TSqlTokenType.To); 

            GenerateSpace();
            GenerateNameEqualsValue(TSqlTokenType.File, node.File); 

            if (node.PrivateKeyPath != null || node.DecryptionPassword != null || node.EncryptionPassword != null)
            {
                NewLineAndIndent();
                GenerateWithPrivateKey(node.PrivateKeyPath, node.EncryptionPassword, node.DecryptionPassword);
            }
        }
    }
}
