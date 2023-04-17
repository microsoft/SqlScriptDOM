//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateOrAlterFunctionStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateOrAlterFunctionStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Create);
            GenerateKeywordAndSpace(TSqlTokenType.Or);
            GenerateKeywordAndSpace(TSqlTokenType.Alter);

            GenerateFunctionStatementBody(node);
        }
    }
}
