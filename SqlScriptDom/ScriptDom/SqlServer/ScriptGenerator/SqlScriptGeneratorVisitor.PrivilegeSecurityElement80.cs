//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.PrivilegeSecurityElement80.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(PrivilegeSecurityElement80 node)
        {
            if (node.Privileges != null && node.Privileges.Count > 0)
            {
                GenerateCommaSeparatedList(node.Privileges);

                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.On);
                GenerateSpaceAndFragmentIfNotNull(node.SchemaObjectName);

                if (node.Columns != null && node.Columns.Count > 0)
                {
                    GenerateSpace();
                    GenerateParenthesisedCommaSeparatedList(node.Columns);
                }
            }
        }
    }
}
