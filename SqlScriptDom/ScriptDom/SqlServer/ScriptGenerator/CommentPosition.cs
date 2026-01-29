//------------------------------------------------------------------------------
// <copyright file="CommentPosition.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    /// <summary>
    /// Categorizes comment placement relative to code during script generation.
    /// </summary>
    internal enum CommentPosition
    {
        /// <summary>
        /// Comment appears before the associated code on previous line(s).
        /// </summary>
        Leading,

        /// <summary>
        /// Comment appears after code on the same line.
        /// </summary>
        Trailing,

        /// <summary>
        /// Comment not directly associated with code (e.g., end of file, standalone block).
        /// </summary>
        Standalone
    }
}
