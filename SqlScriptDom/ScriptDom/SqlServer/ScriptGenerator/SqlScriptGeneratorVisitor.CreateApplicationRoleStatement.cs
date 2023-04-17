//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateApplicationRoleStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateApplicationRoleStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Create);

            GenerateApplicationRoleStatementBase(node);
        }
    }
}
