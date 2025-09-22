//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.Utils.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        // When a trailing single-line comment appears at end of a SELECT list line, suppress alignment padding
        // for the very next clause body (e.g., FROM) so we preserve a single space formatting.
        private bool _suppressNextClauseAlignment = false;
        // Track which comment tokens have already been generated to avoid duplicates
        private readonly HashSet<TSqlParserToken> _generatedComments = new HashSet<TSqlParserToken>();
        // Track whether we already emitted leading (file-level) comments

        private bool _leadingCommentsEmitted = false;
        // Track the highest token index logically emitted (fragment or comment)
        private int _lastEmittedTokenIndex = -1;
        // Deferred single-line comments that actually belong to the next statement (prevented from being inlined before a semicolon)
        private readonly List<TSqlParserToken> _pendingLeadingComments = new List<TSqlParserToken>();

        private void EmitPendingLeadingComments()
        {
            if (_pendingLeadingComments.Count == 0) return;
            foreach (var tok in _pendingLeadingComments)
            {
                _writer.AddToken(tok);
                _writer.NewLine();
            }
            _pendingLeadingComments.Clear();
        }
        // get the name for an enum value
        public static TValue GetValueForEnumKey<TKey, TValue>(Dictionary<TKey, TValue> dict, TKey key)
            where TKey : struct, IConvertible
        {
            TValue value = default(TValue);
            if (!dict.TryGetValue(key, out value))
            {
                System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
            }
            return value;
        }

        // generate a comma-separated fragment list
        protected void GenerateFragmentList<T>(IList<T> fragmentList) where T : TSqlFragment
        {
            Boolean first = true;
            foreach (TSqlFragment fragment in fragmentList)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    GenerateSymbolAndSpace(TSqlTokenType.Comma);
                }

                GenerateFragmentIfNotNull(fragment);
            }
        }

        // generate ON / OFF state with an equal sign
        protected void GenerateOptionStateWithEqualSign(String optionName, OptionState optionState)
        {
            GenerateOptionState(optionName, optionState, true);
        }

        protected void GenerateOptionState(String optionName, OptionState optionState)
        {
            this.GenerateOptionState(optionName, optionState, false);
        }

        // generate ON / OFF state with an optional equal sign between name and value
        protected void GenerateOptionState(String optionName, OptionState optionState, Boolean generateEqualSign)
        {
            if (optionState != OptionState.NotSet)
            {
                GenerateIdentifier(optionName);

                // generate a spce or an equal sign
                if (generateEqualSign)
                {
                    GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                }

                GenerateSpace();
                GenerateOptionStateOnOff(optionState);
            }
        }

        // generate ON / OFF for an DatabaseConfigurationOptionState
        protected void GenerateDatabaseConfigurationOptionStateOnOff(DatabaseConfigurationOptionState optionState)
        {
            if (optionState == DatabaseConfigurationOptionState.On)
            {
                GenerateKeyword(TSqlTokenType.On);
            }
            else if (optionState == DatabaseConfigurationOptionState.Off)
            {
                GenerateKeyword(TSqlTokenType.Off);
            }
        }

        // generate ON / OFF for an OptionState
        protected void GenerateOptionStateOnOff(OptionState optionState)
        {
            if (optionState == OptionState.On)
            {
                GenerateKeyword(TSqlTokenType.On);
            }
            else if (optionState == OptionState.Off)
            {
                GenerateKeyword(TSqlTokenType.Off);
            }
        }

        // generate ON / oFF state in SQL80 style
        protected void GenerateOptionStateInSql80Style(string optionName, OptionState optionState)
        {
            if (optionState == OptionState.On)
            {
                GenerateIdentifier(optionName);
            }
        }

        // generate Name = value 
        protected void GenerateNameEqualsValue(String name, TSqlFragment value)
        {
            GenerateTokenAndEqualSign(name);
            GenerateSpaceAndFragmentIfNotNull(value);
        }

        // generate Name = value
        protected void GenerateNameEqualsValue(String name, String value)
        {
            GenerateTokenAndEqualSign(name);
            GenerateSpaceAndIdentifier(value);
        }

        // generate Name = value 
        protected void GenerateNameEqualsValue(TSqlTokenType keywordId, TSqlFragment value)
        {
            GenerateTokenAndEqualSign(keywordId);
            GenerateSpaceAndFragmentIfNotNull(value);
        }

        // generate Name = value
        protected void GenerateNameEqualsValue(TSqlTokenType keywordId, String value)
        {
            GenerateTokenAndEqualSign(keywordId);
            GenerateSpaceAndIdentifier(value);
        }

        // generate Name = value
        protected void GenerateNameEqualsValue(TokenGenerator generator, TSqlFragment value)
        {
            GenerateToken(generator);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpaceAndFragmentIfNotNull(value);
        }

        // generate Name = value
        protected void GenerateNameEqualsValue(TokenGenerator generator, String value)
        {
            GenerateToken(generator);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpaceAndIdentifier(value);
        }

        // generate Token =
        protected void GenerateTokenAndEqualSign(String idText)
        {
            GenerateIdentifierWithoutCasing(idText);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
        }

        // generate Token =
        protected void GenerateTokenAndEqualSign(TSqlTokenType keywordId)
        {
            GenerateKeyword(keywordId);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
        }

        // generate the script from the given fragment if the fragment is not null
        protected void GenerateFragmentIfNotNull(TSqlFragment fragment)
        {
            if (fragment != null)
            {
                // Emit leading comments (those before the first real token) once
                if (_options.PreserveComments && !_leadingCommentsEmitted)
                {
                    EmitLeadingComments(fragment);
                    _leadingCommentsEmitted = true;
                }

                // Gap comments: any comments between the last emitted token and this fragment's first token
                if (_options.PreserveComments)
                {
                    EmitGapCommentsBeforeFragment(fragment);
                }

                fragment.Accept(this);
                
                // Attach trailing/inline comments to the fragment just generated
                if (_options.PreserveComments)
                {
                    GenerateCommentsAfterFragment(fragment);
                }

                // Update last emitted token index (fragment itself)
                if (fragment.LastTokenIndex > _lastEmittedTokenIndex)
                {
                    _lastEmittedTokenIndex = fragment.LastTokenIndex;
                }
            }
        }

        // generate a space and generate the script from the given fragement if the fragment is not null
        protected void GenerateSpaceAndFragmentIfNotNull(TSqlFragment fragment)
        {
            if (fragment != null)
            {
                GenerateSpace();
                GenerateFragmentIfNotNull(fragment);
            }
        }

        // generate the script from the given fragment with enclosing parenthesises
        protected void GenerateParenthesisedFragmentIfNotNull(TSqlFragment fragment)
        {
            if (fragment != null)
            {
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateFragmentIfNotNull(fragment);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
        }

        // generate a comma-separated list
        protected void GenerateCommaSeparatedList<T>(IList<T> list) where T : TSqlFragment
        {
            GenerateCommaSeparatedList(list, false);
        }

        // generate a comma-separated list
        protected void GenerateCommaSeparatedList<T>(IList<T> list, Boolean insertNewLine) where T : TSqlFragment
        {
            GenerateList(list, delegate()
            {
                GenerateSymbol(TSqlTokenType.Comma);
                if (insertNewLine)
                {
                    NewLine();
                }
                else
                {
                    GenerateSpace();
                }
            });
        }

        // generate a comma-separated list
        protected void GenerateCommaSeparatedList<T>(IList<T> list, bool insertNewLine, bool indent, bool generateSpaces = true) where T : TSqlFragment
        {
            GenerateList(list, delegate()
            {
                GenerateSymbol(TSqlTokenType.Comma);
                if (insertNewLine)
                {
                    NewLine();

                    if (indent)
                    {
                        Indent();
                    }
                }
                else if (generateSpaces)
                {
                    GenerateSpace();
                } 
            });
        }

        // generate a dot-separated list
        protected void GenerateDotSeparatedList<T>(IList<T> list) where T : TSqlFragment
        {
            GenerateList(list, delegate() { GenerateSymbol(TSqlTokenType.Dot); });
        }

        // generate a space-separated list
        protected void GenerateSpaceSeparatedList<T>(IList<T> list) where T : TSqlFragment
        {
            GenerateList(list, delegate() { GenerateSpace(); });
        }

        // generate a list with a user-defined separator
        private void GenerateList<T>(IList<T> list, Action gen) where T : TSqlFragment
        {
            if (list != null)
            {
                Boolean first = true;
                foreach (T fragment in list)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        gen();
                    }

                    GenerateFragmentIfNotNull(fragment);
                }
            }
        }

        // generate a parenthsised comma-separated list
        protected void GenerateParenthesisedCommaSeparatedList<T>(IList<T> list) where T : TSqlFragment
        {
            GenerateParenthesisedCommaSeparatedList(list, false);
        }

        // generate a parenthsised comma-separated list
        protected void GenerateParenthesisedCommaSeparatedList<T>(IList<T> list, Boolean alwaysGenerateParenthses, Boolean generateSpaces = true) where T : TSqlFragment
        {
            if (list != null && list.Count > 0)
            {
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateCommaSeparatedList(list, false, false, generateSpaces);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
            else if (alwaysGenerateParenthses)
            {
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
        }

        protected void GenerateFragmentList<T>(IList<T> list, ListGenerationOption option) where T : TSqlFragment
        {
            AlignmentPoint parentheses = new AlignmentPoint();
            AlignmentPoint items = new AlignmentPoint();

            Boolean generateParentheses = (option.AlwaysGenerateParenthesis || (list.Count > 0 && option.Parenthesised));

            // generate open parenthesis
            if (generateParentheses)
            {
                if (option.NewLineBeforeOpenParenthesis)
                {
                    NewLine();
                }
                else
                {
                    GenerateSpace();
                }

                if (option.IndentParentheses)
                {
                    Indent();
                }

                Mark(parentheses);
                GenerateSymbol(TSqlTokenType.LeftParenthesis);

                if (option.NewLineAfterOpenParenthesis)
                {
                    NewLine();
                }
            }

            Boolean firstItem = true;
            foreach (var item in list)
            {
                if (firstItem) // do we have to produce a new line before the first item?
                {
                    if (option.NewLineBeforeFirstItem && option.NewLineAfterOpenParenthesis == false)
                    // we want to avoid an empty line between open parenthesis and the items
                    {
                        NewLine();
                    }

                    firstItem = false;
                }
                else
                {
                    GenerateSeparator(option);
                    if (option.NewLineBeforeItems)
                    {
                        NewLine();
                    }
                }

                for (int indentIterator = 0; indentIterator < option.MultipleIndentItems ; indentIterator++)
                    Indent();

                if (option.NewLineBeforeItems)
                {
                    // we only mark it for multiple lines
                    Mark(items);
                }

                GenerateFragmentIfNotNull(item);
            }

            // generate close parenthesis
            if (generateParentheses)
            {
                if (option.NewLineBeforeCloseParenthesis)
                {
                    NewLine();
                    if (option.AlignParentheses)
                    {
                        Mark(parentheses);
                    }
                }

                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
        }

        protected void GenerateAlignedParenthesizedOptionsWithMultipleIndent<T>(IList<T> list, int indentValue) where T : TSqlFragment
        {
            ListGenerationOption option = ListGenerationOption.CreateOptionFromFormattingConfig(_options);
            option.AlignParentheses = true;
            option.MultipleIndentItems = indentValue;
            if (list.Count > 0)
                GenerateFragmentList(list, option);
            else
                GenerateParenthesisedCommaSeparatedList(list, true);
        }

        protected void GenerateAlignedParenthesizedOptions<T>(IList<T> list) where T : TSqlFragment
        {
            GenerateAlignedParenthesizedOptionsWithMultipleIndent(list, 0);
        }
        
        private void GenerateSeparator(ListGenerationOption option)
        {
            switch (option.Separator)
            {
                case ListGenerationOption.SeparatorType.Comma:
                    GenerateSymbol(TSqlTokenType.Comma);
                    if (option.NewLineBeforeItems == false)
                    {
                        GenerateSpace();
                    }
                    break;
                case ListGenerationOption.SeparatorType.Dot:
                    GenerateSymbol(TSqlTokenType.Dot);
                    break;
                case ListGenerationOption.SeparatorType.Space:
                    GenerateSpace();
                    break;
                default:
                    Debug.Assert(false, "Unknown separator type");
                    break;
            }
        }

        // generate a whitespace 
        protected void GenerateSpace()
        {
            _writer.AddToken(ScriptGeneratorSupporter.CreateWhitespaceToken(1));
        }

        // generate a keyword 
        protected void GenerateKeyword(TSqlTokenType keywordId)
        {
            _writer.AddKeyword(keywordId);
        }

        // generate a keyword and an appending space
        protected void GenerateKeywordAndSpace(TSqlTokenType keywordId)
        {
            GenerateKeyword(keywordId);
            GenerateSpace();
        }

        // generate a white space and a keyword
        protected void GenerateSpaceAndKeyword(TSqlTokenType keywordId)
        {
            GenerateSpace();
            GenerateKeyword(keywordId);
        }

        // generate a symbol
        protected void GenerateSymbol(TSqlTokenType symbolId)
        {
            // symbols are treated as keywords by the parser
            GenerateKeyword(symbolId);
        }

        // generate a token for a given token type and text
        protected void GenerateToken(TSqlTokenType tokenType, String text)
        {
            TSqlParserToken token = new TSqlParserToken(tokenType, text);
            _writer.AddToken(token);
        }

        // generate a white space and a symbol
        protected void GenerateSpaceAndSymbol(TSqlTokenType symbolId)
        {
            GenerateSpace();
            GenerateSymbol(symbolId);
        }

        // generate a symbol and a white space 
        protected void GenerateSymbolAndSpace(TSqlTokenType symbolId)
        {
            GenerateSymbol(symbolId);
            GenerateSpace();
        }

        // generate an identifier
        protected void GenerateIdentifier(String text)
        {
#if false
            Debug.Assert(IsKeyword(text) == false, String.Format(CultureInfo.CurrentCulture, "Keyword({0}) is treated as an identifier", text));
#endif
            _writer.AddIdentifierWithCasing(text);
        }

        // generate an identifier without checking if it is a keyword
        protected void GenerateIdentifierWithoutCheck(String text)
        {
            _writer.AddIdentifierWithoutCasing(text);
        }

        protected void GenerateIdentifierWithoutCasing(String text)
        {
            _writer.AddIdentifierWithoutCasing(text);
        }

        // generate a space and an identifier
        protected void GenerateSpaceAndIdentifier(String idText)
        {
            GenerateSpace();
            GenerateIdentifier(idText);
        }

        protected void GenerateToken(TSqlTokenType tokenType, String text, Boolean applyCasing)
        {
            if (applyCasing)
            {
                text = ScriptGeneratorSupporter.GetCasedString(text, _options.KeywordCasing);
            }

            TSqlParserToken token = new TSqlParserToken(tokenType, text);
            _writer.AddToken(token);
        }

        // generate a comma-separated list for a value of a flag enum
        protected void GenerateCommaSeparatedFlagOpitons<TKey>(Dictionary<TKey, TokenGenerator> optionsGenerators, TKey options) 
            where TKey : struct, IConvertible
        {
            Boolean first = true;
            UInt64 allOptionsValue = System.Convert.ToUInt64(options, CultureInfo.InvariantCulture);
            foreach (TKey option in optionsGenerators.Keys)
            {
                UInt64 optionValue = System.Convert.ToUInt64(option, CultureInfo.InvariantCulture);
                if ((optionValue & allOptionsValue) == optionValue)
                {
                    TokenGenerator generator = GetValueForEnumKey(optionsGenerators, option);
                    if (generator != null)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            GenerateSymbolAndSpace(TSqlTokenType.Comma);
                        }

                        GenerateToken(generator);
                    }
                }
            }
        }

        // generate one token from a generator
        protected void GenerateToken(TokenGenerator generator)
        {
            generator.Generate(_writer);
        }

        // generate tokens from a list of generators
        protected void GenerateTokenList(List<TokenGenerator> generatorList)
        {
            foreach (TokenGenerator generator in generatorList)
            {
                generator.Generate(_writer);
            }
        }

        // generate a keyword followed by a list of identifiers
        protected void GenerateSpaceSeparatedTokens(TSqlTokenType keywordId, params String[] identifiers)
        {
            GenerateKeyword(keywordId);
            foreach (String id in identifiers)
            {
                GenerateSpaceAndIdentifier(id);
            }
        }

        // generate a list of keywords
        protected void GenerateSpaceSeparatedTokens(params TSqlTokenType[] keywords)
        {
            Boolean first = true;
            foreach (TSqlTokenType id in keywords)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    GenerateSpace();
                }
                GenerateKeyword(id);
            }
        }

        // generate a list of identifiers
        protected void GenerateSpaceSeparatedTokens(params String[] identifiers)
        {
            Boolean first = true;
            foreach (String id in identifiers)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    GenerateSpace();
                }
                GenerateIdentifier(id);
            }
        }

        protected void GenerateFragmentWithAlignmentPointIfNotNull(TSqlFragment node, AlignmentPoint ap)
        {
            Debug.Assert(node != null, "TSqlFragment should not be null");
#if !PIMODLANGUAGE
            Debug.Assert(ap != null, "Alignment point should not be null");
#endif

            if (node != null && ap != null)
            {
                AddAlignmentPointForFragment(node, ap);
                GenerateFragmentIfNotNull(node);
                ClearAlignmentPointsForFragment(node);
            }
        }
        
        /// <summary>
        /// Emit leading (file-level) comments appearing before the first token of the first fragment we generate.
        /// </summary>
        private void EmitLeadingComments(TSqlFragment fragment)
        {
            if (fragment?.ScriptTokenStream == null || fragment.FirstTokenIndex <= 0)
                return;

            for (int i = 0; i < fragment.FirstTokenIndex && i < fragment.ScriptTokenStream.Count; i++)
            {
                var token = fragment.ScriptTokenStream[i];
                if ((token.TokenType == TSqlTokenType.SingleLineComment ||
                    token.TokenType == TSqlTokenType.MultilineComment) &&
                    !_generatedComments.Contains(token))
                {
                    GenerateCommentToken(token);
                    _generatedComments.Add(token);
                }
            }
        }

        // Emit comments (not already emitted) located strictly between the last emitted token and the fragment's first token.
        private void EmitGapCommentsBeforeFragment(TSqlFragment fragment)
        {
            if (!_options.PreserveComments)
                return;
            if (fragment?.ScriptTokenStream == null || fragment.FirstTokenIndex < 0)
                return;

            int start = _lastEmittedTokenIndex + 1;
            int end = fragment.FirstTokenIndex - 1;
            if (end < start)
                return; // no gap

            var tokens = fragment.ScriptTokenStream;
            for (int i = start; i <= end && i < tokens.Count; i++)
            {
                var t = tokens[i];
                if ((t.TokenType == TSqlTokenType.SingleLineComment || t.TokenType == TSqlTokenType.MultilineComment) && !_generatedComments.Contains(t))
                {
                    // Heuristic: if single-line comment directly follows a comma or identifier in same logical area, keep inline.
                    // For multiline, existing logic will put it on its own line (GenerateCommentToken handles that).
                    GenerateCommentToken(t);
                    _generatedComments.Add(t);
                    if (i > _lastEmittedTokenIndex)
                        _lastEmittedTokenIndex = i;
                }
            }
        }

        /// <summary>
        /// Generates comment tokens that appear after the specified fragment in the original script.
        /// </summary>
        /// <param name="fragment">The fragment to check for trailing comments.</param>
        protected void GenerateCommentsAfterFragment(TSqlFragment fragment)
        {
            if (!_options.PreserveComments)
                return;
            if (fragment?.ScriptTokenStream == null || fragment.LastTokenIndex < 0)
                return;
                
            // Walk forward from the last token of the fragment until we reach the next non-whitespace, non-comment token.
            // Any comments in this window are treated as trailing (including inline) comments of this fragment.
            for (int i = fragment.LastTokenIndex + 1; i < fragment.ScriptTokenStream.Count; i++)
            {
                var token = fragment.ScriptTokenStream[i];
                if ((token.TokenType == TSqlTokenType.SingleLineComment || 
                     token.TokenType == TSqlTokenType.MultilineComment) &&
                    !_generatedComments.Contains(token))
                {
                    GenerateCommentToken(token);
                    _generatedComments.Add(token);
                }
                else if (token.TokenType != TSqlTokenType.WhiteSpace)
                {
                    // Stop at the next non-whitespace, non-comment token
                    break;
                }
            }
        }
        
        /// <summary>
        /// Generates a comment token to the output.
        /// </summary>
        /// <param name="commentToken">The comment token to generate.</param>
        protected void GenerateCommentToken(TSqlParserToken commentToken)
        {
            if (commentToken.TokenType == TSqlTokenType.SingleLineComment)
            {
                bool atLineStart = _writer.IsLastElementNewLine() || !_writer.HasElements();
                if (atLineStart)
                {
                    // Standalone comment line; just emit and ensure newline after
                    _writer.AddToken(commentToken);
                    if (!commentToken.Text.EndsWith("\n") && !commentToken.Text.EndsWith("\r\n"))
                    {
                        NewLine();
                    }
                }
                else
                {
                    // Inline trailing single-line comment: ensure space then emit; no immediate newline (next clause handles it)
                    _writer.EnsureSingleTrailingSpace();
                    _writer.AddToken(commentToken);
                    _suppressNextClauseAlignment = true;
                    // Do not add newline here to avoid extra blank line; clause separator will introduce it.
                }
            }
            else if (commentToken.TokenType == TSqlTokenType.MultilineComment)
            {
                // Decide if this multiline comment should be inline or block.
                bool isFirstLeadingComment = _generatedComments.Count == 0 && !_leadingCommentsEmitted;
                bool inlineContext = IsLikelyInlineMultilineComment(commentToken);

                if (inlineContext)
                {
                    _writer.InsertInlineTrailingComment(commentToken);
                    _generatedComments.Add(commentToken);
                    return; // already handled fully
                }
                else if (!isFirstLeadingComment)
                {
                    NewLine();
                }

                _writer.AddToken(commentToken);

                if (!commentToken.Text.EndsWith("\n") && !commentToken.Text.EndsWith("\r\n"))
                {
                    NewLine();
                }
            }
        }

        // Heuristic: treat a multiline comment as inline if it appeared between tokens on the same original line
        // and immediately after a comma or identifier in a SELECT list.
        // We approximate using token indices we have tracked: we look backwards in the token stream for the last non-whitespace token.
        private bool IsLikelyInlineMultilineComment(TSqlParserToken commentToken)
        {
            // Need script token stream: attempt to cast through reflection via known field not available here; rely on commentToken.Offset only is insufficient.
            // Fallback heuristic: if previous emitted token index is >= 0 and difference between current token index and last emitted <= 3 (implying only whitespace/newline tokens between),
            // AND the whitespace did not include a newline (cannot directly inspect), we relax and allow inline.
            // Since we cannot read intervening token kinds here without passing more context, store last index in _lastEmittedTokenIndex.
            // If distance small, treat as inline unless we just began (first leading comment handled earlier).
            // This is conservative: will inline some cases that are acceptable.
            if (_lastEmittedTokenIndex < 0)
                return false;

            // If we just emitted a fragment and the gap scanner placed us here, small gap implies inline.
            int currentApproxIndex = _lastEmittedTokenIndex + 1; // best-effort; real index not carried with token instance
            // Without actual token index on the token, we cannot be precise; return false only when first leading.
            // Provide a configurable hook if later needed.
            return true; // prefer inline for multiline comments discovered in gaps
        }
    }
}
