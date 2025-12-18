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

        /// <summary>
        /// Checks if VECTOR keyword appears in the upcoming tokens within a reasonable lookahead window.
        /// Used to determine if SaveGuessing optimization is needed for VECTOR data type parsing.
        /// </summary>
        /// <returns>true if VECTOR keyword found in lookahead; false otherwise</returns>
        protected bool ContainsVectorInLookahead()
        {
            // Scan ahead looking for VECTOR keyword (case-insensitive identifier match)

            const int LookaheadLimit = 100; // Define a named constant for the lookahead limit
            // We scan up to LookaheadLimit tokens to handle deeply nested JOIN structures with VECTOR types
            for (int i = 1; i <= LookaheadLimit; i++)
            {
                IToken token;
                try
                {
                    token = LT(i);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error accessing token at lookahead index {i}: {ex.Message}");
                    break;
                }
                if (token == null || token.Type == Token.EOF_TYPE)
                {
                    break;
                }
                
                // Check if this is an identifier token with text "VECTOR"
                if (token.Type == TSql170ParserInternal.Identifier &&
                    string.Equals(token.getText(), CodeGenerationSupporter.Vector, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}
