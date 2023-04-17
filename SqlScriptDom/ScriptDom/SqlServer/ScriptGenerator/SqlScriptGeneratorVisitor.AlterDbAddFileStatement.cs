//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterDbAddFileStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterDatabaseAddFileStatement node)
        {
            GenerateAlterDbStatementHead(node);

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.Add); 

            if (node.IsLog)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Log); 
            }
            GenerateSpaceAndKeyword(TSqlTokenType.File);
            GenerateSpace();
            GenerateCommaSeparatedList(node.FileDeclarations);

            if (node.FileGroup != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.To);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Filegroup);
                GenerateSpaceAndFragmentIfNotNull(node.FileGroup);
            }
        }
    }
}
