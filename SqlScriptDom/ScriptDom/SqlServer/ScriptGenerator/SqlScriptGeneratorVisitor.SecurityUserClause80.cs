//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SecurityUserClause80.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(SecurityUserClause80 node)
        {
            switch (node.UserType80)
            {
                case UserType80.Null:
                    GenerateKeyword(TSqlTokenType.Null);
                    break;
                case UserType80.Public:
                    GenerateKeyword(TSqlTokenType.Public);
                    break;
                case UserType80.Users:
                    if (node.Users != null && node.Users.Count > 0)
                    {
                        GenerateCommaSeparatedList(node.Users);
                    }
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }
    }
}
