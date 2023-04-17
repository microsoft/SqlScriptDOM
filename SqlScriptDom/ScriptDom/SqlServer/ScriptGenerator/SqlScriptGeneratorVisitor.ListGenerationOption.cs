//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ListGenerationOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        internal class ListGenerationOption
        {
            internal enum SeparatorType
            {
                Comma,
                Dot,
                Space,
            }

            public Boolean Parenthesised { get; set; }
            public Boolean AlwaysGenerateParenthesis { get; set; }
            public Boolean IndentParentheses { get; set; }
            public Boolean AlignParentheses { get; set; }
            public Boolean NewLineBeforeOpenParenthesis { get; set; }
            public Boolean NewLineAfterOpenParenthesis { get; set; }
            public Boolean NewLineBeforeCloseParenthesis { get; set; }

            public SeparatorType Separator { get; set; }

            public Boolean NewLineBeforeFirstItem { get; set; }
            public Boolean NewLineBeforeItems { get; set; }
            public int MultipleIndentItems { get; set; }

            public static ListGenerationOption MultipleLineSelectElementOption = new ListGenerationOption()
            {
                Parenthesised = false,
                AlwaysGenerateParenthesis = false,
                IndentParentheses = false,
                AlignParentheses = false,

                Separator = SeparatorType.Comma,
                NewLineBeforeFirstItem = false,
                NewLineBeforeItems = true,
                MultipleIndentItems = 0,
            };

            public static ListGenerationOption CreateOptionFromFormattingConfig(SqlScriptGeneratorOptions formatting)
            {
                ListGenerationOption option = new ListGenerationOption();

                option.Parenthesised = true;
                option.AlwaysGenerateParenthesis = true;
                option.NewLineBeforeOpenParenthesis = formatting.NewLineBeforeOpenParenthesisInMultilineList;
                option.NewLineAfterOpenParenthesis = true;
                option.IndentParentheses = false;
                option.NewLineBeforeCloseParenthesis = formatting.NewLineBeforeCloseParenthesisInMultilineList;
                option.AlignParentheses = false;

                option.NewLineBeforeItems = true;
                option.NewLineBeforeFirstItem = false;
                option.MultipleIndentItems = 1;
                option.Separator = ListGenerationOption.SeparatorType.Comma;

                return option;
            }
       }

    }
}
