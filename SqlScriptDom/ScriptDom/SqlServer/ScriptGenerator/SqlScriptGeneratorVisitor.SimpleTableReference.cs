//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SimpleTableSource.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateSpaceAndAlias(Identifier alias)
        {
            if (alias != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.As);
                GenerateSpaceAndFragmentIfNotNull(alias);
            }
        }

        protected void GenerateTableAndColumnAliases(TableReferenceWithAliasAndColumns node)
        {
            if (node.ForPath)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.For);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Path);
            }

            GenerateSpaceAndAlias(node.Alias);
            GenerateParenthesisedCommaSeparatedList(node.Columns);
        }
    }
}
