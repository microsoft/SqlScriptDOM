//------------------------------------------------------------------------------
// <copyright file="IdentifierBracketing.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Represents the possible ways of controlling square bracket usage around object
    /// identifiers during script generation.
    /// </summary>
    public enum IdentifierBracketing
    {
        /// <summary>
        /// Preserve the original bracketing/quoting of identifiers
        /// </summary>
        Preserve,

        /// <summary>
        /// Wrap all object identifiers in square brackets
        /// </summary>
        IncludeBrackets,

        /// <summary>
        /// Remove brackets from identifiers that do not require them. Identifiers that
        /// conflict with reserved words or contain special characters retain their brackets.
        /// This is a best-effort transformation: in rare cases an identifier lexes as an ordinary
        /// identifier yet its brackets are semantically required by the surrounding syntax (for
        /// example a schema-qualified name that collides with a special relational operator such as
        /// AI_GENERATE_CHUNKS). Such brackets may be removed, so ExcludeBrackets is not guaranteed to
        /// preserve semantics for every possible input.
        /// </summary>
        ExcludeBrackets
    }
}
