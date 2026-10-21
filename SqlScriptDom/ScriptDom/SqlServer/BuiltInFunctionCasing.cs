//------------------------------------------------------------------------------
// <copyright file="BuiltInFunctionCasing.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Represents the possible ways of casing built-in / system function names during script
    /// generation. Any value other than Preserve takes precedence over KeywordCasing for function
    /// names; data type names, keywords, string literals, and variables are never affected. Casing
    /// is applied to every unqualified, non-delimited function name, which is safe because T-SQL
    /// resolves a one-part scalar function name as a built-in function and never as a user-defined
    /// one (a scalar user-defined function must be called with at least a two-part name).
    /// Schema-qualified and delimited function names, and method calls on a variable or column, are
    /// always preserved.
    /// </summary>
    public enum BuiltInFunctionCasing
    {
        /// <summary>
        /// Preserve the built-in function name exactly as it would be produced without this option
        /// (backward-compatible: has no effect on the formatted output).
        /// </summary>
        Preserve,

        /// <summary>
        /// All letters in upper case
        /// </summary>
        Uppercase,

        /// <summary>
        /// All letters in lower case
        /// </summary>
        Lowercase,

        /// <summary>
        /// First letter capitalized, remaining letters lower case
        /// </summary>
        PascalCase
    }
}
