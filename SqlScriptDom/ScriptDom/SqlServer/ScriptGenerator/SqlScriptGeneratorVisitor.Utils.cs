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

        // generate the script from the given fragement if the fragment is not null
        protected void GenerateFragmentIfNotNull(TSqlFragment fragment)
        {
            if (fragment != null)
            {
                fragment.Accept(this);
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
        protected void GenerateCommaSeparatedList<T>(IList<T> list, bool insertNewLine, bool indent) where T : TSqlFragment
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
                else
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
        protected void GenerateParenthesisedCommaSeparatedList<T>(IList<T> list, Boolean alwaysGenerateParenthses) where T : TSqlFragment
        {
            if (list != null && list.Count > 0)
            {
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateCommaSeparatedList(list);
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
#if false
        // check if a string is a keyword
        protected abstract Boolean IsKeyword(String identifier);
#endif
    }
}
