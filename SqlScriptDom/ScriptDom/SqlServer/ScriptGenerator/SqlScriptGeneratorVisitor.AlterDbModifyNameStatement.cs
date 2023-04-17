//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterDbModifyNameStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterDatabaseModifyNameStatement node)
        {
            GenerateAlterDbStatementHead(node);

            NewLineAndIndent();
            GenerateIdentifier(CodeGenerationSupporter.Modify);
            GenerateSpace();
            GenerateNameEqualsValue(CodeGenerationSupporter.Name, node.NewDatabaseName);
        }
    }
}
