//------------------------------------------------------------------------------
// <copyright file="TSql80ParserBaseInternal.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using antlr;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    internal abstract class TSql80ParserBaseInternal : antlr.LLkParser
    {
        private readonly TSqlFragmentFactory _fragmentFactory = new TSqlFragmentFactory();

        private IList<ParseError> _parseErrors;
        private bool _phaseOne;
        protected TSqlWhitespaceTokenFilter _tokenSource;
        private bool _initialQuotedIdentifiersOn = true;

        // Indicates the previous statement level error location in phase one parsing mode.
        // Start from -1 which is not a valid line/column. 0 would be ambiguous.
        private int _phaseOnePreviousStatementLevelErrorLine = -1;
        private int _phaseOnePreviousStatementLevelErrorColumn = -1;

        private static readonly antlr.collections.impl.BitSet _statementLevelRecoveryTokens = new antlr.collections.impl.BitSet(4);

        private static readonly antlr.collections.impl.BitSet _phaseOneBatchLevelRecoveryTokens = new antlr.collections.impl.BitSet(3);

        private static readonly antlr.collections.impl.BitSet _ddlStatementBeginnerTokens = new antlr.collections.impl.BitSet(2);

        const int LookAhead = 2;

        //private static HashSet<string> _languageString = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        //{
        //    CodeGenerationSupporter.ChineseMacaoSar,
        //    CodeGenerationSupporter.ChineseSingapore,
        //    CodeGenerationSupporter.SerbianCyrillic,
        //    CodeGenerationSupporter.Spanish,
        //    CodeGenerationSupporter.ChineseHongKong,
        //    CodeGenerationSupporter.SerbianLatin,
        //    CodeGenerationSupporter.Portuegese,
        //    CodeGenerationSupporter.BritishEnglish,
        //    CodeGenerationSupporter.SimplifiedChinese,
        //    CodeGenerationSupporter.Marathi,
        //    CodeGenerationSupporter.Malayalam,
        //    CodeGenerationSupporter.Kannada,
        //    CodeGenerationSupporter.Telugu,
        //    CodeGenerationSupporter.Tamil,
        //    CodeGenerationSupporter.Gujarati,
        //    CodeGenerationSupporter.Punjabi,
        //    CodeGenerationSupporter.BengaliIndia,
        //    CodeGenerationSupporter.MalayMalaysia,
        //    CodeGenerationSupporter.Hindi,
        //    CodeGenerationSupporter.Vietnamese,
        //    CodeGenerationSupporter.Lithuanian,
        //    CodeGenerationSupporter.Latvian,
        //    CodeGenerationSupporter.Slovenian,
        //    CodeGenerationSupporter.Ukrainian,
        //    CodeGenerationSupporter.Indonesian,
        //    CodeGenerationSupporter.Urdu,
        //    CodeGenerationSupporter.Thai,
        //    CodeGenerationSupporter.Swedish,
        //    CodeGenerationSupporter.Slovak,
        //    CodeGenerationSupporter.Croatian,
        //    CodeGenerationSupporter.Russian,
        //    CodeGenerationSupporter.Romanian,
        //    CodeGenerationSupporter.Brazilian,
        //    CodeGenerationSupporter.NorwegianBokmal,
        //    CodeGenerationSupporter.Dutch,
        //    CodeGenerationSupporter.Korean,
        //    CodeGenerationSupporter.Japanese,
        //    CodeGenerationSupporter.Italian,
        //    CodeGenerationSupporter.Icelandic,
        //    CodeGenerationSupporter.Hebrew,
        //    CodeGenerationSupporter.French,
        //    CodeGenerationSupporter.English,
        //    CodeGenerationSupporter.German,
        //    CodeGenerationSupporter.TraditionalChinese,
        //    CodeGenerationSupporter.Catalan,
        //    CodeGenerationSupporter.Bulgarian,
        //    CodeGenerationSupporter.Arabic,
        //    CodeGenerationSupporter.Neutral,
        //};

        //private static HashSet<string> _languageIdentifier = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        //{
        //    CodeGenerationSupporter.Spanish,
        //    CodeGenerationSupporter.Portuegese,
        //    CodeGenerationSupporter.Marathi,
        //    CodeGenerationSupporter.Malayalam,
        //    CodeGenerationSupporter.Kannada,
        //    CodeGenerationSupporter.Telugu,
        //    CodeGenerationSupporter.Tamil,
        //    CodeGenerationSupporter.Gujarati,
        //    CodeGenerationSupporter.Punjabi,
        //    CodeGenerationSupporter.Hindi,
        //    CodeGenerationSupporter.Vietnamese,
        //    CodeGenerationSupporter.Lithuanian,
        //    CodeGenerationSupporter.Latvian,
        //    CodeGenerationSupporter.Slovenian,
        //    CodeGenerationSupporter.Ukrainian,
        //    CodeGenerationSupporter.Indonesian,
        //    CodeGenerationSupporter.Urdu,
        //    CodeGenerationSupporter.Thai,
        //    CodeGenerationSupporter.Swedish,
        //    CodeGenerationSupporter.Slovak,
        //    CodeGenerationSupporter.Croatian,
        //    CodeGenerationSupporter.Russian,
        //    CodeGenerationSupporter.Romanian,
        //    CodeGenerationSupporter.Brazilian,
        //    CodeGenerationSupporter.Dutch,
        //    CodeGenerationSupporter.Korean,
        //    CodeGenerationSupporter.Japanese,
        //    CodeGenerationSupporter.Italian,
        //    CodeGenerationSupporter.Icelandic,
        //    CodeGenerationSupporter.Hebrew,
        //    CodeGenerationSupporter.French,
        //    CodeGenerationSupporter.English,
        //    CodeGenerationSupporter.German,
        //    CodeGenerationSupporter.Catalan,
        //    CodeGenerationSupporter.Bulgarian,
        //    CodeGenerationSupporter.Arabic,
        //    CodeGenerationSupporter.Neutral,
        //};

        //private static HashSet<int> _languageInteger = new HashSet<int>()
        //{
        //    5124,
        //    4100,
        //    3098,
        //    3082,
        //    3076,
        //    2074,
        //    2070,
        //    2057,
        //    2052,
        //    1102,
        //    1100,
        //    1099,
        //    1098,
        //    1097,
        //    1095,
        //    1094,
        //    1093,
        //    1086,
        //    1081,
        //    1066,
        //    1063,
        //    1062,
        //    1060,
        //    1058,
        //    1057,
        //    1056,
        //    1054,
        //    1053,
        //    1051,
        //    1050,
        //    1049,
        //    1048,
        //    1046,
        //    1044,
        //    1043,
        //    1042,
        //    1041,
        //    1040,
        //    1039,
        //    1037,
        //    1036,
        //    1033,
        //    1031,
        //    1028,
        //    1027,
        //    1026,
        //    1025,
        //    0,
        //};

        private static HashSet<SqlDataTypeOption> _possibleSingleParameterDataTypes = new HashSet<SqlDataTypeOption>()
        {
            SqlDataTypeOption.Char, SqlDataTypeOption.VarChar, SqlDataTypeOption.NChar,
            SqlDataTypeOption.NVarChar, SqlDataTypeOption.Decimal, SqlDataTypeOption.Float, 
            SqlDataTypeOption.Numeric, SqlDataTypeOption.Binary, SqlDataTypeOption.VarBinary,
            SqlDataTypeOption.Time, SqlDataTypeOption.DateTime2, SqlDataTypeOption.DateTimeOffset
        };

        #region Constructors

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql80ParserBaseInternal(TokenBuffer tokenBuf, int k)
            : base(tokenBuf, k)
        {
        }

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql80ParserBaseInternal(ParserSharedInputState state, int k)
            : base(state, k)
        {
        }

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql80ParserBaseInternal(TokenStream lexer, int k)
            : base(lexer, k)
        {
        }

        /// <summary>
        /// Real constructor (the one which is used)
        /// </summary>
        /// <param name="initialQuotedIdentifiersOn">if set to <c>true</c> initial quoted identifiers will be on.</param>
        public TSql80ParserBaseInternal(bool initialQuotedIdentifiersOn)
            : base(LookAhead)
        {
            _initialQuotedIdentifiersOn = initialQuotedIdentifiersOn;
        }

        public void InitializeForNewInput(IList<TSqlParserToken> tokens, IList<ParseError> errors, bool phaseOne)
        {
            _tokenSource = new TSqlWhitespaceTokenFilter(_initialQuotedIdentifiersOn, tokens);
            _parseErrors = errors;
            _fragmentFactory.SetTokenStream(tokens);
            PhaseOne = phaseOne;
            setTokenBuffer(new TokenBuffer(_tokenSource));
            resetState();
        }

        static TSql80ParserBaseInternal()
        {
            _ddlStatementBeginnerTokens.add(TSql80ParserInternal.Create);
            _ddlStatementBeginnerTokens.add(TSql80ParserInternal.Alter);

            _statementLevelRecoveryTokens.add(TSql80ParserInternal.Go);
            _statementLevelRecoveryTokens.add(TSql80ParserInternal.Semicolon);
            _statementLevelRecoveryTokens.orInPlace(_ddlStatementBeginnerTokens);

            _phaseOneBatchLevelRecoveryTokens.add(TSql80ParserInternal.Go);
            _phaseOneBatchLevelRecoveryTokens.orInPlace(_ddlStatementBeginnerTokens);
        }

        #endregion

        protected void ResetQuotedIdentifiersSettingToInitial()
        {
            _tokenSource.QuotedIdentifier = _initialQuotedIdentifiersOn;
        }

        /// <summary>
        /// Updates token offsets for given fragment
        /// </summary>
        /// <param name="fragment">The fragment.</param>
        /// <param name="token">The token.</param>
        internal static void UpdateTokenInfo(TSqlFragment fragment, antlr.IToken token)
        {
            TSqlWhitespaceTokenFilter.TSqlParserTokenProxyWithIndex proxy = (TSqlWhitespaceTokenFilter.TSqlParserTokenProxyWithIndex)token;
            int tokenIndex = proxy.TokenIndex;
            if (tokenIndex != TSqlFragment.Uninitialized)
                fragment.UpdateTokenInfo(tokenIndex, tokenIndex);
        }

        protected static void AddAndUpdateTokenInfo<TFragmentType>(TSqlFragment node, IList<TFragmentType> collection, TFragmentType item)
            where TFragmentType : TSqlFragment
        {
            collection.Add(item);
            node.UpdateTokenInfo(item);
        }

        protected static void AddAndUpdateTokenInfo<TFragmentType>(TSqlFragment node, IList<TFragmentType> collection, IList<TFragmentType> otherCollection)
            where TFragmentType : TSqlFragment
        {
            foreach (TFragmentType item in otherCollection)
            {
                AddAndUpdateTokenInfo(node, collection, item);
            }
        }

        protected static string DecodeAsciiStringLiteral(string encodedValue)
        {
            int length = encodedValue.Length;
            Debug.Assert(length > 1);

            string valueWithoutQuotes = encodedValue.Substring(1, length - 2);
            if (encodedValue[0] == '"') // Quoted identifier when quoted identifiers disabled
                return valueWithoutQuotes.Replace("\"\"", "\"");
            else
                return valueWithoutQuotes.Replace("''", "'");
        }

        protected static string DecodeUnicodeStringLiteral(string encodedValue)
        {
            int length = encodedValue.Length;
            Debug.Assert(length > 2);

            return encodedValue.Substring(2, length - 3).Replace("''", "'");
        }

        protected static bool IsAsciiStringLob(string asciiValue)
        {
            return asciiValue.Length > 8000;
        }

        protected static bool IsUnicodeStringLob(string unicodeValue)
        {
            return unicodeValue.Length > 8000;
        }

        protected static bool IsBinaryLiteralLob(string binaryValue)
        {
            //Binary value string includes 0x
            //
            return binaryValue.Length - 2 > 16000;
        }

        /// <summary>
        /// Used in instantiating fragments.
        /// </summary>
        public TSqlFragmentFactory FragmentFactory
        {
            get
            {
                return _fragmentFactory;
            }
        }

        /// <summary>
        /// Indicates if the parser is in Phase One parsing mode.
        /// </summary>
        public bool PhaseOne
        {
            get
            {
                return _phaseOne;
            }
            set
            {
                _phaseOne = value;
            }
        }

        /// <summary>
        /// Adds a parse error to the list/
        /// </summary>
        /// <param name="parseError">The error to be added.</param>
        protected void AddParseError(ParseError parseError)
        {
            _parseErrors.Add(parseError);
        }

        /// <summary>
        /// Recovers from an error at the statement level by
        /// consuming (skipping over) tokens until one of the tokens in _errorRecoveryTokens.
        /// </summary>
        /// <param name="statementStartLine">The statement start line.</param>
        /// <param name="statementStartColumn">The statement start column.</param>
        protected void RecoverAtStatementLevel(int statementStartLine, int statementStartColumn)
        {
            consumeUntil(_statementLevelRecoveryTokens);
            int nextTokenLine = LT(1).getLine();
            int nextTokenColumn = LT(1).getColumn();
            if (nextTokenLine == statementStartLine &&
                nextTokenColumn == statementStartColumn)
            {
                if (PhaseOne &&
                    _phaseOnePreviousStatementLevelErrorLine != nextTokenLine &&
                    _phaseOnePreviousStatementLevelErrorColumn != nextTokenColumn)
                {
                    _phaseOnePreviousStatementLevelErrorLine = nextTokenLine;
                    _phaseOnePreviousStatementLevelErrorColumn = nextTokenColumn;
                    // We have recovered at a possible DDL statement, re-enter the script.
                    throw new PhaseOneBatchException();
                }
                else
                {
                    consume();
                }
            }
        }

        /// <summary>
        /// If the first token is something we expect as a DDL statement than skip 
        /// that one in order to avoid infinite recursion (it would have never 
        /// caused an exception if it could be parsed).
        /// </summary>
        protected void SkipInitialDdlTokens()
        {
            if (_ddlStatementBeginnerTokens.member(LA(1)))
            {
                consume();
            }
        }

        /// <summary>
        /// Recovers from an error at the batch level by
        /// consuming (skipping over) tokens until a GO.
        /// </summary>
        protected void RecoverAtBatchLevel()
        {
            if (PhaseOne == true)
            {
                // The initial DDL tokens are skipped, if they could be understood, we would not hit an error 
                // at the batch level.
                SkipInitialDdlTokens();
                // Consume until the end of the batch or one of the P1 statement starters.
                consumeUntil(_phaseOneBatchLevelRecoveryTokens);
                if ((LA(1) != TSql80ParserInternal.Go) && (LA(1) != TSql80ParserInternal.EOF))
                {
                    // We have recovered at a possible DDL statement, re-enter the script.
                    throw new PhaseOneBatchException();
                }
            }
            else
            {
                // Recover at GO, or EOF(implicit).
                consumeUntil(TSql80ParserInternal.Go);
            }
        }

        /// <summary>
        /// If the parser is in phase one mode, the statement is wrapped in PhaseOnePartialAstException
        /// and thrown.
        /// </summary>
        /// <param name="statement">The partial AST.</param>
        protected void ThrowPartialAstIfPhaseOne(TSqlStatement statement)
        {
            if (PhaseOne == true)
                throw new PhaseOnePartialAstException(statement);
        }

        /// <summary>
        /// If the parser is in phase one mode, the constraint is wrapped in PhaseOneConstraintException
        /// and thrown.
        /// </summary>
        /// <param name="constraint">The constraint.</param>
        protected void ThrowConstraintIfPhaseOne(ConstraintDefinition constraint)
        {
            if (PhaseOne == true)
                throw new PhaseOneConstraintException(constraint);
        }

        /// <summary>
        /// Checks if the next tokens text is the same as the keyword in a case insensitive way.
        /// </summary>
        /// <param name="keyword">The keyword to check against.</param>
        /// <returns></returns>
        protected bool NextTokenMatches(string keyword)
        {
            return (LA(1) != TSql80ParserInternal.EOF) && (String.Equals(LT(1).getText(), keyword, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Checks if the asked tokens text is the same as the keyword in a case insensitive way.
        /// </summary>
        /// <param name="keyword">The keyword to check against.</param>
        /// <param name="which">The token to look at.</param>
        /// <returns></returns>
        protected bool NextTokenMatches(string keyword, int which)
        {
            return (LA(which) != TSql80ParserInternal.EOF) && (String.Equals(LT(which).getText(), keyword, StringComparison.OrdinalIgnoreCase));
        }

        protected bool NextTokenMatchesOneOf(params string[] keywords)
        {
            if (LA(1) == TSql80ParserInternal.EOF)
                return false;

            string text = LT(1).getText();
            foreach (string keyword in keywords)
            {
                if (String.Equals(keyword, text, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Throws an exception that won't be logged if reached the end of a batch or file.
        /// </summary>
        protected void ThrowIfEndOfFileOrBatch()
        {
            if ((LA(1) == TSql80ParserInternal.EOF) || (LA(1) == TSql80ParserInternal.Go))
            {
                throw new TSqlParseErrorException(null, true);
            }
        }

        /// <summary>
        /// Updates the result with a new expression with left associativity.
        /// </summary>
        /// <param name="result">Expression to be updated.</param>
        /// <param name="expression">New expression to be inserted</param>
        /// <param name="type">The type of the new expression.</param>
        protected void AddBinaryExpression(ref ScalarExpression result, ScalarExpression expression, BinaryExpressionType type)
        {
            Debug.Assert(result != null);
            BinaryExpression binaryExpression = this.FragmentFactory.CreateFragment<BinaryExpression>();
            binaryExpression.FirstExpression = result;
            binaryExpression.SecondExpression = expression;
            binaryExpression.BinaryExpressionType = type;
            result = binaryExpression;
        }

        /// <summary>
        /// Updates the result with a new expression with left associativity.
        /// </summary>
        /// <param name="result">Expression to be updated.</param>
        /// <param name="expression">New expression to be inserted</param>
        /// <param name="type">The type of the new expression.</param>
        protected void AddBinaryExpression(ref BooleanExpression result, BooleanExpression expression, BooleanBinaryExpressionType type)
        {
            Debug.Assert(result != null);
            BooleanBinaryExpression binaryExpression = this.FragmentFactory.CreateFragment<BooleanBinaryExpression>();
            binaryExpression.FirstExpression = result;
            binaryExpression.SecondExpression = expression;
            binaryExpression.BinaryExpressionType = type;
            result = binaryExpression;
        }

        /// <summary>
        /// Creates an empty identifier at token.
        /// </summary>
        /// <param name="token">The token to get the location information.</param>
        /// <returns>The created identifier.</returns>
        protected Identifier GetEmptyIdentifier(antlr.IToken token)
        {
            Identifier identifier = this.FragmentFactory.CreateFragment<Identifier>();
            UpdateTokenInfo(identifier, token);
            identifier.SetIdentifier(string.Empty);
            return identifier;
        }

        /// <summary>
        /// Checks for duplicates in XML FOR clause options 
        /// </summary>
        /// <param name="current">The enum that the newOption will be added.</param>
        /// <param name="newOption">The new option to be added.</param>
        /// <param name="token">The token that was parsed for the new option.</param>
        /// <returns>The aggregated value for options.</returns>
        protected static void CheckXmlForClauseOptionDuplication(XmlForClauseOptions current, XmlForClauseOptions newOption, antlr.IToken token)
        {
            if ((current & newOption) != 0)
                throw GetUnexpectedTokenErrorException(token);

            if ((newOption & XmlForClauseOptions.ElementsAll) != 0 &&
                (current & XmlForClauseOptions.ElementsAll) != 0)
            {
                throw GetUnexpectedTokenErrorException(token);
            }
        }

        /// <summary>
        /// Adds the item to the list, checking with the max. If max is smaller than zero there is no limit. Error is thrown if max is exceeded.
        /// </summary>
        /// <param name="list">List.</param>
        /// <param name="item">Item.</param>
        /// <param name="max">The max items allowed.</param>
        protected static void AddIdentifierToListWithCheck(List<Identifier> list, Identifier item, int max)
        {
            if (list.Count == max)
            {
                throw GetUnexpectedTokenErrorException(item);
            }
            list.Add(item);
        }

        protected static void CheckOptionDuplication(ref long encountered, int newOption, TSqlFragment vOption)
        {
            CheckOptionDuplication(ref encountered, newOption, GetFirstToken(vOption));
        }

        protected static void CheckOptionDuplication(ref long encountered, int newOption, IToken token)
        {
            long newOptionBit = (1L << newOption);
            if ((encountered & newOptionBit) == newOptionBit)
            {
                ThrowParseErrorException("SQL46049", token, TSqlParserResource.SQL46049Message, token.getText());
            }
            encountered |= newOptionBit;
        }

        protected static void CheckOptionDuplication(ref ulong encountered, int newOption, TSqlFragment vOption)
        {
            CheckOptionDuplication(ref encountered, newOption, GetFirstToken(vOption));
        }

        protected static void CheckOptionDuplication(ref ulong encountered, int newOption, IToken token)
        {
            ulong newOptionBit = (1ul << newOption);
            if ((encountered & newOptionBit) == newOptionBit)
            {
                ThrowParseErrorException("SQL46049", token, TSqlParserResource.SQL46049Message, token.getText());
            }
            encountered |= newOptionBit;
        }

        protected IdentifierOrValueExpression IdentifierOrValueExpression(Identifier identifier)
        {
            IdentifierOrValueExpression vIdentifierOrValueExpression = this.FragmentFactory.CreateFragment<IdentifierOrValueExpression>();
            vIdentifierOrValueExpression.Identifier = identifier;
            return vIdentifierOrValueExpression;
        }

        protected IdentifierOrValueExpression IdentifierOrValueExpression(ValueExpression valueExpression)
        {
            IdentifierOrValueExpression vIdentifierOrValueExpression = this.FragmentFactory.CreateFragment<IdentifierOrValueExpression>();
            vIdentifierOrValueExpression.ValueExpression = valueExpression;
            return vIdentifierOrValueExpression;
        }

        /// <summary>
        /// Parses the input to determine the literal type.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected static OdbcLiteralType ParseOdbcLiteralType(antlr.IToken token)
        {
            if (TryMatch(token, CodeGenerationSupporter.T))
            {
                return OdbcLiteralType.Time;
            }
            else if (TryMatch(token, CodeGenerationSupporter.D))
            {
                return OdbcLiteralType.Date;
            }
            else if (TryMatch(token, CodeGenerationSupporter.TS))
            {
                return OdbcLiteralType.Timestamp;
            }
            else if (TryMatch(token, CodeGenerationSupporter.Guid))
            {
                return OdbcLiteralType.Guid;
            }
            else
            {
                throw GetUnexpectedTokenErrorException(token);
            }
        }

        /// <summary>
        /// Parses Join optimizer hint
        /// </summary>
        /// <param name="token">A token that should represent "MERGE", "HASH" or "LOOP",
        /// otherwise an invalid token exception will be generated</param>
        /// <returns>The hint specified by token.</returns>
        protected static OptimizerHintKind ParseJoinOptimizerHint(antlr.IToken token)
        {
            Debug.Assert(token.Type == TSql80ParserInternal.Identifier);

            switch (token.getText().ToUpperInvariant())
            {
                case CodeGenerationSupporter.Merge:
                    return OptimizerHintKind.MergeJoin;
                case CodeGenerationSupporter.Hash:
                    return OptimizerHintKind.HashJoin;
                case CodeGenerationSupporter.Loop:
                    return OptimizerHintKind.LoopJoin;
                default:
                    throw GetUnexpectedTokenErrorException(token);
            }
        }

        /// <summary>
        /// Parses Union optimizer hint
        /// </summary>
        /// <param name="token">A token that should represent "CONCAT", "HASH", "MERGE" or "KEEP",
        /// otherwise an invalid token exception will be generated</param>
        /// <returns>The hint specified by token.</returns>
        protected static OptimizerHintKind ParseUnionOptimizerHint(antlr.IToken token)
        {
            Debug.Assert(token.Type == TSql80ParserInternal.Identifier);

            switch (token.getText().ToUpperInvariant())
            {
                case CodeGenerationSupporter.Concat:
                    return OptimizerHintKind.ConcatUnion;
                case CodeGenerationSupporter.Hash:
                    return OptimizerHintKind.HashUnion;
                case CodeGenerationSupporter.Merge:
                    return OptimizerHintKind.MergeUnion;
                case CodeGenerationSupporter.Keep:
                    return OptimizerHintKind.KeepUnion;
                default:
                    throw GetUnexpectedTokenErrorException(token);
            }
        }

        /// <summary>
        /// Checks if the next rule is a Select Parenthesis.
        /// The logic is if there is a Join, Inner, Full, Cross, Union, Except, Intersect
        /// when one parenthesis is open, the first parenthesis belongs to a select.
        /// </summary>
        /// <returns>True if the next rule is a join.</returns>
        protected bool IsNextRuleSelectParenthesis()
        {
            bool matches = false;

            if (LA(1) == TSql80ParserInternal.LeftParenthesis && LA(2) == TSql80ParserInternal.Select)
                return true;

            // So it starts with a paranthesis, mark and enter the main algorithm.
            int markSpot = mark();
            consume();
            int openParens = 1;
            for (bool loop = true; loop == true; consume())
            {
                switch (LA(1))
                {
                    case TSql80ParserInternal.LeftParenthesis:
                        ++openParens;
                        break;
                    case TSql80ParserInternal.RightParenthesis:
                        --openParens;
                        if (openParens == 0)
                        {
                            // we ended without matching.
                            loop = false;
                        }
                        break;
                    case TSql80ParserInternal.EOF:
                        // we ended without matching.
                        loop = false;
                        break;
                    case TSql80ParserInternal.Join:
                    case TSql80ParserInternal.Inner:
                    case TSql80ParserInternal.Full:
                    case TSql80ParserInternal.Cross:
                    case TSql80ParserInternal.Outer:
                    case TSql80ParserInternal.Union:
                    case TSql80ParserInternal.Except:
                    case TSql80ParserInternal.Intersect:
                        // Only if we are at the top level, # of open paranthesis should be one.
                        if (openParens == 1)
                        {
                            matches = true;
                            loop = false;
                        }
                        break;
                    default:
                        // Anything else is ignored.
                        break;
                }
            }
            rewind(markSpot);
            return matches;
        }

        /// <summary>
        /// Checks if the next rule is a boolean parenthesis.
        /// The logic is if one of the tokens that is used in boolean expression is
        /// seen the expression is a boolean expression.
        /// Boolean expressions can appear in case expressions, so if we enter a case 
        /// expression, the whole expression will not be classified boolean until 
        /// we exit from the case expression.
        /// Boolean expressions can appear in select, so if there is a select in the open parans level or below, seeing a
        /// boolean operand does not matter.
        /// </summary>
        /// <returns>True if the next rule is a boolean parenthesis.</returns>
        protected bool IsNextRuleBooleanParenthesis()
        {
            // Fail fast
            if (LA(1) != TSql80ParserInternal.LeftParenthesis)
            {
                return false;
            }

            bool matches = false;

            // So it starts with a paranthesis, mark and enter the main algorithm.
            int markSpot = mark();
            consume();
            int openParens = 1;
            int caseDepth = 0;
            // 0 means there was no select
            int topmostSelect = 0;
            int insideIIf = 0;

            for (bool loop = true; loop == true; consume())
            {
                switch (LA(1))
                {
                    case TSql80ParserInternal.Identifier:
                        // if identifier is IIF
                        if(NextTokenMatches(CodeGenerationSupporter.IIf))
                        {
                            ++insideIIf;
                        }
                        break;
                    case TSql80ParserInternal.LeftParenthesis:
                        ++openParens;
                        break;
                    case TSql80ParserInternal.RightParenthesis:
                        if (openParens == topmostSelect)
                        {
                            topmostSelect = 0;
                        }

                        --openParens;
                        if (openParens == 0)
                        {
                            // we ended without matching.
                            loop = false;
                        }
                        break;
                    case TSql80ParserInternal.EOF:
                        // we ended without matching.
                        loop = false;
                        break;
                    case TSql80ParserInternal.And:
                    case TSql80ParserInternal.Or:
                    case TSql80ParserInternal.Not:
                    case TSql80ParserInternal.EqualsSign:
                    case TSql80ParserInternal.GreaterThan:
                    case TSql80ParserInternal.LessThan:
                    case TSql80ParserInternal.Bang:
                    case TSql80ParserInternal.MultiplyEquals:
                    case TSql80ParserInternal.RightOuterJoin:
                    case TSql80ParserInternal.Is:
                    case TSql80ParserInternal.In:
                    case TSql80ParserInternal.Like:
                    case TSql80ParserInternal.Between:
                    case TSql80ParserInternal.Contains:
                    case TSql80ParserInternal.FreeText:
                    case TSql80ParserInternal.Exists:
                    case TSql80ParserInternal.TSEqual:
                    case TSql80ParserInternal.Update:
                        if (caseDepth == 0 && topmostSelect == 0 && insideIIf == 0)
                        {
                            // The number of open paranthesis are not important.
                            // Unless inside an iff
                            matches = true;
                            loop = false;
                        }
                        else if (insideIIf > 0)
                        {
                            // Found the operator inside IIF
                            --insideIIf;
                        }
                        break;
                    case TSql80ParserInternal.Case:
                        ++caseDepth;
                        break;
                    case TSql80ParserInternal.End:
                        --caseDepth;
                        break;
                    case TSql80ParserInternal.Select:
                        if (topmostSelect == 0)
                        {
                            topmostSelect = openParens;
                        }
                        break;
                    default:
                        // Anything else is ignored.
                        break;
                }
            }
            rewind(markSpot);
            return matches;
        }

        #region Lookahead Utilities

        /// <summary>
        /// In the guessing mode (i.e. within a syntactic predicate), returns the current 
        /// lookahead token; otherwise, does nothing.
        /// <para>
        /// Use it like the following:
        /// <code>
        /// your_rule { IToken marker = null; } :
        /// (
        ///     ({ if (!SkipGuessing(marker)) } : your_rule ({ SaveGuessing(out marker); } : ))=> 
        ///     ({ if (!SkipGuessing(marker)) } : your_rule)
        ///     | ...
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="marker">When the method returns, contains the current lookahead token 
        /// or the null value, if called in the non-guessing mode (i.e. not within a syntactic 
        /// predicate).</param>
        /// <returns>true in the guessing mode; otherwise, false.</returns>
        /// <seealso cref="SkipGuessing"/>
        protected bool SaveGuessing(out IToken marker)
        {
            marker = null;

            // Do not proceed if called in the non-guessing mode
            if (inputState.guessing == 0)
                return false;

            // Return the current lookahead token
            marker = LT(1);

            return true;

        }

        /// <summary>
        /// In the guessing mode (i.e. within a syntactic predicate), advances the input position 
        /// to the specified token, which becomes the current lookahead token; otherwise, does 
        /// nothing.
        /// <para>
        /// Use it like the following:
        /// <code>
        /// your_rule { IToken marker = null; } :
        /// (
        ///     ({ if (!SkipGuessing(marker)) } : your_rule ({ SaveGuessing(out marker); } : ))=> 
        ///     ({ if (!SkipGuessing(marker)) } : your_rule)
        ///     | ...
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="marker">The token to advance the input position to.</param>
        /// <returns>true in the guessing mode; otherwise, false.</returns>
        /// <remarks>
        /// <para>
        /// The primary usage of this method is to break recursive exponential lookaheads. It is 
        /// a rare case when a recursive rule is ambiguous and a conflict is resolved by the full
        /// syntactic predicate (aka lookahead) for the rule.
        /// </para>
        /// <para>
        /// Consider the following example:
        /// <code>
        ///     outer: '(' ((outer)=> outer | inner) ')'
        /// </code>
        ///    Each time "outer" is parsed, the nested one is parsed twice, first for the lookahead 
        ///    and then for real. This happens at each level of nesting, so the overall complexity 
        ///    becomes O(2^n), where n is the number of nested "outer".
        ///    </para>
        ///    <para>
        /// Sometimes, it is possible to redesign the grammar and avoid such a lookahead (e.g. 
        /// using left factoring). But there are cases when we do not want to significantly change 
        /// the grammar or the language is just non-LL(k). When so, this method provides a way to 
        /// improve the performance to O(n^2), which is suitable for most practical cases. The idea
        /// it is to skip redundant lookahead branches. Let's consider the above example. ANTLR 
        /// first parses "outer" for the lookahead, i.e. actions are turned off. Then, if no errors, 
        /// it parsers "outer" again, now with actions turned on. However, if this is happening 
        /// within an upper level lookahead, actions are actually turned off for both parsings and 
        /// there is no any difference between them. So, we can skip the second parsing and simply 
        /// consume the needed number of tokens. Here is how it looks like when expanded (outer_LA 
        /// means parsing for lookahead, actions turned off):
        /// <code>
        ///     outer
        ///         outer_LA
        ///             outer_LA
        ///                 outer_LA
        ///                     ...
        ///                 outer
        ///                     ...
        ///             outer
        ///                 outer_LA
        ///                     ...
        ///                 outer
        ///                     ...
        ///         outer
        ///             outer_LA
        ///                 ...
        ///             outer
        ///                 ...
        /// </code>
        /// Each time "outer" appears under "outer_LA" it is redundant and can be skipped. And here 
        /// is a code snippet doing this:
        /// <code>
        /// outer { IToken marker = null; } :
        /// (
        ///     (outer 
        ///         // Save the token following "outer".
        ///         ({ SaveGuessing(out marker); } : )) => 
        ///     // If within a lookahead, simply consume tokens until the one following "outer"; 
        ///     // otherwise, do a regular parsing
        ///     ({ if (!SkipGuessing(marker)) } :
        ///         outer)
        ///     |
        ///     ...
        /// </code>
        /// Note that all the processing is done within rule/subrule initialization blocks instead 
        /// of regular actions. As mentioned, actions are ignored within a lookahead; however, 
        /// initialization blocks are always executed.
        ///    </para>
        /// </remarks>
        /// <seealso cref="SaveGuessing"/>
        protected bool SkipGuessing(IToken marker)
        {
            // Do not proceed if there is nothing to consume
            if (marker == null)
                return false;

            // Do not proceed if called in the non-guessing mode
            if (inputState.guessing == 0)
                return false;

            // Consume all tokens until the specified one
            // Note that below we check for EOF, not just for the marker. It is supposed to prevent 
            // a "catastrophic" behavior when method is misused. ANTLR works in such a way that, 
            // having reached EOF once, it is repeatedly returned for all subsequent calls to LT() 
            // and LA(). That means, without a check for EOF, the below loop is infinite when can 
            // not find the marker. 
            while (LA(1) != Token.EOF_TYPE && LT(1) != marker)
                consume();
            
            Debug.Assert(marker == LT(1));

            return true;
        }

        #endregion Lookahead Utilities

        #region Context Sensitive Keyword Matchers

        /// <summary>
        /// Used by other private functions of the form MatchX. If the text
        /// of the token does not match the keyword, an exception is thrown.
        /// </summary>
        /// <param name="token">The token from lexer.</param>
        /// <param name="keyword">The keyword it is matching.</param>
        protected static void Match(antlr.IToken token, string keyword)
        {
            if (!String.Equals(token.getText(), keyword, StringComparison.OrdinalIgnoreCase))
            {
                ThrowParseErrorException("SQL46005", token,
                    TSqlParserResource.SQL46005Message, keyword, token.getText());
            }
        }

        /// <summary>
        /// Checks, if passed identifier value matches constant.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="constant">The constant.</param>
        protected static void Match(Identifier id, string constant)
        {
            if (!String.Equals(id.Value, constant, StringComparison.OrdinalIgnoreCase))
                throw GetUnexpectedTokenErrorException(id);
        }

        /// <summary>
        /// Checks, if passed identifier value matches constant.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="constant">The constant.</param>
        /// <param name="tokenForError">The token for error.</param>
        protected static void Match(Identifier id, string constant, antlr.IToken tokenForError)
        {
            if (!String.Equals(id.Value, constant, StringComparison.OrdinalIgnoreCase))
                throw GetUnexpectedTokenErrorException(tokenForError);
        }

        /// <summary>
        /// Used by other private functions of the form MatchX. If the text
        /// of the token does not match the one of the keywords, an exception is thrown.
        /// </summary>
        /// <param name="token">The token from lexer.</param>
        /// <param name="keyword">The keyword it is matching.</param>
        /// <param name="alternate">The alternate string to match.</param>
        protected static void Match(antlr.IToken token, string keyword, string alternate)
        {
            if ((String.Equals(token.getText(), keyword, StringComparison.OrdinalIgnoreCase) == false)
                && (String.Equals(token.getText(), alternate, StringComparison.OrdinalIgnoreCase) == false))
            {
                // TODO, sacaglar: a new type of parse error might be required, a variation of SQL46005,
                // Expected one of the keywords from this list: {0}, got {1} instead.
                throw GetUnexpectedTokenErrorException(token);
            }
        }

        /// <summary>
        /// Tries to match token text with keyword
        /// </summary>
        /// <param name="token">The token from lexer.</param>
        /// <param name="keyword">The keyword it is matching.</param>
        /// <returns>True if there was a match.</returns>
        protected static bool TryMatch(antlr.IToken token, string keyword)
        {
            return String.Equals(token.getText(), keyword, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Tries to match token text with keyword
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="keyword">The keyword it is matching.</param>
        /// <returns>True if there was a match.</returns>
        protected static bool TryMatch(Identifier identifier, string keyword)
        {
            return String.Equals(identifier.Value, keyword, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Tries to match string literal with one of predefined values, and throws error if fails
        /// </summary>
        /// <param name="literal">The string litral.</param>
        /// <param name="keywords">Predefined string to match.</param>
        protected static void MatchString(Literal literal, params string[] keywords)
        {
            Debug.Assert(literal.LiteralType == LiteralType.String);
            string stringVal = literal.Value;
            foreach (string kw in keywords)
            {
                if (String.Equals(stringVal, kw, StringComparison.OrdinalIgnoreCase))
                    return;
            }
            ThrowIncorrectSyntaxErrorException(GetFirstToken(literal));
        }

        #endregion

        #region Context Sensitive Parse Functions

        /// <summary>
        /// Parses the token into a sql data type
        /// in a case insensitive manner.
        /// Pay attention; cursor and table are not here,
        /// they can't be because they are reserved words,
        /// they will be catched as tokens not identifiers.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected static SqlDataTypeOption ParseDataType(string token)
        {
            switch (token.ToUpperInvariant())
            {
                case "BIGINT":
                    return SqlDataTypeOption.BigInt;
                case "INTEGER":
                case "INT":
                    return SqlDataTypeOption.Int;
                case "SMALLINT":
                    return SqlDataTypeOption.SmallInt;
                case "TINYINT":
                    return SqlDataTypeOption.TinyInt;
                case "BIT":
                    return SqlDataTypeOption.Bit;
                case "DEC":
                case "DECIMAL":
                    return SqlDataTypeOption.Decimal;
                case "NUMERIC":
                    return SqlDataTypeOption.Numeric;
                case "MONEY":
                    return SqlDataTypeOption.Money;
                case "SMALLMONEY":
                    return SqlDataTypeOption.SmallMoney;
                case "FLOAT":
                    return SqlDataTypeOption.Float;
                case "REAL":
                    return SqlDataTypeOption.Real;
                case "DATETIME":
                    return SqlDataTypeOption.DateTime;
                case "SMALLDATETIME":
                    return SqlDataTypeOption.SmallDateTime;
                case "CHARACTER":
                case "CHAR":
                    return SqlDataTypeOption.Char;
                case "VARCHAR":
                    return SqlDataTypeOption.VarChar;
                case "TEXT":
                    return SqlDataTypeOption.Text;
                case "NCHAR":
                case "NCHARACTER":
                    return SqlDataTypeOption.NChar;
                case "NVARCHAR":
                    return SqlDataTypeOption.NVarChar;
                case "NTEXT":
                    return SqlDataTypeOption.NText;
                case "BINARY":
                    return SqlDataTypeOption.Binary;
                case "VARBINARY":
                    return SqlDataTypeOption.VarBinary;
                case "IMAGE":
                    return SqlDataTypeOption.Image;
                case "CURSOR":
                    return SqlDataTypeOption.Cursor;
                case "SQL_VARIANT":
                    return SqlDataTypeOption.Sql_Variant;
                case "TABLE":
                    return SqlDataTypeOption.Table;
                case "ROWVERSION":
                    return SqlDataTypeOption.Rowversion;
                case "TIMESTAMP":
                    return SqlDataTypeOption.Timestamp;
                case "UNIQUEIDENTIFIER":
                    return SqlDataTypeOption.UniqueIdentifier;
                default:
                    return SqlDataTypeOption.None;
            }
        }

        /// <summary>
        /// Classifies a token to a legacy Index WithOption.
        /// </summary>
        /// <param name="token">Token to be classified.</param>
        /// <returns>The with option.</returns>
        protected static IndexOptionKind ParseIndexLegacyWithOption(antlr.IToken token)
        {
            IndexOptionKind indexOption;
            if (!IndexOptionHelper.Instance.TryParseOption(token, SqlVersionFlags.TSql80, out indexOption))
            {
                ThrowParseErrorException("SQL46015", token,
                    TSqlParserResource.SQL46015Message, token.getText());
            }

            return indexOption;
        }

        private static Dictionary<IndexAffectingStatement, string> _indexOptionContainerStatementNames = new Dictionary<IndexAffectingStatement, string>
            {
                { IndexAffectingStatement.AlterTableAddElement, "ALTER TABLE" },
                { IndexAffectingStatement.AlterTableRebuildAllPartitions, "ALTER TABLE REBUILD PARTITION" },
                { IndexAffectingStatement.AlterTableRebuildOnePartition, "ALTER TABLE REBUILD PARTITION" },
                { IndexAffectingStatement.AlterIndexRebuildAllPartitions, "ALTER INDEX REBUILD PARTITION" },
                { IndexAffectingStatement.AlterIndexRebuildOnePartition, "ALTER INDEX REBUILD PARTITION" },
                { IndexAffectingStatement.AlterIndexSet, "ALTER INDEX" },
                { IndexAffectingStatement.AlterIndexReorganize, "ALTER INDEX REORGANIZE" },
                { IndexAffectingStatement.CreateColumnStoreIndex, "CREATE COLUMNSTORE INDEX" },
                { IndexAffectingStatement.CreateIndex, "CREATE INDEX" },
                { IndexAffectingStatement.CreateTable, "CREATE TABLE" },
                { IndexAffectingStatement.CreateTableInlineIndex, "CREATE TABLE (inline index)" },
                { IndexAffectingStatement.CreateType, "CREATE TYPE" },
                { IndexAffectingStatement.CreateXmlIndex, "CREATE XML INDEX" },
                { IndexAffectingStatement.CreateOrAlterFunction, "CREATE/ALTER FUNCTION" },
                { IndexAffectingStatement.DeclareTableVariable, "DECLARE" },
                { IndexAffectingStatement.CreateSpatialIndex, "CREATE SPATIAL INDEX" },
                { IndexAffectingStatement.AlterTableAlterIndexRebuild, "ALTER TABLE ALTER INDEX REBUILD" },
                { IndexAffectingStatement.AlterTableAlterColumn, "ALTER TABLE ALTER COLUMN" },
                { IndexAffectingStatement.AlterIndexResume, "ALTER INDEX RESUME" }
            };

        protected static void ThrowWrongIndexOptionError(IndexAffectingStatement statement, TSqlFragment option)
        {
            string optionName = string.Empty;
            if (option.FirstTokenIndex >= 0 && option.ScriptTokenStream != null &&
                option.FirstTokenIndex < option.ScriptTokenStream.Count)
            {
                TSqlParserToken token = option.ScriptTokenStream[option.FirstTokenIndex];
                optionName = token.Text;
            }
            else
            {
                // Somehow we don't have token stream, or positions are not set correctly
                Debug.Assert(false);
            }

            string statementName;
            if (!_indexOptionContainerStatementNames.TryGetValue(statement, out statementName))
            {
                Debug.Assert(false); // Unknown enum value!
                statementName = string.Empty;
            }

            ThrowParseErrorException("SQL46057", option,
                TSqlParserResource.SQL46057Message, optionName, statementName);
        }

        protected static void CheckFillFactorRange(Literal value)
        {
            int convertedValue;
            if (!Int32.TryParse(value.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out convertedValue) ||
                convertedValue < 1 || convertedValue > 100)
            {
                ThrowParseErrorException("SQL46060", value, TSqlParserResource.SQL46060Message, value.Value);
            }
        }

        protected static void CheckIdentifierLength(Identifier value)
        {
            if (value.Value.Length > Identifier.MaxIdentifierLength)
            {
                ThrowParseErrorException("SQL46095", value, TSqlParserResource.SQL46095Message, value.Value.Substring(0, 128));
            }
        }

        protected static void CheckIdentifierLiteralLength(IdentifierLiteral value)
        {
            if (value.Value.Length > Identifier.MaxIdentifierLength)
            {
                ThrowParseErrorException("SQL46095", value, TSqlParserResource.SQL46095Message, value.Value.Substring(0, 128));
            }
        }

        protected static void ThrowIfPercentValueOutOfRange(ScalarExpression expr)
        {
            Double convertedValue;
            Literal literalValue = null;
            if (expr is ParenthesisExpression)
            {
                ParenthesisExpression pExpr = expr as ParenthesisExpression;
                if (pExpr != null)
                {
                    ThrowIfPercentValueOutOfRange(pExpr.Expression);
                }
            }
            else if (expr is UnaryExpression)
            {
                UnaryExpression unaryExpr = expr as UnaryExpression;
                if (unaryExpr != null)
                {
                    if (unaryExpr.UnaryExpressionType == UnaryExpressionType.Negative)
                    {
                        ThrowParseErrorException("SQL46094", expr, TSqlParserResource.SQL46094Message);
                    }
                    else
                    {
                        ThrowIfPercentValueOutOfRange(unaryExpr.Expression);
                    }
                }
            }
            else
            {
                literalValue = expr as Literal;
                if (literalValue != null &&
                   (literalValue.LiteralType == LiteralType.Real ||
                   literalValue.LiteralType == LiteralType.Numeric ||
                    literalValue.LiteralType == LiteralType.Integer))
                {
                    if (!Double.TryParse(literalValue.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out convertedValue) ||
                        convertedValue < 0 || convertedValue > 100)
                    {
                        ThrowParseErrorException("SQL46094", expr, TSqlParserResource.SQL46094Message);
                    }
                }
            }
        }

        protected static void VerifyAllowedIndexOption(IndexAffectingStatement statement, IndexOption option)
        {
            VerifyAllowedIndexOption(statement, option, SqlVersionFlags.None);
        }

        private static readonly List<IndexAffectingStatement> StatementsWithBucketCount = new List<IndexAffectingStatement>()
        {
            IndexAffectingStatement.CreateTable,
            IndexAffectingStatement.CreateTableInlineIndex,
            IndexAffectingStatement.CreateType,
            IndexAffectingStatement.AlterTableAlterIndexRebuild
        };

        protected static void VerifyAllowedIndexOption(IndexAffectingStatement statement, IndexOption option, SqlVersionFlags versionFlags)
        {
            bool invalidOption = false;

            if (option.OptionKind == IndexOptionKind.FileStreamOn &&
                statement != IndexAffectingStatement.AlterTableAddElement)
            {
                invalidOption = true;
            }            

            if (option.OptionKind == IndexOptionKind.BucketCount &&
                !StatementsWithBucketCount.Contains(statement))
            {
                invalidOption = true;
            }

            bool columnStoreIndexDataCompressionOptionLevels = false;
            bool indexDataCompressionOptionLevels = false;
            if (option.OptionKind == IndexOptionKind.DataCompression)
            {
                DataCompressionOption dcOption = option as DataCompressionOption;
                columnStoreIndexDataCompressionOptionLevels = dcOption.CompressionLevel == DataCompressionLevel.ColumnStore ||
                               dcOption.CompressionLevel == DataCompressionLevel.ColumnStoreArchive;

                indexDataCompressionOptionLevels = dcOption.CompressionLevel == DataCompressionLevel.None ||
                                dcOption.CompressionLevel == DataCompressionLevel.Row ||
                                dcOption.CompressionLevel == DataCompressionLevel.Page;

                // Column store compression levels are only supported in 120 and above
                if ((versionFlags & SqlVersionFlags.TSql120AndAbove) == 0 && columnStoreIndexDataCompressionOptionLevels)
                {
                    invalidOption = true;
                }
            }

            // XML compression is only supported for 160 and above
            if (option.OptionKind == IndexOptionKind.XmlCompression)
            {
                if ((versionFlags & SqlVersionFlags.TSql160AndAbove) == 0)
                {
                    invalidOption = true;
                }
            }

            switch (statement)
            {
                case IndexAffectingStatement.AlterIndexRebuildOnePartition:
                    if (option.OptionKind != IndexOptionKind.SortInTempDB &&
                        option.OptionKind != IndexOptionKind.MaxDop &&
                        option.OptionKind != IndexOptionKind.DataCompression &&
                        option.OptionKind != IndexOptionKind.XmlCompression &&
                        option.OptionKind != IndexOptionKind.Resumable &&
                        option.OptionKind != IndexOptionKind.MaxDuration &&
                        (option.OptionKind != IndexOptionKind.Online || (versionFlags & SqlVersionFlags.TSql120AndAbove) == 0))
                    {
                        invalidOption = true;
                    }
                    break;
                case IndexAffectingStatement.AlterTableRebuildOnePartition:
                    if (option.OptionKind != IndexOptionKind.SortInTempDB &&
                        option.OptionKind != IndexOptionKind.MaxDop &&
                        option.OptionKind != IndexOptionKind.DataCompression &&
                        option.OptionKind != IndexOptionKind.XmlCompression &&
                        (option.OptionKind != IndexOptionKind.Online || (versionFlags & SqlVersionFlags.TSql120AndAbove) == 0)) // SPOIR
                    {
                        invalidOption = true;
                    }
                    break;
                case IndexAffectingStatement.AlterIndexRebuildAllPartitions:
                    if (option.OptionKind == IndexOptionKind.DropExisting ||
                        option.OptionKind == IndexOptionKind.LobCompaction ||
                        option.OptionKind == IndexOptionKind.Order ||
                        option.OptionKind == IndexOptionKind.OptimizeForSequentialKey)
                    {
                        invalidOption = true;
                    }
                    break;
                case IndexAffectingStatement.AlterTableRebuildAllPartitions:
                    if (option.OptionKind == IndexOptionKind.DropExisting ||
                        option.OptionKind == IndexOptionKind.LobCompaction ||
                        option.OptionKind == IndexOptionKind.Order || 
                        option.OptionKind == IndexOptionKind.Resumable ||
                        option.OptionKind == IndexOptionKind.MaxDuration ||
                        option.OptionKind == IndexOptionKind.OptimizeForSequentialKey)
                    {
                        invalidOption = true;
                    }
                    break;
                case IndexAffectingStatement.AlterIndexReorganize:
                    if (option.OptionKind != IndexOptionKind.LobCompaction &&
                       (option.OptionKind != IndexOptionKind.CompressAllRowGroups || (versionFlags & SqlVersionFlags.TSql130AndAbove) == 0 ))
                        invalidOption = true;
                    break;
                case IndexAffectingStatement.AlterIndexSet:
                    if (option.OptionKind != IndexOptionKind.AllowRowLocks &&
                        option.OptionKind != IndexOptionKind.AllowPageLocks &&
                        option.OptionKind != IndexOptionKind.IgnoreDupKey &&
                        option.OptionKind != IndexOptionKind.StatisticsNoRecompute &&
                        (option.OptionKind != IndexOptionKind.CompressionDelay || (versionFlags & SqlVersionFlags.TSql130AndAbove) == 0 ) &&
                        (option.OptionKind != IndexOptionKind.OptimizeForSequentialKey || (versionFlags & SqlVersionFlags.TSql150AndAbove) == 0 ))
                    {
                        invalidOption = true;
                    }
                    break;
                case IndexAffectingStatement.AlterIndexResume:
                    if (option.OptionKind != IndexOptionKind.MaxDop &&
                        option.OptionKind != IndexOptionKind.MaxDuration &&
                        option.OptionKind != IndexOptionKind.WaitAtLowPriority)
                    {
                        invalidOption = true;
                    }
                    break;
                case IndexAffectingStatement.AlterTableAddElement:
                    if (option.OptionKind == IndexOptionKind.DropExisting ||
                        option.OptionKind == IndexOptionKind.LobCompaction ||
                        option.OptionKind == IndexOptionKind.Order || 
                        option.OptionKind == IndexOptionKind.Resumable ||
                        option.OptionKind == IndexOptionKind.MaxDuration)
                    {
                        invalidOption = true;
                    }
                    break;
                case IndexAffectingStatement.CreateTable:
                case IndexAffectingStatement.DeclareTableVariable:
                case IndexAffectingStatement.CreateOrAlterFunction:
                    if (option.OptionKind == IndexOptionKind.SortInTempDB ||
                        option.OptionKind == IndexOptionKind.Online ||
                        option.OptionKind == IndexOptionKind.MaxDop ||
                        option.OptionKind == IndexOptionKind.LobCompaction ||
                        option.OptionKind == IndexOptionKind.DropExisting ||
                        option.OptionKind == IndexOptionKind.Order ||
                        option.OptionKind == IndexOptionKind.Resumable ||
                        option.OptionKind == IndexOptionKind.MaxDuration)
                    {
                        invalidOption = true;
                    }
                    else if (option.OptionKind == IndexOptionKind.DataCompression)
                    {
                        //Specifying COLUMNSTORE or COLUMNSTORE_ARCHIVE as the data compression for create table
                        //is a syntax error
                        if (columnStoreIndexDataCompressionOptionLevels)
                        {
                            invalidOption = true;
                        }
                    }
                    break;
                case IndexAffectingStatement.CreateColumnStoreIndex:
                    if (option.OptionKind == IndexOptionKind.DataCompression)
                    {
                        if ((versionFlags & SqlVersionFlags.TSql120AndAbove) == 0 ||
                            indexDataCompressionOptionLevels)
                        {
                            invalidOption = true;
                        }
                    }
                    else if (option.OptionKind == IndexOptionKind.XmlCompression)
                    {
                        invalidOption = true;
                    }
                    else if (option.OptionKind == IndexOptionKind.SortInTempDB ||
                             option.OptionKind == IndexOptionKind.Order ||
                             option.OptionKind == IndexOptionKind.CompressionDelay)
                    {
                        if ((versionFlags & SqlVersionFlags.TSql130AndAbove) == 0)
                        {
                            invalidOption = true;
                        }
                    }
                    else if (option.OptionKind == IndexOptionKind.Online)
                    {
                        if ((versionFlags & SqlVersionFlags.TSql140AndAbove) == 0)
                        {
                            invalidOption = true;
                        }
                    }
                    else if (option.OptionKind != IndexOptionKind.DropExisting &&
                             option.OptionKind != IndexOptionKind.MaxDop)
                    {
                        invalidOption = true;
                    }
                    break;
                case IndexAffectingStatement.CreateType:
                    if (option.OptionKind != IndexOptionKind.IgnoreDupKey &&
                        option.OptionKind != IndexOptionKind.BucketCount)
                        invalidOption = true;
                    break;
                case IndexAffectingStatement.CreateIndex:
                case IndexAffectingStatement.CreateTableInlineIndex:
                    if (option.OptionKind == IndexOptionKind.LobCompaction ||
                        option.OptionKind == IndexOptionKind.Order ||
                        ((versionFlags & SqlVersionFlags.TSql150AndAbove) == 0 && option.OptionKind == IndexOptionKind.Resumable) ||
                        ((versionFlags & SqlVersionFlags.TSql150AndAbove) == 0 && option.OptionKind == IndexOptionKind.MaxDuration))
                    {
                        invalidOption = true;
                    }
                    else if (option.OptionKind == IndexOptionKind.DataCompression)
                    {
                        //Specifying COLUMNSTORE or COLUMNSTORE_ARCHIVE as the data compression for create (non-columnstore) index
                        //is a syntax error
                        if (columnStoreIndexDataCompressionOptionLevels)
                        {
                            invalidOption = true;
                        }
                    }
                    break;
                case IndexAffectingStatement.CreateXmlIndex:
                    if (option.OptionKind == IndexOptionKind.DataCompression ||
                        option.OptionKind == IndexOptionKind.LobCompaction ||
                        option.OptionKind == IndexOptionKind.CompressAllRowGroups ||
                        option.OptionKind == IndexOptionKind.CompressionDelay ||
                        option.OptionKind == IndexOptionKind.Resumable ||
                        option.OptionKind == IndexOptionKind.MaxDuration ||
                        option.OptionKind == IndexOptionKind.OptimizeForSequentialKey)
                    {
                        invalidOption = true;
                    }
                    else if (option.OptionKind == IndexOptionKind.IgnoreDupKey)
                    {
                        IndexStateOption indexStateOption = option as IndexStateOption;
                        if (indexStateOption != null)
                        {
                            invalidOption = indexStateOption.OptionState == OptionState.On;
                        }
                    }
                    break;
                case IndexAffectingStatement.CreateSpatialIndex:
                    if (option.OptionKind == IndexOptionKind.DataCompression)
                    {
                        if ((versionFlags & SqlVersionFlags.TSql110AndAbove) == 0 ||
                            columnStoreIndexDataCompressionOptionLevels)
                        {
                            invalidOption = true;
                        }
                    }
                    else if (option.OptionKind == IndexOptionKind.LobCompaction ||
                             option.OptionKind == IndexOptionKind.FileStreamOn ||
                             option.OptionKind == IndexOptionKind.CompressAllRowGroups ||
                             option.OptionKind == IndexOptionKind.CompressionDelay ||
                             option.OptionKind == IndexOptionKind.Resumable ||
                             option.OptionKind == IndexOptionKind.MaxDuration ||
                             option.OptionKind == IndexOptionKind.XmlCompression ||
                             option.OptionKind == IndexOptionKind.OptimizeForSequentialKey)
                    {
                        invalidOption = true;
                    }
                    break;
                case IndexAffectingStatement.AlterTableAlterIndexRebuild:
                    if ((versionFlags & SqlVersionFlags.TSql130AndAbove) == 0 ||
                        option.OptionKind != IndexOptionKind.BucketCount)
                    {
                        invalidOption = true;
                    }
                    break;
                case IndexAffectingStatement.AlterTableAlterColumn:
                    if ((versionFlags & SqlVersionFlags.TSql130AndAbove) == 0 ||
                        option.OptionKind != IndexOptionKind.Online)
                    {
                        invalidOption = true;
                    }
                    break;
                default:
                    Debug.Assert(false); // Bug - unknown statement with index options
                    break;
            }

            if (invalidOption)
                ThrowWrongIndexOptionError(statement, option);
        }

        protected static void ThrowSyntaxErrorIfNotCreateAlterTable(IndexAffectingStatement statement, IToken atToken)
        {
            if (statement != IndexAffectingStatement.CreateTable &&
                statement != IndexAffectingStatement.AlterTableAddElement)
            {
                ThrowIncorrectSyntaxErrorException(atToken);
            }
        }

        /// <summary>
        /// Classifies a token to a Create/Alter function statement
        /// </summary>
        /// <param name="token">Token to be classified.</param>
        /// <returns>The with option.</returns>
        protected static FunctionOptionKind ParseAlterCreateFunctionWithOption(antlr.IToken token)
        {
            switch (token.getText().ToUpperInvariant())
            {
                case CodeGenerationSupporter.Encryption:
                    return FunctionOptionKind.Encryption;
                case CodeGenerationSupporter.SchemaBinding:
                    return FunctionOptionKind.SchemaBinding;
                default:
                    throw new TSqlParseErrorException(
                        CreateParseError("SQL46026", token, TSqlParserResource.SQL46026Message, token.getText()));
            }
        }

        /// <summary>
        /// Classifies a token to a Create Statistics WithOption.
        /// </summary>
        /// <param name="token">Token to be classified.</param>
        /// <returns>The with option.</returns>
        protected static StatisticsOptionKind ParseCreateStatisticsWithOption(antlr.IToken token)
        {
            switch (token.getText().ToUpperInvariant())
            {
                case CodeGenerationSupporter.FullScan:
                    return StatisticsOptionKind.FullScan;
                case CodeGenerationSupporter.NoRecompute:
                    return StatisticsOptionKind.NoRecompute;
                default:
                    throw new TSqlParseErrorException(CreateParseError("SQL46018", token, TSqlParserResource.SQL46018Message, token.getText()));
            }
        }

        /// <summary>
        /// Classifies a token to a sample options WithOption.
        /// </summary>
        /// <param name="token">Token to be classified.</param>
        /// <returns>The with option.</returns>
        protected static StatisticsOptionKind ParseSampleOptionsWithOption(antlr.IToken token)
        {
            // Percent is already a reserved word.
            if (String.Compare(CodeGenerationSupporter.Rows, token.getText(), StringComparison.OrdinalIgnoreCase) == 0)
            {
                return StatisticsOptionKind.SampleRows;
            }
            else
            {
                throw new TSqlParseErrorException(
                    CreateParseError("SQL46019", token, TSqlParserResource.SQL46019Message, token.getText()));
            }
        }

        /// <summary>
        /// Classifies trigger enforcement.
        /// </summary>
        /// <param name="token">Token to be classified.</param>
        /// <returns>TriggerEnforcement.</returns>
        protected static TriggerEnforcement ParseTriggerEnforcement(antlr.IToken token)
        {
            switch (token.getText().ToUpperInvariant())
            {
                case CodeGenerationSupporter.Enable:
                    return TriggerEnforcement.Enable;
                case CodeGenerationSupporter.Disable:
                    return TriggerEnforcement.Disable;
                default:
                    // No viable alternative exception is valid here since TRIGGER keyword is parsed after this.
                    throw new antlr.NoViableAltException(token, token.getFilename());
            }
        }

        #endregion

        /// <summary>
        /// Checks that the table name is defined.
        /// </summary>
        /// <param name="column">The column.</param>
        protected static void CheckSpecialColumn(ColumnReferenceExpression column)
        {
            if ((column.ColumnType != ColumnType.Regular) && (column.MultiPartIdentifier != null && column.MultiPartIdentifier.Count >= 4))
            {
                throw new TSqlParseErrorException(
                    CreateParseError("SQL46028", GetFirstToken(column),
                    TSqlParserResource.SQL46028Message));
            }
        }

        /// <summary>
        /// Checks the identifiers in the qualifier for a Select * expression are valid.
        /// </summary>
        /// <param name="column">The column.</param>
        protected static void CheckStarQualifier(SelectStarExpression column)
        {
            if (column.Qualifier != null)
            {
                int count = column.Qualifier.Count;
                if (count >= 4)
                {
                    throw new TSqlParseErrorException(
                        CreateParseError("SQL46028", GetFirstToken(column),
                        TSqlParserResource.SQL46028Message));
                }
                if (count == 0 || (count >= 1 && !String.IsNullOrEmpty(column.Qualifier[count - 1].Value)))
                {
                    return;
                }
                ThrowParseErrorException("SQL46016", column, TSqlParserResource.SQL46016Message);
            }
        }

        /// <summary>
        /// Checks that the table name is defined.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="multiPartRequisite">If this is false, table not being defined is only a problem with multipart names.</param>
        protected static void CheckTableNameExistsForColumn(ColumnReferenceExpression column, bool multiPartRequisite)
        {
            int count = column.MultiPartIdentifier == null ? 0 : column.MultiPartIdentifier.Count;

            if (!multiPartRequisite)
            {
                // If not multipart return.
                if (((column.ColumnType == ColumnType.Regular) && (count == 1)) ||
                    ((column.ColumnType != ColumnType.Regular) && (count == 0)))
                {
                    return;
                }
            }

            bool tableNameDefined = false;

            if (column.ColumnType == ColumnType.Regular)
            {
                if (count >= 2 &&
                    !String.IsNullOrEmpty(column.MultiPartIdentifier[count - 2].Value))
                {
                    tableNameDefined = true;
                }
            }
            else
            {
                if (count >= 1 &&
                    !String.IsNullOrEmpty(column.MultiPartIdentifier[count - 1].Value))
                {
                    tableNameDefined = true;
                }
            }

            if (!tableNameDefined)
                ThrowParseErrorException("SQL46016", column, TSqlParserResource.SQL46016Message);
        }

        /// <summary>
        /// Checks that the schemaObjectName's DatabaseIdentifier is either null or String.Empty.
        /// </summary>
        /// <param name="name">Name to be checked.</param>
        /// <param name="statementType">Used for error reporting.</param>
        protected static void CheckTwoPartNameForSchemaObjectName(SchemaObjectName name, string statementType)
        {
            if (name.DatabaseIdentifier != null && !String.IsNullOrEmpty(name.DatabaseIdentifier.Value))
            {
                throw new TSqlParseErrorException(CreateParseError("SQL46021",
                    GetFirstToken(name), TSqlParserResource.SQL46021Message, statementType));
            }
        }

        /// <summary>
        /// Checks that the language term is a valid one.
        /// </summary>
        /// <param name="inputString">Literal to be checked.</param>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "inputString")]
        protected static void CheckIfValidLanguageString(Literal inputString)
        {
            //if (!(_languageString.Contains(inputString.Value)))
            //    ThrowParseErrorException("SQL46052", inputString, TSqlParserResource.SQL46052Message, inputString.Value);
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "inputString")]
        protected static void CheckIfValidLanguageIdentifier(Identifier inputString)
        {
            //if (!(_languageIdentifier.Contains(inputString.Value)))
            //    ThrowParseErrorException("SQL46052", inputString, TSqlParserResource.SQL46052Message, inputString.Value);
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "inputValue")]
        protected static void CheckIfValidLanguageInteger(Literal inputValue)
        {
            //int integerValue;
            //if (!(Int32.TryParse(inputValue.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out integerValue)))
            //    ThrowParseErrorException("SQL46053", inputValue, TSqlParserResource.SQL46053Message, inputValue.Value);
            //else if (!(_languageInteger.Contains(integerValue)))
            //    ThrowParseErrorException("SQL46053", inputValue, TSqlParserResource.SQL46053Message, inputValue.Value);
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "inputValue")]
        protected static void CheckIfValidLanguageHex(Literal inputValue)
        {
            //int hexValue;
            //string hexString = inputValue.Value.Remove(0, 2);
            //if (!Int32.TryParse(hexString, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out hexValue))
            //    ThrowParseErrorException("SQL46053", inputValue, TSqlParserResource.SQL46053Message, inputValue.Value);
            //else if (!(_languageInteger.Contains(hexValue)))
            //    ThrowParseErrorException("SQL46053", inputValue, TSqlParserResource.SQL46053Message, inputValue.Value);
        }

        /// <summary>
        /// Small helper to check if token is StopAtMark or StopBeforeMark option beginning
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected static bool IsStopAtBeforeMarkRestoreOption(antlr.IToken token)
        {
            return TryMatch(token, CodeGenerationSupporter.StopAtMark) || TryMatch(token, CodeGenerationSupporter.StopBeforeMark);
        }

        /// <summary>
        /// Creates StopRestoreOption
        /// </summary>
        /// <param name="optionBeginning">token to distinguish between StopAtMark and StopBeforeMark</param>
        /// <param name="mark">Mark</param>
        /// <param name="afterClause">Optional AFTER clause</param>
        /// <returns></returns>
        protected StopRestoreOption CreateStopRestoreOption(antlr.IToken optionBeginning, ValueExpression mark, ValueExpression afterClause)
        {
            StopRestoreOption option = FragmentFactory.CreateFragment<StopRestoreOption>();

            if (TryMatch(optionBeginning, CodeGenerationSupporter.StopAtMark))
            {
                option.IsStopAt = true;
                option.OptionKind = RestoreOptionKind.StopAt;
            }
            else
            {
                option.OptionKind = RestoreOptionKind.Stop;
            }

            option.Mark = mark;
            if (afterClause != null)
                option.After = afterClause;
            return option;
        }

        protected ScalarExpressionRestoreOption CreateSimpleRestoreOptionWithValue(antlr.IToken optionBeginning, ScalarExpression optionValue)
        {
            ScalarExpressionRestoreOption option = FragmentFactory.CreateFragment<ScalarExpressionRestoreOption>();
            option.OptionKind = RestoreOptionWithValueHelper.Instance.ParseOption(optionBeginning);
            option.Value = optionValue;
            return option;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "entryPoint")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "exception")]
        protected void CreateInternalError(string entryPoint, Exception exception)
        {
            string message = TSqlParserResource.SQL46001Message;

#if DEBUG
            message = string.Format(CultureInfo.InvariantCulture, "{0} Entry point: {1} Stack trace: {2}",
                message, entryPoint, exception.ToString());
#endif

            ParseError error = new ParseError(46001, _tokenSource.LastToken.Offset,
                _tokenSource.LastToken.Line, _tokenSource.LastToken.Column, message);
            AddParseError(error);
        }

        // Returns, if there was an error
        protected void SetFunctionBodyStatement(FunctionStatementBody parent, BeginEndBlockStatement compoundStatement)
        {
            if (compoundStatement != null)
            {
                StatementList statementList = FragmentFactory.CreateFragment<StatementList>();
                AddAndUpdateTokenInfo(statementList, statementList.Statements, compoundStatement);
                parent.StatementList = statementList;
            }
        }

        protected static void AddConstraintToColumn(ConstraintDefinition constraint, ColumnDefinition column)
        {
            // Special treatment for Default constraint, 
            // there can be at most one of it in a column definition.
            DefaultConstraintDefinition defaultConstraint = constraint as DefaultConstraintDefinition;
            if (defaultConstraint != null)
            {
                if (column.DefaultConstraint != null)
                    ThrowParseErrorException("SQL46012", constraint, TSqlParserResource.SQL46012Message);

                column.DefaultConstraint = defaultConstraint;
            }
            else
                AddAndUpdateTokenInfo(column, column.Constraints, constraint);
        }

        protected void PutIdentifiersIntoFunctionCall(FunctionCall functionCall, MultiPartIdentifier identifiers)
        {
            int count = identifiers.Count;
            // Identifier list here is from identifierList rule, which should never return empty lists
            Debug.Assert(count > 0);

            functionCall.FunctionName = identifiers[count - 1];
            if (count > 1)
            {
                MultiPartIdentifierCallTarget callTarget = FragmentFactory.CreateFragment<MultiPartIdentifierCallTarget>();
                MultiPartIdentifier multiPartIdentifier = FragmentFactory.CreateFragment<MultiPartIdentifier>();
                for (int i = 0; i < count - 1; ++i)
                {
                    AddAndUpdateTokenInfo(multiPartIdentifier, multiPartIdentifier.Identifiers, identifiers[i]);
                }
                callTarget.MultiPartIdentifier = multiPartIdentifier;
                functionCall.CallTarget = callTarget;
            }
        }

        protected void VerifyColumnDataType(ColumnDefinition column)
        {
            // If the scalarDataType is not parsed, the ColumnIdentifier has to be a timestamp.
            if ((column.DataType == null) && !String.Equals(column.ColumnIdentifier.Value, CodeGenerationSupporter.TimeStamp, StringComparison.OrdinalIgnoreCase))
            {
                throw GetUnexpectedTokenErrorException();
            }
        }

        protected void CreateSetClauseColumn(AssignmentSetClause setClause, MultiPartIdentifier multiPartIdentifier)
        {
            ColumnReferenceExpression column = FragmentFactory.CreateFragment<ColumnReferenceExpression>();
            column.ColumnType = ColumnType.Regular;
            column.MultiPartIdentifier = multiPartIdentifier;
            setClause.Column = column;
        }

        #region Checker Functions

        /// <summary>
        /// Checks National and Varying keywords and updates the type if necessary.
        /// </summary>
        /// <param name="type">The Sql data type.</param>
        /// <param name="nationalToken">The national token.</param>
        /// <param name="isVarying">if set to <c>true</c> [is varying].</param>
        protected static void ProcessNationalAndVarying(SqlDataTypeReference type, IToken nationalToken, bool isVarying)
        {
            if (nationalToken != null && isVarying) // if both national and varying are defined
            {
                if (type.SqlDataTypeOption == SqlDataTypeOption.Char)
                    type.SqlDataTypeOption = SqlDataTypeOption.NVarChar;
                else
                    ThrowParseErrorException("SQL46002", nationalToken,
                        TSqlParserResource.SQL46002Message, GetSqlDataTypeName(type.SqlDataTypeOption));
            }
            else if (nationalToken != null) // if only national is defined
            {
                switch (type.SqlDataTypeOption)
                {
                    case SqlDataTypeOption.Char:
                        type.SqlDataTypeOption = SqlDataTypeOption.NChar;
                        break;
                    case SqlDataTypeOption.Text:
                        type.SqlDataTypeOption = SqlDataTypeOption.NText;
                        break;
                    default:
                        ThrowParseErrorException("SQL46003", nationalToken,
                            TSqlParserResource.SQL46003Message, GetSqlDataTypeName(type.SqlDataTypeOption));
                        break;
                }
            }
            else if (isVarying) // if only varying is defined
            {
                switch (type.SqlDataTypeOption)
                {
                    case SqlDataTypeOption.Binary:
                        type.SqlDataTypeOption = SqlDataTypeOption.VarBinary;
                        break;
                    case SqlDataTypeOption.Char:
                        type.SqlDataTypeOption = SqlDataTypeOption.VarChar;
                        break;
                    case SqlDataTypeOption.NChar:
                        type.SqlDataTypeOption = SqlDataTypeOption.NVarChar;
                        break;
                    default:
                        ThrowParseErrorException("SQL46004", type,
                            TSqlParserResource.SQL46004Message, GetSqlDataTypeName(type.SqlDataTypeOption));
                        break;
                }
            }
        }

        protected static string GetSqlDataTypeName(SqlDataTypeOption type)
        {
            if (type == SqlDataTypeOption.None)
                return TSqlParserResource.UserDefined;
            return type.ToString();
        }

        /// <summary>
        /// Checks if specified data type can have parameter(s)
        /// </summary>
        /// <param name="dataType">Sql data type.</param>
        protected static void CheckSqlDataTypeParameters(SqlDataTypeReference dataType)
        {
            switch (dataType.Parameters.Count)
            {
                case 0:
                    // Nothing to do - can be any type
                    break;
                case 1:
                    if (!_possibleSingleParameterDataTypes.Contains(dataType.SqlDataTypeOption))
                    {
                        ThrowParseErrorException("SQL46008", dataType,
                            TSqlParserResource.SQL46008Message, dataType.SqlDataTypeOption.ToString());
                    }

                    if (dataType.Parameters[0].LiteralType == LiteralType.Max &&
                        // Only these three types cause PARSE errors in SSMS when used with MAX
                        (dataType.SqlDataTypeOption == SqlDataTypeOption.Char ||
                        dataType.SqlDataTypeOption == SqlDataTypeOption.NChar ||
                        dataType.SqlDataTypeOption == SqlDataTypeOption.Binary))
                    {
                        ThrowIncorrectSyntaxErrorException(GetFirstToken(dataType.Parameters[0]));
                    }
                    break;
                case 2:
                    if (dataType.SqlDataTypeOption != SqlDataTypeOption.Decimal &&
                        dataType.SqlDataTypeOption != SqlDataTypeOption.Numeric)
                    {
                        ThrowParseErrorException("SQL46009", dataType,
                            TSqlParserResource.SQL46009Message, dataType.SqlDataTypeOption.ToString());
                    }
                    break;
                default:
                    Debug.Assert(false); // Should not be possible due to grammar
                    break;
            }
        }

        /// <summary>
        /// Context-sensitive check to determine whether the schema object just parsed 
        /// should be instantiated as a SchemaObjectTableReference
        /// </summary>
        protected bool IsTableReference(bool allowMultipleTableHints)
        {
            if (LA(1) != TSql80ParserInternal.LeftParenthesis)
                return true;

            int markSpot = mark();
            try
            {
                consume();
                if ((LA(1) == TSql80ParserInternal.Identifier || LA(1) == TSql80ParserInternal.HoldLock) &&
                    (LA(2) == TSql80ParserInternal.RightParenthesis || allowMultipleTableHints))
                {
                    TableHintKind hintKind;
                    QuoteType quote;
                    string idValue = Identifier.DecodeIdentifier(LT(1).getText(), out quote);
                    if (TableHintOptionsHelper.Instance.TryParseOption(idValue, SqlVersionFlags.TSql80, out hintKind))
                    {
                        return true;
                    }
                }
            }
            finally
            {
                rewind(markSpot);
            }
            return false;
        }

        #endregion

        internal delegate T ParserEntryPoint<T>()
            where T : TSqlFragment;

        internal T ParseRuleWithStandardExceptionHandling<T>(ParserEntryPoint<T> entryPoint, string entryPointName)
            where T : TSqlFragment
        {
            T vResult = null;

            try
            {
                vResult = entryPoint();
            }
            catch (TSqlParseErrorException exception)
            {
                if (!exception.DoNotLog)
                    AddParseError(exception.ParseError);
            }
            catch (antlr.NoViableAltException exception)
            {
                ParseError error = GetFaultTolerantUnexpectedTokenError(
                    exception.token, exception, _tokenSource.LastToken.Offset);
                AddParseError(error);
            }
            catch (antlr.MismatchedTokenException exception)
            {
                ParseError error = GetFaultTolerantUnexpectedTokenError(
                    exception.token, exception, _tokenSource.LastToken.Offset);
                AddParseError(error);
            }
            catch (antlr.RecognitionException)
            {
                ParseError error = GetUnexpectedTokenError(LT(1));
                AddParseError(error);
            }
            catch (antlr.TokenStreamRecognitionException exception)
            {
                ParseError error = ProcessTokenStreamRecognitionException(exception, _tokenSource.LastToken.Offset);
                AddParseError(error);
            }
            catch (antlr.ANTLRException exception)
            {
                CreateInternalError(entryPointName, exception);
            }
            catch (System.StackOverflowException exception)
            {
                CreateInternalError(entryPointName, exception);
            }
            catch (System.NullReferenceException exception)
            {
                CreateInternalError(entryPointName, exception);
            }
            catch (System.ArgumentException exception)
            {
                CreateInternalError(entryPointName, exception);
            }
            catch (System.IndexOutOfRangeException exception)
            {
                CreateInternalError(entryPointName, exception);
            }
            return vResult;
        }

        protected void SetNameForDoublePrecisionType(DataTypeReference dataType, IToken doubleToken, IToken precisionToken)
        {
            Identifier identifier = FragmentFactory.CreateFragment<Identifier>();
            identifier.Value = CodeGenerationSupporter.Float;
            UpdateTokenInfo(identifier, doubleToken);
            UpdateTokenInfo(identifier, precisionToken);

            dataType.Name = FragmentFactory.CreateFragment<SchemaObjectName>();
            AddAndUpdateTokenInfo(dataType.Name, dataType.Name.Identifiers, identifier);

            UpdateTokenInfo(dataType, doubleToken);
            UpdateTokenInfo(dataType, precisionToken);
        }

        protected static void CheckForTemporaryFunction(SchemaObjectName name)
        {
            if (name.BaseIdentifier != null && name.BaseIdentifier.Value != null &&
                name.BaseIdentifier.Value.StartsWith("#", StringComparison.Ordinal))
            {
                ThrowParseErrorException("SQL46093", name, TSqlParserResource.SQL46093Message,
                    name.BaseIdentifier.Value);
            }
        }

        protected static void CheckForTemporaryView(SchemaObjectName name)
        {
            if (name.BaseIdentifier != null && name.BaseIdentifier.Value != null &&
                name.BaseIdentifier.Value.StartsWith("#", StringComparison.Ordinal))
            {
                ThrowParseErrorException("SQL46092", name, TSqlParserResource.SQL46092Message,
                    name.BaseIdentifier.Value);
            }
        }

        #region Error reporting utilities

        #region Generic error reporting stuff

        protected static IToken GetFirstToken(TSqlFragment fragment)
        {
            Debug.Assert(fragment != null);

            if (fragment.ScriptTokenStream != null && fragment.FirstTokenIndex != TSqlFragment.Uninitialized)
                return fragment.ScriptTokenStream[fragment.FirstTokenIndex];
            else
                return null;
        }

        public static void ThrowParseErrorException(string identifier, TSqlFragment fragment, string messageTemplate, params string[] args)
        {
            ThrowParseErrorException(identifier, GetFirstToken(fragment), messageTemplate, args);
        }

        public static void ThrowParseErrorException(string identifier, IToken token, string messageTemplate, params string[] args)
        {
            ParseError error = CreateParseError(identifier, token, messageTemplate, args);
            throw new TSqlParseErrorException(error);
        }

        public static ParseError CreateParseError(string identifier,
            IToken token, string messageTemplate, params string[] args)
        {
            int line, column, offset;
            TSqlWhitespaceTokenFilter.TSqlParserTokenProxyWithIndex proxy =
                token as TSqlWhitespaceTokenFilter.TSqlParserTokenProxyWithIndex;
            if (proxy != null)
            {
                line = proxy.Token.Line;
                column = proxy.Token.Column;
                offset = proxy.Token.Offset;
            }
            else
            {
                TSqlParserToken sqlToken = token as TSqlParserToken;
                if (sqlToken != null)
                {
                    line = sqlToken.Line;
                    column = sqlToken.Column;
                    offset = sqlToken.Offset;
                }
                else
                {
                    Debug.Assert(false);
                    line = 1;
                    column = 1;
                    offset = 0;
                }
            }
            return CreateParseError(identifier, offset, line, column, messageTemplate, args);
        }

        public static ParseError CreateParseError(string identifier, int offset,
            int line, int column, string messageTemplate, params string[] args)
        {
            System.Diagnostics.Debug.Assert(Char.IsLetter(identifier, 0) && Char.IsLetter(identifier, 1) && Char.IsLetter(identifier, 2));
            return new ParseError(Int32.Parse(identifier.Substring(3), CultureInfo.InvariantCulture), offset, line, column,
                string.Format(CultureInfo.CurrentCulture, messageTemplate, args));
        }

        /// <summary>
        /// Gets the appropriate error out of TokenStreamRecognitionException
        /// </summary>
        /// <param name="exception">The exception to be processed.</param>
        /// <param name="lastOffset">The last offset.</param>
        /// <returns>The created error.</returns>
        internal static ParseError ProcessTokenStreamRecognitionException(antlr.TokenStreamRecognitionException exception, int lastOffset)
        {
            antlr.NoViableAltException noViableAltException = exception.recog as antlr.NoViableAltException;
            if (noViableAltException != null)
            {
                return GetFaultTolerantUnexpectedTokenError(noViableAltException.token,
                    noViableAltException, lastOffset);
            }

            antlr.MismatchedTokenException mismatchedTokenException = exception.recog as antlr.MismatchedTokenException;
            if (mismatchedTokenException != null)
            {
                return GetFaultTolerantUnexpectedTokenError(mismatchedTokenException.token,
                    mismatchedTokenException, lastOffset);
            }

            // Right now, we can only get unexpected EOF... but we might need unexpected char error at some point in the future
            antlr.NoViableAltForCharException notViableAltForCharException = exception.recog as antlr.NoViableAltForCharException;
            if (notViableAltForCharException != null)
            {
                return TSql80ParserBaseInternal.CreateParseError("SQL46010",
                    lastOffset,
                    notViableAltForCharException.getLine(),
                    notViableAltForCharException.getColumn(),
                    TSqlParserResource.SQL46010Message,
                    notViableAltForCharException.foundChar.ToString());
            }

            // TODO, sacaglar: This is translated into an internal error.  Is there a runtime log for this?
            return new ParseError(46001,
                lastOffset,
                exception.recog.getLine(),
                exception.recog.getColumn(), TSqlParserResource.SQL46001Message);
        }

        internal static ParseError GetFaultTolerantUnexpectedTokenError(antlr.IToken token,
            RecognitionException exception, int lastOffset)
        {
            // If token is null, we look at the next lookahead.
            if (token == null)
            {
                // TODO, sacaglar: This is translated into an internal error.  Is there a runtime log for this?
                return new ParseError(46001, lastOffset, exception.getLine(), exception.getColumn(), TSqlParserResource.SQL46001Message);
            }
            return GetUnexpectedTokenError(token);
        }

        #endregion

        #region Specializations for common errors
        public static ParseError GetIncorrectSyntaxError(IToken token)
        {
            return CreateParseError("SQL46010", token, TSqlParserResource.SQL46010Message, token.getText());
        }

        public static void ThrowIncorrectSyntaxErrorException(TSqlFragment fragment)
        {
            ThrowIncorrectSyntaxErrorException(GetFirstToken(fragment));
        }

        public static void ThrowIncorrectSyntaxErrorException(IToken token)
        {
            ParseError error = GetIncorrectSyntaxError(token);

            throw new TSqlParseErrorException(error);
        }

        /// <summary>
        /// Sets the next token as the unexpected token and calls the related class.
        /// </summary>
        /// <returns>The created error.</returns>
        protected TSqlParseErrorException GetUnexpectedTokenErrorException()
        {
            return GetUnexpectedTokenErrorException(LT(1));
        }

        protected ParseError GetUnexpectedTokenError()
        {
            return GetUnexpectedTokenError(LT(1));
        }

        internal static ParseError GetUnexpectedTokenError(antlr.IToken token)
        {
            Debug.Assert(token != null);
            ParseError error;
            if (token.Type == TSql80ParserInternal.EOF)
            {
                error = CreateParseError("SQL46029", token, TSqlParserResource.SQL46029Message);
            }
            else
            {
                error = GetIncorrectSyntaxError(token);
            }
            return error;
        }

        internal static TSqlParseErrorException GetUnexpectedTokenErrorException(antlr.IToken token)
        {
            ParseError error = GetUnexpectedTokenError(token);
            return new TSqlParseErrorException(error);
        }

        /// <summary>
        /// Checks the unexpected identifier and creates an error of the appropriate type depending on the type of the token.
        /// </summary>
        /// <param name="identifier">The identifier associated with the error.</param>
        /// <returns>The created error.</returns>
        protected static TSqlParseErrorException GetUnexpectedTokenErrorException(Identifier identifier)
        {
            string text;
            if (identifier.QuoteType != QuoteType.NotQuoted)
            {
                text = Identifier.EncodeIdentifier(identifier.Value);
            }
            else
            {
                text = identifier.Value;
            }
            return new TSqlParseErrorException(CreateParseError("SQL46010",
                GetFirstToken(identifier), TSqlParserResource.SQL46010Message, text));
        }

        #endregion

        #endregion
    }
}