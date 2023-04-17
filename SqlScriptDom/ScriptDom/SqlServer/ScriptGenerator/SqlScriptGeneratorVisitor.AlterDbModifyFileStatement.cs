//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterDbModifyFileStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterDatabaseModifyFileStatement node)
        {
            GenerateAlterDbStatementHead(node);

            NewLineAndIndent();
            GenerateIdentifier(CodeGenerationSupporter.Modify);
            GenerateSpaceAndKeyword(TSqlTokenType.File);
            GenerateSpaceAndFragmentIfNotNull(node.FileDeclaration);
        }
    }
}
