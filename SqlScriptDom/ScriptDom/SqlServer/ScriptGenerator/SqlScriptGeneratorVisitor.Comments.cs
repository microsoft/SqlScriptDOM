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
        }

        /// <summary>
        /// Gets leading comments that appear between the last processed token and the current fragment.
        /// </summary>
        /// <param name="fragment">The current fragment being visited.</param>
        /// <returns>List of comment information for leading comments.</returns>
        protected List<CommentInfo> GetLeadingComments(TSqlFragment fragment)
        {
            var comments = new List<CommentInfo>();

            if (_currentTokenStream == null || fragment == null || !_options.PreserveComments)
            {
                return comments;
            }

            int startIndex = _lastProcessedTokenIndex + 1;
            int endIndex = fragment.FirstTokenIndex;

            // Scan for comments between last processed and current fragment
            for (int i = startIndex; i < endIndex && i < _currentTokenStream.Count; i++)
            {
                var token = _currentTokenStream[i];
                if (IsCommentToken(token))
                {
                    comments.Add(new CommentInfo(token, CommentPosition.Leading, fragment.FirstTokenIndex));
                }
            }

            return comments;
        }

        /// <summary>
        /// Gets trailing comments that appear on the same line after the fragment.
        /// </summary>
        /// <param name="fragment">The current fragment being visited.</param>
        /// <returns>List of comment information for trailing comments.</returns>
        protected List<CommentInfo> GetTrailingComments(TSqlFragment fragment)
        {
            var comments = new List<CommentInfo>();

            if (_currentTokenStream == null || fragment == null || !_options.PreserveComments)
            {
                return comments;
            }

            int lastTokenIndex = fragment.LastTokenIndex;
            if (lastTokenIndex < 0 || lastTokenIndex >= _currentTokenStream.Count)
            {
                return comments;
            }

            var lastToken = _currentTokenStream[lastTokenIndex];
            int lastTokenLine = lastToken.Line;

            // Scan for comments after the last token on the same line
            for (int i = lastTokenIndex + 1; i < _currentTokenStream.Count; i++)
            {
                var token = _currentTokenStream[i];
                
                // Stop if we've gone past the same line (unless it's whitespace with no newline)
                if (token.Line > lastTokenLine)
                {
                    break;
                }

                // Found a comment on the same line
                if (IsCommentToken(token))
                {
                    comments.Add(new CommentInfo(token, CommentPosition.Trailing, lastTokenIndex));
                    
                    // For single-line comments, there can't be anything else after on this line
                    if (token.TokenType == TSqlTokenType.SingleLineComment)
                    {
                        break;
                    }
                }
            }

            return comments;
        }

        /// <summary>
        /// Emits a comment using the script writer with current indentation.
        /// </summary>
        /// <param name="commentInfo">The comment to emit.</param>
        protected void EmitComment(CommentInfo commentInfo)
        {
            if (commentInfo == null || commentInfo.Token == null)
            {
                return;
            }

            var text = commentInfo.Text;
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            if (commentInfo.IsSingleLineComment)
            {
                EmitSingleLineComment(text, commentInfo.Position);
            }
            else if (commentInfo.IsMultiLineComment)
            {
                EmitMultiLineComment(text, commentInfo.Position);
            }
        }

        /// <summary>
        /// Emits a single-line comment.
        /// </summary>
        private void EmitSingleLineComment(string text, CommentPosition position)
        {
            if (position == CommentPosition.Trailing)
            {
                // Trailing: add space before comment, keep on same line
                _writer.AddToken(ScriptGeneratorSupporter.CreateWhitespaceToken(1));
            }
            else
            {
                // Leading: comment goes on its own line
                // Indentation is already applied by current context
            }

            // Write the comment as-is (preserving the -- prefix)
            _writer.AddToken(new TSqlParserToken(TSqlTokenType.SingleLineComment, text));

            if (position == CommentPosition.Leading)
            {
                // After a leading comment, we need a newline
                _writer.NewLine();
            }
        }

        /// <summary>
        /// Emits a multi-line comment, preserving internal structure.
        /// </summary>
        private void EmitMultiLineComment(string text, CommentPosition position)
        {
            if (position == CommentPosition.Trailing)
            {
                // Trailing: add space before comment
                _writer.AddToken(ScriptGeneratorSupporter.CreateWhitespaceToken(1));
            }

            // For multi-line comments, we preserve the content as-is
            // The comment includes /* and */ delimiters
            _writer.AddToken(new TSqlParserToken(TSqlTokenType.MultilineComment, text));

            if (position == CommentPosition.Leading)
            {
                // After a leading multi-line comment, add newline
                _writer.NewLine();
            }
        }

        /// <summary>
        /// Called before visiting a fragment to emit any leading comments.
        /// </summary>
        /// <param name="fragment">The fragment about to be visited.</param>
        protected void BeforeVisitFragment(TSqlFragment fragment)
        {
            if (!_options.PreserveComments || _currentTokenStream == null || fragment == null)
            {
                return;
            }

            var leadingComments = GetLeadingComments(fragment);
            foreach (var comment in leadingComments)
            {
                EmitComment(comment);
            }
        }

        /// <summary>
        /// Called after visiting a fragment to emit any trailing comments and update tracking.
        /// </summary>
        /// <param name="fragment">The fragment that was just visited.</param>
        protected void AfterVisitFragment(TSqlFragment fragment)
        {
            if (!_options.PreserveComments || _currentTokenStream == null || fragment == null)
            {
                return;
            }

            var trailingComments = GetTrailingComments(fragment);
            foreach (var comment in trailingComments)
            {
                EmitComment(comment);
            }

            // Update the last processed token index
            if (fragment.LastTokenIndex >= 0)
            {
                // Account for any trailing comments we just emitted
                int newLastIndex = fragment.LastTokenIndex;
                foreach (var comment in trailingComments)
                {
                    // Find the index of this comment token
                    for (int i = newLastIndex + 1; i < _currentTokenStream.Count; i++)
                    {
                        if (_currentTokenStream[i] == comment.Token)
                        {
                            newLastIndex = i;
                            break;
                        }
                    }
                }
                _lastProcessedTokenIndex = newLastIndex;
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

        #endregion
    }
}
