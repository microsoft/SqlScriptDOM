//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BulkOpenRowset.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(BulkOpenRowset node)
        {
            GenerateKeyword(TSqlTokenType.OpenRowSet);

            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
            GenerateKeyword(TSqlTokenType.Bulk);
            GenerateSpace();
            if (node.DataFiles.Count == 1)
            {
                GenerateCommaSeparatedList(node.DataFiles);
            } 
            else
            {
                GenerateParenthesisedCommaSeparatedList(node.DataFiles);
            }

            if (node.Options.Count > 0)
            {
                GenerateSymbol(TSqlTokenType.Comma);
                GenerateSpace();
                GenerateCommaSeparatedList(node.Options);
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis);

            if (node.WithColumns.Count > 0)
            {
                GenerateSpaceAndSymbol(TSqlTokenType.With);
                GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
                GenerateCommaSeparatedList(node.WithColumns);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }

            GenerateTableAndColumnAliases(node);
        }

        public override void ExplicitVisit(OpenRowsetCosmos node)
        {
            GenerateKeyword(TSqlTokenType.OpenRowSet);
            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
            GenerateNewLineOrSpace(newline: true);
            GenerateCommaSeparatedList(node.Options);
            GenerateSymbol(TSqlTokenType.RightParenthesis);
            if (node.WithColumns.Count > 0)
            {
                GenerateNewLineOrSpace(newline: true);
                GenerateSymbol(TSqlTokenType.With);
                GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
                GenerateCommaSeparatedList(node.WithColumns);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
            GenerateTableAndColumnAliases(node);
        }

        public override void ExplicitVisit(LiteralOpenRowsetCosmosOption node)
        {
            GenerateNameEqualsValue(
                node.OptionKind.ToString().ToUpper(),
                node.Value);
        }
    }
}
