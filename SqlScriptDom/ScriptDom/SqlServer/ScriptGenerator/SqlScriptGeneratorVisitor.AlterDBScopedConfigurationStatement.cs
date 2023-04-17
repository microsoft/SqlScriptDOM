//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterDbScopedConfigurationStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateAlterDatabaseScopedConfigHead(AlterDatabaseScopedConfigurationStatement node)
        {
            GenerateSpaceSeparatedTokens(TSqlTokenType.Alter, TSqlTokenType.Database);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Scoped);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Configuration);
            GenerateSpace();

            if (node.Secondary)
            {
                GenerateIdentifier(CodeGenerationSupporter.For);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Secondary);
                GenerateSpace();
            }
        }
    }
}
