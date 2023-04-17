//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateProcedureStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateOrAlterProcedureStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Create);
            GenerateKeywordAndSpace(TSqlTokenType.Or);
            GenerateKeywordAndSpace(TSqlTokenType.Alter);

            GenerateProcedureStatementBody(node);
        }
    }
}
