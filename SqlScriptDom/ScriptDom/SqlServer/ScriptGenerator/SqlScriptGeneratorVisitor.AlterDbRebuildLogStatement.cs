//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterDbRebuildLogStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterDatabaseRebuildLogStatement node)
        {
            GenerateAlterDbStatementHead(node);

            NewLineAndIndent();
            GenerateIdentifier(CodeGenerationSupporter.Rebuild);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Log); 
            if (node.FileDeclaration != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.On);
                GenerateSpaceAndFragmentIfNotNull(node.FileDeclaration);
            }
        }
    }
}
