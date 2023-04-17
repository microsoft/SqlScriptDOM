//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.RecoveryDatabaseOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        private static Dictionary<RecoveryDatabaseOptionKind, TokenGenerator> _recoveryDatabaseOptionKindNames = 
            new Dictionary<RecoveryDatabaseOptionKind, TokenGenerator>()
        {
            // exlude: not an actual option
            // { RecoveryDatabaseOptionKind.None, CodeGenerationSupporter.None},
            { RecoveryDatabaseOptionKind.Full, new KeywordGenerator(TSqlTokenType.Full)},
            { RecoveryDatabaseOptionKind.BulkLogged, new IdentifierGenerator(CodeGenerationSupporter.BulkLogged)},
            { RecoveryDatabaseOptionKind.Simple, new IdentifierGenerator(CodeGenerationSupporter.Simple)},
        };
  
        public override void ExplicitVisit(RecoveryDatabaseOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == DatabaseOptionKind.Recovery);
            GenerateIdentifier(CodeGenerationSupporter.Recovery);
            GenerateSpace();

            TokenGenerator gen = GetValueForEnumKey(_recoveryDatabaseOptionKindNames, node.Value);
            if (gen != null)
            {
                GenerateToken(gen);
            }
        }
    }
}
