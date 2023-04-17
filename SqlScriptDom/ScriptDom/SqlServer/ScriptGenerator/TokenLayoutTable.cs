//------------------------------------------------------------------------------
// <copyright file="TokenLayoutTable.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using antlr;
using Microsoft.VisualStudio.TeamSystem.Data.Parser.TSql;

namespace Microsoft.Data.Schema.ScriptDom.Sql.ScriptGenerator
{
    /// <summary>
    /// Represents a table of tokens used for layout purposes
    /// </summary>
    internal class TokenLayoutTable : IEnumerable<TSqlParserToken>
    {
        #region internal class TokenLayoutColumn
        /// <summary>
        /// Represents a column in a token layout table.
        /// </summary>
        internal class TokenLayoutColumn
        {
            private List<TSqlParserToken> _tokens = new List<TSqlParserToken>();

            /// <summary>
            /// Gets a list of tokens in this column
            /// </summary>
            public List<TSqlParserToken> Tokens
            {
                get { return _tokens; }
            }

            /// <summary>
            /// Gets the length of this column in characters
            /// </summary>
            /// <returns>The sum of the lengths of all tokens in this column</returns>
            public int GetLength()
            {
                int len = 0;
                foreach (IToken tok in Tokens)
                {
                    if (tok.getText() != null)
                    {
                        len += tok.getText().Length;
                    }
                }
                return len;
            }
        }
        #endregion

        #region internal class TokenLayoutLine
        /// <summary>
        /// Represents a line in a token layout table
        /// </summary>
        internal class TokenLayoutLine
        {
            #region Private Fields
            private List<TokenLayoutColumn> _columns = new List<TokenLayoutColumn>();
            private TokenLayoutColumn _currentColumn = null;
            private Boolean _useColumnAlignment = true;
            private Boolean _hasWhiteSpaceOnly; // do we have white space only for this line?
            #endregion

            public TokenLayoutLine() : this(true) { }

            public TokenLayoutLine(bool useColumnAlignment)
            {
                _currentColumn = new TokenLayoutColumn();
                _columns.Add(_currentColumn);
                this._useColumnAlignment = useColumnAlignment;
                _hasWhiteSpaceOnly = true;
            }

            public bool UseColumnAlignment
            {
                get { return _useColumnAlignment; }
                set { _useColumnAlignment = value; }
            }

            /// <summary>
            /// Gets a list of the columns in this line
            /// </summary>
            public List<TokenLayoutColumn> Columns
            {
                get { return _columns; }
            }

            /// <summary>
            /// Gets the column on which the writer is positioned in this line
            /// </summary>
            public TokenLayoutColumn CurrentColumn
            {
                get { return _currentColumn; }
            }

            /// <summary>
            /// Adds a token to the current column in this line
            /// </summary>
            /// <param name="tok">The token to add</param>
            public void AddToken(TSqlParserToken tok)
            {
                // Verify that there is a current column
                // If this fails, it is an error with our code so we want an Assertion, not an Exception
                Debug.Assert(_currentColumn != null, "TokenLayoutLine.AddToken: Line cannot be added to after calling EndLine");

                _hasWhiteSpaceOnly = _hasWhiteSpaceOnly && tok.TokenType == TSqlTokenTypes.WS;

                // Add the token to the current column
                _currentColumn.Tokens.Add(tok);
            }

            /// <summary>
            /// Positions the writer on the next column in this line
            /// </summary>
            public void NextColumn()
            {
                // Create a new column, set it as current and add it to the line
                _currentColumn = new TokenLayoutColumn();
                _columns.Add(_currentColumn);
            }

            /// <summary>
            /// Called to end the line, disallows furthur addition of tokens to this line
            /// </summary>
            public void EndLine()
            {
                _currentColumn = null;
            }

            /// <summary>
            /// The line has white space only
            /// </summary>
            public Boolean HasWhiteSpaceOnly
            {
                get { return _hasWhiteSpaceOnly; }
            }
        }
        #endregion

        #region Private Fields
        private List<TokenLayoutLine> _lines = new List<TokenLayoutLine>();
        private TokenLayoutLine _currentLine = null;
        #endregion

        #region Public Constructors

        public TokenLayoutTable()
        {
            _currentLine = new TokenLayoutLine();
            _lines.Add(_currentLine);
        } 

        #endregion

        #region Public Properties

        public List<TokenLayoutLine> Lines
        {
            get { return _lines; }
        }

        public TokenLayoutLine CurrentLine
        {
            get { return _currentLine; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a token to the current line/column cell in the layout table
        /// </summary>
        /// <param name="tok">The token to add</param>
        public void AddToken(TSqlParserToken tok)
        {
            // Verify that there is a current column
            // If this fails, it is an error with our code so we want an Assertion, not an Exception
            Debug.Assert(_currentLine != null, "TokenLayoutTable.AddToken: There is no current line");

            // Add the token to that line
            _currentLine.AddToken(tok);
        }

        /// <summary>
        /// Advances to the next column in the current line of the layout table
        /// </summary>
        public void NextColumn()
        {
            // Verify that there is a current line
            Debug.Assert(_currentLine != null, "TokenLayoutTable.NextColumn: No current layout line");

            // Advance to the next column in that line
            _currentLine.NextColumn();
        }

        /// <summary>
        /// Advances to the next line in the layout table
        /// </summary>
        /// <param name="remainInCurrentColumn">
        /// A boolean indicating if the current column should remain the same in the next line as it
        /// was in the previous line.
        /// </param>
        /// <param name="useColumnAlignment">
        /// A boolean indicating if the next line should use column alignment or if its contents
        /// should be treated as already layed out.
        /// </param>
        public void NextLine(bool remainInCurrentColumn, bool useColumnAlignment)
        {
            // If we are to remain in the current column, find out which column that is
            int currentColumn = _currentLine.Columns.Count - 1; // The last column is the current one

            // Create the new line, set it as current and add it to the lines list
            _currentLine = new TokenLayoutLine(useColumnAlignment);
            _lines.Add(_currentLine);

            // If we are to remain in the previously current column, move to that column
            if (remainInCurrentColumn)
            {
                for (int i = 0; i < currentColumn; i++)
                {
                    _currentLine.NextColumn();
                }
            }
        }

        /// <summary>
        /// Writes the contents of this layout table to the specified script writer
        /// </summary>
        /// <param name="writer">The script writer to write contents to</param>
        public void WriteTo(ScriptWriter writer)
        {
            for(int i = 0; i < _lines.Count; i++)
            {
                foreach (TokenLayoutColumn col in _lines[i].Columns)
                {
                    foreach (TSqlParserToken tok in col.Tokens)
                    {
                        writer.WriteToken(tok);
                    }
                }

                // Don't write a newline if this is the last one
                if(i < _lines.Count - 1)
                    writer.WriteNewline(true);
            }
        }

        #endregion

        #region IEnumerable<TSqlParserToken> Members

        public IEnumerator<TSqlParserToken> GetEnumerator()
        {
            Boolean firstLine = true;
            foreach (TokenLayoutLine line in this.Lines)
            {
                if (firstLine)
                {
                    firstLine = false;
                }
                else
                {
                    yield return new TSqlParserToken(TSqlTokenTypes.WS, -1, Environment.NewLine);
                }

                foreach (TokenLayoutColumn col in line.Columns)
                {
                    foreach (TSqlParserToken tok in col.Tokens)
                    {
                        yield return tok;
                    }
                }
            }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<TSqlParserToken>)this).GetEnumerator();
        }

        #endregion
    }
}
