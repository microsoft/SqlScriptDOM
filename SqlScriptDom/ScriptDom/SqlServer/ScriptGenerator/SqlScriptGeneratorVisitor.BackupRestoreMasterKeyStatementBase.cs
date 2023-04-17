//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BackupRestoreMasterKeyStatementBase.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateCommonRestorePart(BackupRestoreMasterKeyStatementBase node, Boolean isService)
        {
            GenerateKeyword(TSqlTokenType.Restore); 

            if (isService)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Service); 
            }

            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Master);
            GenerateSpaceAndKeyword(TSqlTokenType.Key); 

            // FROM
            GenerateSpaceAndKeyword(TSqlTokenType.From);
            GenerateSpace();
            GenerateNameEqualsValue(TSqlTokenType.File, node.File); 

            // DECRIPTION BY PASSWORD = 'password'
            GenerateSpace();
            GenerateDecryptionByPassword(node.Password);
        }
    }
}
