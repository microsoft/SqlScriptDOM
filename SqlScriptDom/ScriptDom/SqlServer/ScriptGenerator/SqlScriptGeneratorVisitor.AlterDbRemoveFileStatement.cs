//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterDbRemoveFileStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterDatabaseRemoveFileStatement node)
        {
            GenerateAlterDbStatementHead(node);

            NewLineAndIndent();
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Remove);
            GenerateSpaceAndKeyword(TSqlTokenType.File);
            GenerateSpaceAndFragmentIfNotNull(node.File);
        }
    }
}
