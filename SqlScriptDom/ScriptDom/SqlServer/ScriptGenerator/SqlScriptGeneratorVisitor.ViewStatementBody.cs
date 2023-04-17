//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ViewStatementBody.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateViewStatementBody(ViewStatementBody node)
        {
            GenerateKeyword(TSqlTokenType.View);

            GenerateSpaceAndFragmentIfNotNull(node.SchemaObjectName);

            if (node.Columns.Count > 0)
            {
                if (_options.MultilineViewColumnsList)
                {
                    ListGenerationOption option = ListGenerationOption.CreateOptionFromFormattingConfig(_options);

                    GenerateFragmentList(node.Columns, option);
                }
                else
                {
                    GenerateSpace();
                    GenerateParenthesisedCommaSeparatedList(node.Columns);
                }
            }

            if (node.ViewOptions.Count > 0)
            {
                NewLine();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpace();
                if (node.IsMaterialized)
                {
                    GenerateKeyword(TSqlTokenType.LeftParenthesis);
                    GenerateCommaSeparatedList(node.ViewOptions);
                    GenerateKeyword(TSqlTokenType.RightParenthesis);
                }
                else
                {
                    GenerateCommaSeparatedList(node.ViewOptions);
                }
            }

            GenerateNewLineOrSpace(_options.AsKeywordOnOwnLine);

            GenerateKeyword(TSqlTokenType.As);
            NewLine();

            if (_options.IndentViewBody)
            {
                Indent();
            }

            AlignmentPoint select = new AlignmentPoint();
            MarkAndPushAlignmentPoint(select);

            // to avoid generate semicolon for the SELECT statement
            Boolean originalValue = _generateSemiColon;
            _generateSemiColon = false;

            GenerateFragmentIfNotNull(node.SelectStatement);

            // restore the original value
            _generateSemiColon = originalValue;
            
            PopAlignmentPoint();

            if (node.WithCheckOption)
            {
                NewLine();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpaceAndKeyword(TSqlTokenType.Check);
                GenerateSpaceAndKeyword(TSqlTokenType.Option);
            }
        }
    }
}
