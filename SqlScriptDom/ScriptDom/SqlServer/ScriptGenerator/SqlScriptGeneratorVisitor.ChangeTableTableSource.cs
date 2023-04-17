//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ChangeTableTableSource.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateChangeTablePrefix(SchemaObjectName target, string changeTableKind)
        {
            GenerateIdentifier(CodeGenerationSupporter.ChangeTable);
            GenerateSymbol(TSqlTokenType.LeftParenthesis);
            GenerateIdentifier(changeTableKind);
            GenerateSpaceAndFragmentIfNotNull(target);
            GenerateSymbolAndSpace(TSqlTokenType.Comma);
        }

        public override void ExplicitVisit(ChangeTableChangesTableReference node)
        {
            GenerateChangeTablePrefix(node.Target, CodeGenerationSupporter.Changes);
            GenerateFragmentIfNotNull(node.SinceVersion);

            GenerateSymbol(TSqlTokenType.RightParenthesis);
            GenerateTableAndColumnAliases(node);
        }

        public override void ExplicitVisit(ChangeTableVersionTableReference node)
        {
            GenerateChangeTablePrefix(node.Target, CodeGenerationSupporter.Version);
            GenerateParenthesisedCommaSeparatedList(node.PrimaryKeyColumns);
            GenerateSymbolAndSpace(TSqlTokenType.Comma);
            GenerateParenthesisedCommaSeparatedList(node.PrimaryKeyValues);

            GenerateSymbol(TSqlTokenType.RightParenthesis);
            GenerateTableAndColumnAliases(node);
        }
    }
}