//------------------------------------------------------------------------------
// <copyright file="IdentifierCasing.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Represents the possible ways of casing object identifiers during script generation.
    /// This does not apply to keywords, string literals, function names, or variables.
    /// </summary>
    public enum IdentifierCasing
    {
        /// <summary>
        /// Preserve the original casing of identifiers
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
        /// First letter of the identifier capitalized, all remaining letters lower case
        /// </summary>
        PascalCase
    }
}
