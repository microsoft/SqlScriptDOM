//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateOrAlterTriggerStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateOrAlterTriggerStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Create);
            GenerateKeywordAndSpace(TSqlTokenType.Or);
            GenerateKeywordAndSpace(TSqlTokenType.Alter);

            GenerateTriggerStatementBody(node);
        }
    }
}
