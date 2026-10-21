//------------------------------------------------------------------------------
// <copyright file="InternalLexer.support.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.IO;
using antlr;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal abstract class TSqlLexerBaseInternal : antlr.CharScanner
    {
        protected TSqlLexerBaseInternal(LexerSharedInputState state)
            : base(state)
        {
        }

        public void InitializeForNewInput(int startOffset, int startLine, int startColumn, TextReader input)
        {
            setTabSize(1);

            resetState(input);

            setColumn(startColumn);
            setLine(startLine);

            _currentLineStartOffset = startOffset - (startColumn-1); // Columns are 1-based
            _acceptableGoOffset = startOffset;
        }

        private int _complexTokenStartOffset = 0;
        protected int _currentLineStartOffset = 0;

        /// <summary>
        /// The kind of complex token currently being scanned. Recorded while scanning so
        /// that, if end-of-input is reached before the token is closed, the correct
        /// unterminated-token error can be raised (see <see cref="handlePrematureEof"/>).
        /// </summary>
        private TokenKind _complexTokenKind = TokenKind.Common;

        /// <summary>
        /// Used to be able to handle GO statement.
        /// </summary>
        protected int _acceptableGoOffset = 0;
        
        public int CurrentOffset
        {
            get 
            { 
                return _currentLineStartOffset + getColumn() - 1;  // Columns are 1-based
            }
        }

        public override void newline()
        {
            _currentLineStartOffset += getColumn() - 1; // Columns are 1-based
            base.newline();
        }

        protected internal override IToken makeToken(int t)
        {
            // A token was produced successfully, so we are no longer inside a complex token.
            _complexTokenKind = TokenKind.Common;
            return new TSqlParserToken(
                (TSqlTokenType)t,
                CurrentOffset - text.Length,
                null,
                inputState.tokenStartLine,
                inputState.tokenStartColumn);
        }

        /// <summary>
        /// Token kinds - for error reporting when we hit EOF in the middle of something
        /// </summary>
        protected enum TokenKind
        {
            Common = 0,
            String = 1,
            SqlCommandIdentifier = 2,
            QuotedIdentifier = 3,
            MultiLineComment = 4
        }

        /// <summary>
        /// Checks if the EOF is the next char, and throws an exception if that is the case.
        /// </summary>
        protected void checkEOF(TokenKind currentToken)
        {
            // Remember which complex token we are scanning so handlePrematureEof can raise
            // the right error if the closing delimiter is never found.
            _complexTokenKind = currentToken;
            if (LA(1) == EOF_CHAR)
            {
                uponEOF();
                ParseError error = null;
                switch (currentToken)
                {
                    case TokenKind.MultiLineComment:
                        error = TSql80ParserBaseInternal.CreateParseError("SQL46032", 
                            CurrentOffset, getLine(), getColumn(), 
                            TSqlParserResource.SQL46032Message);
                        break;
                    case TokenKind.QuotedIdentifier:
                        error = TSql80ParserBaseInternal.CreateParseError("SQL46031",
                            _complexTokenStartOffset, inputState.tokenStartLine, inputState.tokenStartColumn,
                            TSqlParserResource.SQL46031Message, getText());
                        break;
                    case TokenKind.SqlCommandIdentifier:
                        error = TSql80ParserBaseInternal.CreateParseError("SQL46033",
                            _complexTokenStartOffset, inputState.tokenStartLine, inputState.tokenStartColumn,
                            TSqlParserResource.SQL46033Message, getText());
                        break;
                    case TokenKind.String:
                        error = TSql80ParserBaseInternal.CreateParseError("SQL46030",
                            _complexTokenStartOffset, inputState.tokenStartLine, inputState.tokenStartColumn,
                            TSqlParserResource.SQL46030Message, getText());
                        break;
                    case TokenKind.Common:
                    default:
                        break;
                }
                if (error != null)
                    throw new TSqlParseErrorException(error);
            }
        }

        /// <summary>
        /// Remembers start of complex token - to be reported in case of premature EOF
        /// </summary>
        protected void beginComplexToken()
        {
            _complexTokenStartOffset = CurrentOffset;
            // Record the kind of complex token from its opening delimiter so the correct
            // unterminated-token error can still be raised when the token has no content
            // before end-of-input (e.g. a lone quote at the end of the script). The
            // content-loop checkEOF call is not reached at genuine EOF because the EOF
            // sentinel is no longer a member of the content token sets.
            switch (cached_LA1)
            {
                case '\'':      // ascii string literal opens with '
                case 'n':       // unicode string literal opens with n' (lexer is case-insensitive)
                    _complexTokenKind = TokenKind.String;
                    break;
                case '[':
                case '"':       // delimited identifier opens with [ or "
                    _complexTokenKind = TokenKind.QuotedIdentifier;
                    break;
                case '/':       // multi-line comment opens with /*
                    _complexTokenKind = TokenKind.MultiLineComment;
                    break;
                case '$':       // sql command identifier opens with $(
                    _complexTokenKind = TokenKind.SqlCommandIdentifier;
                    break;
                default:
                    _complexTokenKind = TokenKind.Common;
                    break;
            }
        }

        /// <summary>
        /// Called by the lexer base when a character match fails at end-of-input. If we
        /// are scanning a complex token (string literal, delimited identifier, or
        /// multi-line comment), raise the specific unterminated-token error rather than a
        /// generic mismatch. The EOF sentinel is no longer a member of the content token
        /// sets, so the in-loop checkEOF call is not reached at genuine end-of-input; this
        /// hook fires when the closing delimiter match fails instead.
        /// </summary>
        protected override void handlePrematureEof()
        {
            if (inputState.guessing == 0)
            {
                checkEOF(_complexTokenKind);
            }
        }

        internal static bool IsValueTooLargeForTokenInteger(string source)
        {
            const int MaxIntegerLength = 10;
            //RadParser defined this as 10, after MAXDIGIT_INT4, however in Parslex.cpp it is actually MAXDIGIT_INT4+1.
            //
            const int MaxIntegerLengthWithLeadingZero = MaxIntegerLength + 1;

            int tokenLength = source.Length;
            //consistent with the engine lexer, 
            //if an integer token has more than 11 digits then 
            //  it is TOKEN_NUMERIC, even if it would have actually fit in an int, 
            //i.e. 12 leading 0's and a 5 (which is just 5) is still TOKEN_NUMERIC
            //
            if (tokenLength > MaxIntegerLengthWithLeadingZero)
            {
                return true;
            }
            else if (tokenLength >= MaxIntegerLength)
            {
                Int64 sourceLong = Int64.Parse(source, System.Globalization.CultureInfo.InvariantCulture.NumberFormat );
                return sourceLong > Int32.MaxValue;
            }
            return false;
        }
    }
}
