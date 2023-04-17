//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BackupMasterKeyStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(BackupMasterKeyStatement node)
        {
            GenerateKeyword(TSqlTokenType.Backup);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Master);
            GenerateSpaceAndKeyword(TSqlTokenType.Key);
            GenerateSpaceAndKeyword(TSqlTokenType.To);

            GenerateSpace();
            GenerateNameEqualsValue(TSqlTokenType.File, node.File);

            GenerateSpace();
            GenerateEncryptionByPassword(node.Password);
        }
    }
}
