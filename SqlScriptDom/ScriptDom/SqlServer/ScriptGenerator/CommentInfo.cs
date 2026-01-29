//------------------------------------------------------------------------------
// <copyright file="CommentInfo.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    /// <summary>
    /// Intermediate structure for processing comments during script generation.
    /// </summary>
    internal sealed class CommentInfo
    {
        /// <summary>
        /// Gets or sets the original comment token from the token stream.
        /// </summary>
        public TSqlParserToken Token { get; set; }

        /// <summary>
        /// Gets or sets the position of the comment relative to code.
        /// </summary>
        public CommentPosition Position { get; set; }

        /// <summary>
        /// Gets or sets the token index of the associated fragment.
        /// </summary>
        public int AssociatedFragmentIndex { get; set; }

        /// <summary>
        /// Gets whether this is a single-line comment (-- style).
        /// </summary>
        public bool IsSingleLineComment
        {
            get { return Token != null && Token.TokenType == TSqlTokenType.SingleLineComment; }
        }

        /// <summary>
        /// Gets whether this is a multi-line comment (/* */ style).
        /// </summary>
        public bool IsMultiLineComment
        {
            get { return Token != null && Token.TokenType == TSqlTokenType.MultilineComment; }
        }

        /// <summary>
        /// Gets the text content of the comment.
        /// </summary>
        public string Text
        {
            get { return Token?.Text; }
        }

        /// <summary>
        /// Creates a new CommentInfo instance.
        /// </summary>
        /// <param name="token">The comment token.</param>
        /// <param name="position">The position relative to code.</param>
        /// <param name="fragmentIndex">The associated fragment token index.</param>
        public CommentInfo(TSqlParserToken token, CommentPosition position, int fragmentIndex)
        {
            Token = token;
            Position = position;
            AssociatedFragmentIndex = fragmentIndex;
        }
    }
}
