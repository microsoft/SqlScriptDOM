//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterDbRemoveFilegroupStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterDatabaseRemoveFileGroupStatement node)
        {
            GenerateAlterDbStatementHead(node);

            NewLineAndIndent();
            GenerateIdentifier(CodeGenerationSupporter.Remove);

            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Filegroup);
            GenerateSpaceAndFragmentIfNotNull(node.FileGroup);
        }
    }
}
