//------------------------------------------------------------------------------
// <copyright file="TSqlParserToken.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using antlr;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Represents a single Token in the input text.
    /// </summary>
    public sealed class TSqlParserToken : IToken
    {
        // most tokens will want line and text information
        private string _text;
        private int _offset;
        private int _line;
        private int _column;
        private TSqlTokenType _tokenType;
        private bool _convertStringToIdentifier = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TSqlParserToken"/> class.
        /// </summary>
        public TSqlParserToken()
            : this(Token.INVALID_TYPE, 0, null, 1, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TSqlParserToken"/> class.
        /// </summary>
        /// <param name="type">The Token type.</param>
        /// <param name="text">The text.</param>
        public TSqlParserToken(TSqlTokenType type, string text)
            : this(type, 0, text, 1, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TSqlParserToken"/> class.
        /// </summary>
        /// <param name="type">The Token type.</param>
        /// <param name="offset">The offset position in the input.</param>
        /// <param name="text">The text.</param>
        /// <param name="line">The line number in the input.</param>
        /// <param name="column">The column number in the input.</param>
        public TSqlParserToken(TSqlTokenType type, int offset, string text, int line, int column)
        {
            _text = text;
            _tokenType = type;
            _offset = offset;
            _line = line;
            _column = column;
        }

#if false
        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "[\"{0}\",<{1}>,offset={2},line={3},column={4}]",
                Text, _tokenType, _offset, _line, _column);
        }
#endif

        #region Public interface

        /// <summary>
        /// Gets or sets the type of the token.
        /// </summary>
        /// <value>The type of the token.</value>
        public TSqlTokenType TokenType
        {
            get { return _tokenType; }
            set { _tokenType = value; }
        }

        /// <summary>
        /// Required to handle quoted identifiers 'feature' in SQL :-(
        /// </summary>
        internal bool ConvertStringToIdentifier
        {
            get { return _convertStringToIdentifier; }
            set { _convertStringToIdentifier = value; }
        }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        /// <value>The offset.</value>
        public int Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        /// <summary>
        /// Gets or sets the line.
        /// </summary>
        /// <value>The line.</value>
        public int Line
        {
            get { return _line; }
            set { _line = value; }
        }

        /// <summary>
        /// Gets or sets the column.
        /// </summary>
        /// <value>The column.</value>
        public int Column
        {
            get { return _column; }
            set { _column = value; }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// Determines if the token is a language keyword.
        /// </summary>
        /// <returns>True if the token is a language keyword, else false.</returns>
        public bool IsKeyword()
        {
            // Due to the way tokens are defined in TSqlTokenTypes and in the Lexer separately, all keywords come first except for None and EndOfFile.
            //
            return TokenType > TSqlTokenType.EndOfFile && TokenType < TSqlTokenType.Bang;
        }

        #endregion

        #region IToken Members
        int IToken.Type
        {
            get
            {
                if (_tokenType == TSqlTokenType.AsciiStringOrQuotedIdentifier)
                {
                    if (_convertStringToIdentifier)
                        return TSql80ParserInternal.QuotedIdentifier;
                    else
                        return TSql80ParserInternal.AsciiStringLiteral;
                }
                else
                    return (int)_tokenType;
            }
            set
            {
                // COMMENT, olegr: 
                // ANTLR lexer, for some mysterious reason, sets token type, then gets it to variable,
                // and then sets it on token again. This breaks our conversion logic, so, here is a work-around...
                if (_tokenType != TSqlTokenType.AsciiStringOrQuotedIdentifier)
                    _tokenType = (TSqlTokenType)value;
            }
        }

        int IToken.getColumn()
        {
            return _column;
        }

        void IToken.setColumn(int c)
        {
            _column = c;
        }

        int IToken.getLine()
        {
            return _line;
        }

        void IToken.setLine(int l)
        {
            _line = l;
        }

        string IToken.getFilename()
        {
            return null;
        }

        void IToken.setFilename(string name)
        {
        }

        string IToken.getText()
        {
            return Text;
        }

        void IToken.setText(string t)
        {
            Text = t;
        }

        #endregion
    }
}
