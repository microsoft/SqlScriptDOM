//------------------------------------------------------------------------------
// <copyright file="TSql170ParserBaseInternals.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using antlr;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal abstract class TSql170ParserBaseInternal : TSql160ParserBaseInternal
    {
        #region Constructors

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql170ParserBaseInternal(TokenBuffer tokenBuf, int k)
            : base(tokenBuf, k)
        {
        }

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql170ParserBaseInternal(ParserSharedInputState state, int k)
            : base(state, k)
        {
        }

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql170ParserBaseInternal(TokenStream lexer, int k)
            : base(lexer, k)
        {
        }

        /// <summary>
        /// Real constructor (the one which is used)
        /// </summary>
        /// <param name="initialQuotedIdentifiersOn">if set to <c>true</c> initial quoted identifiers will be set to on.</param>
        public TSql170ParserBaseInternal(bool initialQuotedIdentifiersOn)
            : base(initialQuotedIdentifiersOn)
        {
        }

        #endregion
    }
}
