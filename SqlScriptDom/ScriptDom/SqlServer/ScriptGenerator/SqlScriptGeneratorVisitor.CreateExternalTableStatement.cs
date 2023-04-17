//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateExternalTableStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateExternalTableStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.External);
            GenerateSpaceAndKeyword(TSqlTokenType.Table);

            GenerateSpaceAndFragmentIfNotNull(node.SchemaObjectName);

            GenerateExternalTableStatementBody(node);

            if(node.SelectStatement != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.As);
                GenerateNewLineOrSpace(true);
                this.ExplicitVisit(node.SelectStatement);
            }
        }

        protected void GenerateExternalTableStatementBody(ExternalTableStatement node)
        {
            // external table column definitions
            GenerateExternalTableColumnDefinitions(node);

            // external table options
            GenerateExternalTableOptions(node);
        }

        private void GenerateExternalTableColumnDefinitions(ExternalTableStatement node)
        {
            if (node.ColumnDefinitions != null && node.ColumnDefinitions.Count != 0)
            {
                List<TSqlFragment> columns = new List<TSqlFragment>();
                foreach (ExternalTableColumnDefinition col in node.ColumnDefinitions)
                {
                    columns.Add(col);
                }

                ListGenerationOption option = ListGenerationOption.CreateOptionFromFormattingConfig(_options);

                GenerateFragmentList(columns, option);
            }
        }

        private void GenerateExternalTableOptions(ExternalTableStatement node)
        {
            NewLineAndIndent();
            GenerateKeywordAndSpace(TSqlTokenType.With);
            GenerateKeyword(TSqlTokenType.LeftParenthesis);

            // external data source identifier
            if (node.DataSource != null)
            {
                NewLineAndIndent();
                GenerateNameEqualsValue(CodeGenerationSupporter.DataSource, node.DataSource);
            }

            if (node.ExternalTableOptions.Count > 0)
            {
                if (node.DataSource != null)
                {
                    GenerateSymbol(TSqlTokenType.Comma);
                }
                NewLineAndIndent();
                GenerateCommaSeparatedList(node.ExternalTableOptions, true, true);
            }

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.RightParenthesis);
        }
    }
}
