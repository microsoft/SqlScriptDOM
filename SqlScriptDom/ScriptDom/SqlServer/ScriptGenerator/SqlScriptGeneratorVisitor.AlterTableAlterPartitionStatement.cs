//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterTableAlterPartitionStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterTableAlterPartitionStatement node)
        {
            GenerateAlterTableHead(node);
            if (node.IsSplit == true)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Split);
            }
            else
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Merge);
            }
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Range);

            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.BoundaryValue);
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
