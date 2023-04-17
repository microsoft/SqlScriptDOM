//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.RevokeStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(RevokeStatement node)
        {
            GenerateKeyword(TSqlTokenType.Revoke); 

            if (node.GrantOptionFor == true)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Grant);
                GenerateSpaceAndKeyword(TSqlTokenType.Option);
                GenerateSpaceAndKeyword(TSqlTokenType.For); 
            }

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
