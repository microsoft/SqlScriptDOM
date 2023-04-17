//------------------------------------------------------------------------------
// <copyright file="IdentifierLiteral.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Text;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    public partial class IdentifierLiteral
    {
        /// <summary>
        /// Sets the Value and the Quoted properties, should be used if the identifier is
        /// known to be unqouted.
        /// </summary>
        /// <param name="text">The parsed identifier.</param>
        internal void SetUnquotedIdentifier(string text)
        {
            Value = text;
            _quoteType = QuoteType.NotQuoted;
        }

        /// <summary>
        /// Sets the Value and the Quoted properties, by removing the delimiters
        /// and unescaping the special characters if necessary.
        /// </summary>
        /// <param name="text">The parsed identifier.</param>
        internal void SetIdentifier(string text)
        {
            Value = Identifier.DecodeIdentifier(text, out _quoteType);
        }
    }
}
