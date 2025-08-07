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

        /// <summary>
        /// Parses security object kind with support for External Model (TSql170+)
        /// </summary>
        /// <param name="identifier1">The first identifier.</param>
        /// <param name="identifier2">The second identifier.</param>
        /// <returns>The security object kind.</returns>
        protected SecurityObjectKind ParseSecurityObjectKindTSql170(Identifier identifier1, Identifier identifier2)
        {
            if (identifier1 == null)
            {
                throw new ArgumentNullException(nameof(identifier1));
            }

            switch (identifier1.Value.ToUpperInvariant())
            {
                case CodeGenerationSupporter.External:
                    Match(identifier2, CodeGenerationSupporter.Model);
                    return SecurityObjectKind.ExternalModel;
                default:
                    // Fall back to the base class implementation for all other cases
                    return TSql160ParserBaseInternal.ParseSecurityObjectKind(identifier1, identifier2);
            }
        }
    }
}
