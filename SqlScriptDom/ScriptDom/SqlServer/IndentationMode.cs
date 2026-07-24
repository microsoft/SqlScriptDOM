//------------------------------------------------------------------------------
// <copyright file="IndentationMode.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Represents the character used to indent generated script.
    /// </summary>
    public enum IndentationMode
    {
        /// <summary>
        /// Each indentation level inserts IndentationSize space characters.
        /// </summary>
        Spaces,

        /// <summary>
        /// Leading whitespace and the aligned gaps between tokens (for example a clause keyword and its
        /// body, or column-definition fields) are written using only tab characters, rounding each
        /// aligned column up to the next tab stop. Because those tab stops assume each tab is
        /// <see cref="SqlScriptGeneratorOptions.IndentationSize"/> columns wide, the output only appears
        /// aligned when the viewer's tab width (for example the editor's tab size) is set to the same
        /// value as <see cref="SqlScriptGeneratorOptions.IndentationSize"/>. This mode is ignored
        /// (indentation uses spaces) when <see cref="SqlScriptGeneratorOptions.CommaPlacement"/> is
        /// <see cref="CommaPlacement.Leading"/>.
        /// </summary>
        Tabs
    }
}
