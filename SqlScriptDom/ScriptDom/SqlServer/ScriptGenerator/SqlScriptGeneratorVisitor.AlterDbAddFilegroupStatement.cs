//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterDbAddRemoveFilegroupStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterDatabaseAddFileGroupStatement node)
        {
            GenerateAlterDbStatementHead(node);

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.Add); 

            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Filegroup);
            GenerateSpaceAndFragmentIfNotNull(node.FileGroup);

            if (node.ContainsFileStream)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Contains);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.FileStream);
            }

            if (node.ContainsMemoryOptimizedData)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Contains);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.MemoryOptimizedData);
            }
        }
    }
}
