//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterDbScopedConfigurationClearStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterDatabaseScopedConfigurationClearStatement node)
        {
            GenerateAlterDatabaseScopedConfigHead(node);
            GenerateIdentifier(CodeGenerationSupporter.Clear);
            GenerateSpaceAndFragmentIfNotNull(node.Option);
        }
    }
}
