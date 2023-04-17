//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateFunctionStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateFunctionStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Create);

            GenerateFunctionStatementBody(node);
        }
    }
}
