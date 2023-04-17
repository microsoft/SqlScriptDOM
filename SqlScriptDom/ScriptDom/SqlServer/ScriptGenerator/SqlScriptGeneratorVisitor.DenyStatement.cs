//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DenyStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DenyStatement node)
        {
            GenerateKeyword(TSqlTokenType.Deny);

            GenerateSpace();
            GeneratePermissionOnToClauses(node);

            if (node.CascadeOption)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Cascade);
            }

            GenerateAsClause(node);
        }
    }
}
