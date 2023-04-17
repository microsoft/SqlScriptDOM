//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DenyStatement80.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DenyStatement80 node)
        {
            GenerateKeyword(TSqlTokenType.Deny);

            // could be CommandSecurityElement80 
            //          PrivilegeSecurityElement80
            GenerateSpaceAndFragmentIfNotNull(node.SecurityElement80);

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.To);
            GenerateSpaceAndFragmentIfNotNull(node.SecurityUserClause80);

            if (node.CascadeOption == true)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.Cascade);
            }
        }
    }
}
