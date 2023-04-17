//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.RestoreServiceMasterKeyStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(RestoreServiceMasterKeyStatement node)
        {
            // RESTORE SERVICE MASTER KEY FROM FILE = 'file' DECRYPTION BY PASSWORD = 'password'
            GenerateCommonRestorePart(node, true);

            // FORCE
            if (node.IsForce)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Force); 
            }
        }
    }
}
