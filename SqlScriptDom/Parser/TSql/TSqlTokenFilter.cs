//------------------------------------------------------------------------------
// <copyright file="TSqlTokenFilter.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using antlr;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class TSqlWhitespaceTokenFilter : TokenStream
    {
        public TSqlWhitespaceTokenFilter(bool quotedIdentifier, IList<TSqlParserToken> streamToFilter)
        {
            _quotedIdentifier = quotedIdentifier;
            _streamToFilter = streamToFilter;
            _currentTokenIndex = 0;

            // Non-empty stream?
            if (streamToFilter.Count > 0)
                _lastToken = streamToFilter[0];
            else
                _lastToken = new TSqlParserToken(TSqlTokenType.EndOfFile, null);

            // Assuming, that token stream is not changed during parsing!
            _streamLength = streamToFilter.Count;
        }

        public IToken nextToken()
        {
            TSqlParserToken token = null;
            int tokenIndex = TSqlFragment.Uninitialized;
            while (_currentTokenIndex < _streamLength)
            {
                token = _streamToFilter[_currentTokenIndex];
                tokenIndex = _currentTokenIndex;
                ++_currentTokenIndex;

                if (token.TokenType != TSqlTokenType.SingleLineComment &&
                    token.TokenType != TSqlTokenType.MultilineComment &&
                    token.TokenType != TSqlTokenType.WhiteSpace)
                    break;
            }

            // No more tokens - but parser asks for more.. Most likely, this due to malformed token stream
            // (Sidenote: ANTLR can't produce correct tokens from completely empty input, so, this is kinda ok...)
            if (token == null) 
            {
                if (_streamLength != 0)
                {
                    TSqlParserToken lastToken = _streamToFilter[_streamToFilter.Count - 1];
                    int lastTokenLength = (lastToken.Text == null ? 0 : lastToken.Text.Length);

                    token = new TSqlParserToken(TSqlTokenType.EndOfFile, 
                        lastToken.Offset + lastTokenLength, 
                        null,
                        lastToken.Line, lastToken.Column + lastTokenLength);
                }
                else
                    token = new TSqlParserToken(TSqlTokenType.EndOfFile, null);
            }
            else if (token.TokenType == TSqlTokenType.AsciiStringOrQuotedIdentifier)
            {
                token.ConvertStringToIdentifier = _quotedIdentifier;
            }

            _lastToken = token;
            return new TSqlParserTokenProxyWithIndex(token, tokenIndex);
        }

        public TSqlParserToken LastToken
        {
            get { return _lastToken; }
        }

        public bool QuotedIdentifier
        {
            get { return _quotedIdentifier; }
            set { _quotedIdentifier = value; }
        }

        private bool _quotedIdentifier;
        private IList<TSqlParserToken> _streamToFilter;
        private int _currentTokenIndex;
        private int _streamLength;

        // For error reporting in case of totally unexpected internal error
        private TSqlParserToken _lastToken = null;

        #region Class used to communicate token index to parser (for AST creation)
        internal class TSqlParserTokenProxyWithIndex : antlr.IToken
        {
            public TSqlParserTokenProxyWithIndex(TSqlParserToken token, int index)
            {
                _token = token;
                _index = index;
            }

            IToken _token;
            int _index;

            public TSqlParserToken Token
            {
                get { return (TSqlParserToken)_token; }
            }

            public int TokenIndex
            {
                get { return _index; }
            }        

            #region IToken Members

            public int getColumn()
            {
                return _token.getColumn();
            }

            public void setColumn(int c)
            {
                _token.setColumn(c);
            }

            public int getLine()
            {
                return _token.getLine();
            }

            public void setLine(int l)
            {
                _token.setLine(l);
            }

            public string getFilename()
            {
                return _token.getFilename();
            }

            public void setFilename(string name)
            {
                _token.setFilename(name);
            }

            public string getText()
            {
                return _token.getText();
            }

            public void setText(string t)
            {
                _token.setText(t);
            }

            public int Type
            {
                get
                {
                    return _token.Type;
                }
                set
                {
                    _token.Type = value;
                }
            }

            #endregion
        }

        #endregion
    }
}
