//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.RevokeStatement80.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(RevokeStatement80 node)
        {
            GenerateKeyword(TSqlTokenType.Revoke); 
            
            if (node.GrantOptionFor)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Grant); 
                GenerateSpaceAndKeyword(TSqlTokenType.Option); 
                GenerateSpaceAndKeyword(TSqlTokenType.For); 
            }

            // could be
            //      CommandSecurityElement80
            //      PrivilegeSecurityElement80
            GenerateSpaceAndFragmentIfNotNull(node.SecurityElement80);

            GenerateSpaceAndKeyword(TSqlTokenType.From); 
            GenerateSpaceAndFragmentIfNotNull(node.SecurityUserClause80);

            if (node.CascadeOption)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Cascade); 
            }

            if (node.AsClause != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.As);
                GenerateSpaceAndFragmentIfNotNull(node.AsClause);
            }
        }
    }
}
