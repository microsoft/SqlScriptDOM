//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BackupTransactionLogStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(BackupTransactionLogStatement node)
        {
            GenerateKeyword(TSqlTokenType.Backup);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Log); 

            // name
            GenerateSpaceAndFragmentIfNotNull(node.DatabaseName);

            GenerateDeviceAndOption(node);
        }
    }
}
