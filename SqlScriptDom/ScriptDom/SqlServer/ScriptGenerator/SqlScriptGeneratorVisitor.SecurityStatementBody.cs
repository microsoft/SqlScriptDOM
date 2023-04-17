//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SecurityStatementBody.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GeneratePermissionOnToClauses(SecurityStatement node)
        {
            GenerateCommaSeparatedList(node.Permissions);

            if (node.SecurityTargetObject != null)
            {
                NewLineAndIndent();
                GenerateFragmentIfNotNull(node.SecurityTargetObject);
            }

            GenerateSpaceAndKeyword(TSqlTokenType.To);

            GenerateSpace();
            GenerateCommaSeparatedList(node.Principals);
        }

        protected void GenerateAsClause(SecurityStatement node)
        {
            if (node.AsClause != null)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.As);
                GenerateSpaceAndFragmentIfNotNull(node.AsClause);
            }
        }
    }
}
