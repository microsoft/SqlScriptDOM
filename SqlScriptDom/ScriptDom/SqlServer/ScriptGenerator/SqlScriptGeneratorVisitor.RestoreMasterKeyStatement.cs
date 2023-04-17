//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.RestoreMasterKeyStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(RestoreMasterKeyStatement node)
        {
            // RESTORE MASTER KEY FROM FILE = 'file' DECRYPTION BY PASSWORD = 'password'
            GenerateCommonRestorePart(node, false);

            // ENCRYPTION BY PASSWORD = 'password'
            GenerateSpace();
            GenerateEncryptionByPassword(node.EncryptionPassword);

            // FORCE
            if (node.IsForce)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Force); 
            }
        }
    }
}
