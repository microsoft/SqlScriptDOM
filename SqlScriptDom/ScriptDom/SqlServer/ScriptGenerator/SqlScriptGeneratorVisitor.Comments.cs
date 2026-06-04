//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.Comments.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    internal abstract partial class SqlScriptGeneratorVisitor
    {
        #region Comment Tracking Fields

        /// <summary>
        /// Tracks the last token index processed for comment emission.
        /// Used to find comments between visited fragments.
        /// </summary>
        private int _lastProcessedTokenIndex = -1;

        /// <summary>
        /// The current script's token stream, set when visiting begins.
        /// </summary>
        private IList<TSqlParserToken> _currentTokenStream;

        /// <summary>
        /// Tracks which comment tokens have already been emitted to avoid duplicates.
        /// </summary>
        private readonly HashSet<TSqlParserToken> _emittedComments = new HashSet<TSqlParserToken>();

        /// <summary>
        /// Tracks whether leading (file-level) comments have been emitted.
        /// </summary>
        private bool _leadingCommentsEmitted = false;

        /// <summary>
        /// When true, defers trailing comments for fragments at or past
        /// _suppressTrailingCommentsAfterIndex until after the semicolon.
        /// Set by GenerateStatementWithSemiColon.
        /// </summary>
        private bool _suppressTrailingComments = false;

        /// <summary>Statement boundary used by _suppressTrailingComments.</summary>
        private int _suppressTrailingCommentsAfterIndex = -1;

        /// <summary>
        /// Buffer of '--' trailing comments awaiting the next NewLine. A '--'
        /// comment is only safe at end-of-line.
        /// </summary>
        private readonly List<string> _deferredTrailingSingleLineComments = new List<string>();

        #endregion

        #region Comment Preservation Methods

        /// <summary>
        /// Sets the token stream for comment tracking.
        /// Call this before visiting the root node when PreserveComments is enabled.
        /// </summary>
        /// <param name="tokenStream">The token stream from the parsed script.</param>
        protected void SetTokenStreamForComments(IList<TSqlParserToken> tokenStream)
        {
            _currentTokenStream = tokenStream;
            _lastProcessedTokenIndex = -1;
            _emittedComments.Clear();
            _leadingCommentsEmitted = false;
            _suppressTrailingComments = false;
            _suppressTrailingCommentsAfterIndex = -1;
            _deferredTrailingSingleLineComments.Clear();
        }

        /// <summary>
        /// Emits comments that appear before the first fragment in the script (file-level leading comments).
        /// Called once when generating the first fragment.
        /// </summary>
        /// <param name="fragment">The first fragment being generated.</param>
        protected void EmitLeadingComments(TSqlFragment fragment)
        {
            if (!_options.PreserveComments || _currentTokenStream == null || fragment == null)
            {
                return;
            }

            if (fragment.FirstTokenIndex <= 0)
            {
                return;
            }

            for (int i = 0; i < fragment.FirstTokenIndex && i < _currentTokenStream.Count; i++)
            {
                var token = _currentTokenStream[i];
                if (IsCommentToken(token) && !_emittedComments.Contains(token))
                {
                    EmitCommentToken(token, isLeading: true);
                    _emittedComments.Add(token);
                }
            }
        }

        /// <summary>
        /// Emits comments that appear in the gap between the last emitted token and the current fragment.
        /// This captures comments embedded within sub-expressions.
        /// </summary>
        /// <param name="fragment">The fragment about to be generated.</param>
        protected void EmitGapComments(TSqlFragment fragment)
        {
            if (!_options.PreserveComments || _currentTokenStream == null || fragment == null)
            {
                return;
            }

            int startIndex = _lastProcessedTokenIndex + 1;
            int endIndex = fragment.FirstTokenIndex;

            if (endIndex <= startIndex)
            {
                return;
            }

            for (int i = startIndex; i < endIndex && i < _currentTokenStream.Count; i++)
            {
                var token = _currentTokenStream[i];
                if (IsCommentToken(token) && !_emittedComments.Contains(token))
                {
                    EmitCommentToken(token, isLeading: true);
                    _emittedComments.Add(token);
                    _lastProcessedTokenIndex = i;
                }
            }
        }

        /// <summary>
        /// Emits trailing comments after the fragment, scanning across newlines.
        /// Each comment's own-line vs same-line placement is preserved from source.
        /// </summary>
        protected void EmitTrailingComments(TSqlFragment fragment)
        {
            if (!_options.PreserveComments || _currentTokenStream == null || fragment == null)
            {
                return;
            }

            int lastTokenIndex = fragment.LastTokenIndex;
            if (lastTokenIndex < 0 || lastTokenIndex >= _currentTokenStream.Count)
            {
                return;
            }

            int prevEmittedSourceIndex = lastTokenIndex;
            for (int i = lastTokenIndex + 1; i < _currentTokenStream.Count; i++)
            {
                var token = _currentTokenStream[i];

                if (IsCommentToken(token))
                {
                    if (!_emittedComments.Contains(token))
                    {
                        bool ownLine = SourceGapContainsNewline(prevEmittedSourceIndex, i);
                        EmitTrailingCommentToken(token, ownLine);
                        _emittedComments.Add(token);
                        _lastProcessedTokenIndex = i;
                        prevEmittedSourceIndex = i;
                    }
                    continue;
                }

                if (token.TokenType == TSqlTokenType.WhiteSpace)
                {
                    continue;
                }

                // Any other token (including ';') ends the window.
                break;
            }
        }

        /// <summary>
        /// Trailing-comment scan limited to the fragment's last source line.
        /// Used after statement-ending semicolons so a comment on a later line
        /// remains a leading comment of the next statement.
        /// </summary>
        protected void EmitSameLineTrailingComments(TSqlFragment fragment)
        {
            if (!_options.PreserveComments || _currentTokenStream == null || fragment == null)
            {
                return;
            }

            int lastTokenIndex = fragment.LastTokenIndex;
            if (lastTokenIndex < 0 || lastTokenIndex >= _currentTokenStream.Count)
            {
                return;
            }

            for (int i = lastTokenIndex + 1; i < _currentTokenStream.Count; i++)
            {
                var token = _currentTokenStream[i];

                if (token.TokenType == TSqlTokenType.WhiteSpace)
                {
                    if (ContainsLineBreak(token.Text))
                    {
                        break;
                    }
                    continue;
                }

                if (IsCommentToken(token))
                {
                    if (!_emittedComments.Contains(token))
                    {
                        EmitTrailingCommentToken(token, ownLine: false);
                        _emittedComments.Add(token);
                        _lastProcessedTokenIndex = i;

                        // A '--' comment or a newline-spanning '/* */' ends the line.
                        if (token.TokenType == TSqlTokenType.SingleLineComment ||
                            ContainsLineBreak(token.Text))
                        {
                            break;
                        }
                    }
                    continue;
                }

                break;
            }
        }

        /// <summary>True if any whitespace token between fromIndex and toIndex contains a line break.</summary>
        private bool SourceGapContainsNewline(int fromIndex, int toIndex)
        {
            for (int j = fromIndex + 1; j < toIndex; j++)
            {
                var t = _currentTokenStream[j];
                if (t.TokenType == TSqlTokenType.WhiteSpace && ContainsLineBreak(t.Text))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Emits any unemitted comments whose token index falls within the
        /// statement's source token range (up to and including LastTokenIndex).
        /// Catches floating comments inside a statement whose '/' or ';' has been
        /// absorbed into this statement (e.g. '/* */;' or leading ';WITH').
        /// </summary>
        protected void EmitUnemittedCommentsThroughStatementEnd(TSqlStatement statement)
        {
            if (!_options.PreserveComments || _currentTokenStream == null || statement == null)
            {
                return;
            }

            int endInclusive = statement.LastTokenIndex;
            if (endInclusive < 0 || endInclusive >= _currentTokenStream.Count)
            {
                return;
            }

            for (int i = _lastProcessedTokenIndex + 1; i <= endInclusive; i++)
            {
                var t = _currentTokenStream[i];
                if (IsCommentToken(t) && !_emittedComments.Contains(t))
                {
                    EmitTrailingCommentToken(t, ownLine: true);
                    _emittedComments.Add(t);
                }
            }

            if (endInclusive > _lastProcessedTokenIndex)
            {
                _lastProcessedTokenIndex = endInclusive;
            }
        }

        /// <summary>
        /// Emits unemitted comments in the trivia run starting at
        /// _lastProcessedTokenIndex+1; stops at the first non-whitespace,
        /// non-comment token. For use before a container emits a closing
        /// keyword like END.
        /// </summary>
        protected void EmitCommentsUntilNextNonTriviaToken()
        {
            if (!_options.PreserveComments || _currentTokenStream == null)
            {
                return;
            }

            for (int i = _lastProcessedTokenIndex + 1; i < _currentTokenStream.Count; i++)
            {
                var t = _currentTokenStream[i];

                if (IsCommentToken(t))
                {
                    if (!_emittedComments.Contains(t))
                    {
                        EmitTrailingCommentToken(t, ownLine: true);
                        _emittedComments.Add(t);
                        _lastProcessedTokenIndex = i;
                    }
                    continue;
                }

                if (t.TokenType == TSqlTokenType.WhiteSpace)
                {
                    continue;
                }

                break;
            }
        }

        /// <summary>
        /// Emits a trailing comment. '--' comments are deferred to the next
        /// NewLine; block comments are written inline immediately.
        /// </summary>
        private void EmitTrailingCommentToken(TSqlParserToken token, bool ownLine)
        {
            if (token == null)
            {
                return;
            }

            if (token.TokenType == TSqlTokenType.SingleLineComment)
            {
                _deferredTrailingSingleLineComments.Add(token.Text);
                return;
            }

            if (ownLine)
            {
                _writer.NewLine();
            }
            else
            {
                _writer.AddToken(ScriptGeneratorSupporter.CreateWhitespaceToken(1));
            }

            _writer.AddToken(new TSqlParserToken(token.TokenType, token.Text));
        }

        /// <summary>
        /// Writes deferred '--' trailing comments at end-of-line. Called from
        /// the visitor's NewLine helper before each newline, and at end-of-script.
        /// </summary>
        internal void FlushDeferredTrailingSingleLineComments()
        {
            if (_deferredTrailingSingleLineComments.Count == 0)
            {
                return;
            }

            for (int i = 0; i < _deferredTrailingSingleLineComments.Count; i++)
            {
                _writer.AddToken(ScriptGeneratorSupporter.CreateWhitespaceToken(1));
                _writer.AddToken(new TSqlParserToken(
                    TSqlTokenType.SingleLineComment,
                    _deferredTrailingSingleLineComments[i]));

                // The final '--' is terminated by the caller's pending newline;
                // earlier ones need their own.
                if (i < _deferredTrailingSingleLineComments.Count - 1)
                {
                    _writer.NewLine();
                }
            }

            _deferredTrailingSingleLineComments.Clear();
        }

        /// <summary>
        /// Updates tracking after generating a fragment.
        /// </summary>
        /// <param name="fragment">The fragment that was just generated.</param>
        protected void UpdateLastProcessedIndex(TSqlFragment fragment)
        {
            if (fragment != null && fragment.LastTokenIndex > _lastProcessedTokenIndex)
            {
                _lastProcessedTokenIndex = fragment.LastTokenIndex;
            }
        }

        /// <summary>
        /// Called from GenerateFragmentIfNotNull to handle comments before generating a fragment.
        /// This is the key integration point that enables comments within sub-expressions.
        /// </summary>
        /// <param name="fragment">The fragment about to be generated.</param>
        protected void HandleCommentsBeforeFragment(TSqlFragment fragment)
        {
            if (!_options.PreserveComments || _currentTokenStream == null || fragment == null)
            {
                return;
            }

            // Emit file-level leading comments once
            if (!_leadingCommentsEmitted)
            {
                EmitLeadingComments(fragment);
                _leadingCommentsEmitted = true;
            }

            // Emit any comments in the gap between last processed token and this fragment
            EmitGapComments(fragment);
        }

        /// <summary>
        /// Called from GenerateFragmentIfNotNull to handle comments after generating a fragment.
        /// </summary>
        /// <param name="fragment">The fragment that was just generated.</param>
        protected void HandleCommentsAfterFragment(TSqlFragment fragment)
        {
            if (!_options.PreserveComments || _currentTokenStream == null || fragment == null)
            {
                return;
            }

            // Defer until after the semicolon when at statement boundary.
            if (_suppressTrailingComments && fragment.LastTokenIndex >= _suppressTrailingCommentsAfterIndex)
            {
                UpdateLastProcessedIndex(fragment);
                return;
            }

            EmitTrailingComments(fragment);
            UpdateLastProcessedIndex(fragment);
        }

        /// <summary>
        /// Emits a comment token to the output.
        /// </summary>
        /// <param name="token">The comment token.</param>
        /// <param name="isLeading">True if this is a leading comment, false for trailing.</param>
        private void EmitCommentToken(TSqlParserToken token, bool isLeading)
        {
            if (token == null)
            {
                return;
            }

            if (token.TokenType == TSqlTokenType.SingleLineComment)
            {
                if (!isLeading)
                {
                    // Trailing: add space before comment
                    _writer.AddToken(ScriptGeneratorSupporter.CreateWhitespaceToken(1));
                }

                _writer.AddToken(new TSqlParserToken(TSqlTokenType.SingleLineComment, token.Text));

                if (isLeading)
                {
                    // After a leading comment, add newline
                    _writer.NewLine();
                }
            }
            else if (token.TokenType == TSqlTokenType.MultilineComment)
            {
                if (!isLeading)
                {
                    // Trailing: add space before comment
                    _writer.AddToken(ScriptGeneratorSupporter.CreateWhitespaceToken(1));
                }

                _writer.AddToken(new TSqlParserToken(TSqlTokenType.MultilineComment, token.Text));

                if (isLeading)
                {
                    // After a leading multi-line comment, add newline
                    _writer.NewLine();
                }
            }
        }

        /// <summary>
        /// Emits any remaining comments at the end of the token stream.
        /// Call this after visiting the root fragment to capture comments that appear
        /// after the last statement (end-of-script comments).
        /// </summary>
        protected void EmitRemainingComments()
        {
            // Flush deferred '--' comments at end-of-script.
            FlushDeferredTrailingSingleLineComments();

            if (!_options.PreserveComments || _currentTokenStream == null)
            {
                return;
            }

            // Scan from the last processed token to the end of the token stream
            for (int i = _lastProcessedTokenIndex + 1; i < _currentTokenStream.Count; i++)
            {
                var token = _currentTokenStream[i];
                if (IsCommentToken(token) && !_emittedComments.Contains(token))
                {
                    // End-of-script comments: add newline before, emit comment
                    _writer.NewLine();
                    _writer.AddToken(new TSqlParserToken(token.TokenType, token.Text));
                    _emittedComments.Add(token);
                }
            }
        }

        /// <summary>
        /// Checks if a token is a comment token.
        /// </summary>
        private static bool IsCommentToken(TSqlParserToken token)
        {
            return token != null &&
                   (token.TokenType == TSqlTokenType.SingleLineComment ||
                    token.TokenType == TSqlTokenType.MultilineComment);
        }

        /// <summary>True if the text contains '\n' or '\r'.</summary>
        private static bool ContainsLineBreak(string text)
        {
            return text != null && (text.IndexOf('\n') >= 0 || text.IndexOf('\r') >= 0);
        }

        #endregion
    }
}
