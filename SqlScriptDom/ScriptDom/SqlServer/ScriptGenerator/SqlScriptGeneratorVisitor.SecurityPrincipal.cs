//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SecurityPrincipal.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(SecurityPrincipal node)
        {
            switch (node.PrincipalType)
            {
                case PrincipalType.Null:
                    GenerateKeyword(TSqlTokenType.Null); 
                    break;
                case PrincipalType.Public:
                    GenerateKeyword(TSqlTokenType.Public); 
                    break;
                case PrincipalType.Identifier:
                    GenerateFragmentIfNotNull(node.Identifier);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }
    }
}
