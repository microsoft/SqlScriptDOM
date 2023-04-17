//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.OpenRowsetTableSource.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(OpenRowsetTableReference node)
        {
            GenerateKeyword(TSqlTokenType.OpenRowSet);

            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.ProviderName);

            GenerateSymbol(TSqlTokenType.Comma);

            if (node.ProviderString != null)
            {
                GenerateSpaceAndFragmentIfNotNull(node.ProviderString);
            }
            else
            {
                GenerateSpaceAndFragmentIfNotNull(node.DataSource);
                GenerateSymbol(TSqlTokenType.Semicolon);

                GenerateSpaceAndFragmentIfNotNull(node.UserId);
                GenerateSymbol(TSqlTokenType.Semicolon);

                GenerateSpaceAndFragmentIfNotNull(node.Password);
            }

            GenerateSymbol(TSqlTokenType.Comma);

            if (node.Query != null)
            {
                GenerateSpaceAndFragmentIfNotNull(node.Query);
            }
            else
            {
                GenerateSpaceAndFragmentIfNotNull(node.Object);
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis);

            if (node.WithColumns.Count > 0)
            {
                GenerateNewLineOrSpace(newline: true);
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.WithColumns);
            }

            GenerateSpaceAndAlias(node.Alias);
        }
    }
}
