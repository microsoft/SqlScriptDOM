//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AuditSpecificationPart.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AuditSpecificationPart node)
        {
            GenerateKeywordAndSpace(node.IsDrop ? TSqlTokenType.Drop : TSqlTokenType.Add);
            GenerateSymbol(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.Details);
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
