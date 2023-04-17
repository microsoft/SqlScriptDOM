//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SubDmlTableSource.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DataModificationTableReference node)
        {
            GenerateSymbol(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.DataModificationSpecification);
            GenerateSymbol(TSqlTokenType.RightParenthesis);
            GenerateTableAndColumnAliases(node);
        }
    }
}
