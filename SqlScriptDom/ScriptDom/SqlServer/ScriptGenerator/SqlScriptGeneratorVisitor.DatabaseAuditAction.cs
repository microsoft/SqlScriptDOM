//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DatabaseAuditAction.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        private static Dictionary<DatabaseAuditActionKind, TokenGenerator> _databaseAuditActionName = new Dictionary<DatabaseAuditActionKind, TokenGenerator>()
        {
            { DatabaseAuditActionKind.Select, new KeywordGenerator(TSqlTokenType.Select) },
            { DatabaseAuditActionKind.Update, new KeywordGenerator(TSqlTokenType.Update) },
            { DatabaseAuditActionKind.Insert, new KeywordGenerator(TSqlTokenType.Insert) },
            { DatabaseAuditActionKind.Delete, new KeywordGenerator(TSqlTokenType.Delete) },
            { DatabaseAuditActionKind.Execute, new KeywordGenerator(TSqlTokenType.Execute) },
            { DatabaseAuditActionKind.Receive, new IdentifierGenerator(CodeGenerationSupporter.Receive) },
            { DatabaseAuditActionKind.References, new KeywordGenerator(TSqlTokenType.References) }
        };

        public override void ExplicitVisit(DatabaseAuditAction node)
        {
            TokenGenerator generator = GetValueForEnumKey(_databaseAuditActionName, node.ActionKind);
            GenerateToken(generator);
        }
    }
}
