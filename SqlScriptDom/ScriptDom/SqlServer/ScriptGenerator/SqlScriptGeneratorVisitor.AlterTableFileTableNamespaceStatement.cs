//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterTableFileTableNamespaceStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterTableFileTableNamespaceStatement node)
        {
            GenerateAlterTableHead(node);
            GenerateSpaceAndIdentifier(node.IsEnable ? CodeGenerationSupporter.Enable : CodeGenerationSupporter.Disable);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.FileTableNamespace);
        }
    }
}
