//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.GrantStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(GrantStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Grant);

            GeneratePermissionOnToClauses(node);

            if (node.WithGrantOption)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpaceAndKeyword(TSqlTokenType.Grant);
                GenerateSpaceAndKeyword(TSqlTokenType.Option); 
            }

            GenerateAsClause(node);
        }
    }
}
