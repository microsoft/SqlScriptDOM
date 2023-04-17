//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterExternalResourcePoolStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterExternalResourcePoolStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Alter);
            GenerateExternalResourcePoolStatementBody(node);
        }
    }
}
