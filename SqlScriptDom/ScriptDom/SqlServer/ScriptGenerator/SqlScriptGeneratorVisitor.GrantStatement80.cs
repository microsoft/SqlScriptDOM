//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.GrantStatement80.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(GrantStatement80 node)
        {
            GenerateKeyword(TSqlTokenType.Grant);

            // could be
            //      CommandSecurityElement80
            //      PrivilegeSecurityElement80
            GenerateSpaceAndFragmentIfNotNull(node.SecurityElement80);

            GenerateSpaceAndKeyword(TSqlTokenType.To);

            GenerateSpaceAndFragmentIfNotNull(node.SecurityUserClause80);

            if (node.WithGrantOption == true)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpaceAndKeyword(TSqlTokenType.Grant);
                GenerateSpaceAndKeyword(TSqlTokenType.Option); 
            }

            if (node.AsClause != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.As);
                GenerateSpaceAndFragmentIfNotNull(node.AsClause);
            }
        }
    }
}
