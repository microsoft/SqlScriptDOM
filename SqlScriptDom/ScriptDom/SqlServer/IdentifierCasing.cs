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
    /// Represents the possible ways of casing SQL identifiers
    /// </summary>
    public enum IdentifierCasing
    {
        /// <summary>
        /// Preserve original casing
        /// </summary>
        PreserveOriginal,

        /// <summary>
        /// All letters in lower case
        /// </summary>
        Lowercase,

        /// <summary>
        /// All letters in upper case
        /// </summary>
        Uppercase,

        /// <summary>
        /// First letter of each word capitalized, remaining letters lower case
        /// </summary>
        PascalCase
    }
}
