//------------------------------------------------------------------------------
// <copyright file="ClauseBodyAlignment.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Represents how the body of a clause (the part after FROM, WHERE, GROUP BY, etc.) is laid out
    /// relative to its keyword.
    /// </summary>
    public enum ClauseBodyAlignment
    {
        /// <summary>
        /// Keep the body on the keyword's line and line all clause bodies up under a shared column
        /// past the widest keyword (the classic "rivers of whitespace" style).
        /// </summary>
        Aligned,

        /// <summary>
        /// Put the body on its own new line, indented one level (<see cref="IndentationMode"/> /
        /// IndentationSize) past the keyword, so nesting grows one step per level instead of drifting
        /// right as keywords get wider. When this value is used, the AlignClauseBodies setting is
        /// ignored.
        /// </summary>
        Indented
    }
}
