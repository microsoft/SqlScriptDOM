//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterDbScopedConfigurationSetStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterDatabaseScopedConfigurationSetStatement node)
        {
            GenerateAlterDatabaseScopedConfigHead(node);
            GenerateKeyword(TSqlTokenType.Set);
            GenerateSpaceAndFragmentIfNotNull(node.Option);
        }
    }
}
