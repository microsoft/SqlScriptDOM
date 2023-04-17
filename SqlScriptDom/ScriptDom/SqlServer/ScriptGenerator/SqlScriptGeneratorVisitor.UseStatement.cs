//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.UseStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(UseStatement node)
        {
            GenerateKeyword(TSqlTokenType.Use);
            GenerateSpaceAndFragmentIfNotNull(node.DatabaseName);
        }
    }
}
