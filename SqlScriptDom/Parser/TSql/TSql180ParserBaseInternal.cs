//------------------------------------------------------------------------------
// <copyright file="TSql180ParserBaseInternals.cs" company="Microsoft">
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
    internal abstract class TSql180ParserBaseInternal : TSql170ParserBaseInternal
    {
        #region Constructors

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql180ParserBaseInternal(TokenBuffer tokenBuf, int k)
            : base(tokenBuf, k)
        {
        }

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql180ParserBaseInternal(ParserSharedInputState state, int k)
            : base(state, k)
        {
        }

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql180ParserBaseInternal(TokenStream lexer, int k)
            : base(lexer, k)
        {
        }

        /// <summary>
        /// Real constructor (the one which is used)
        /// </summary>
        /// <param name="initialQuotedIdentifiersOn">if set to <c>true</c> initial quoted identifiers will be set to on.</param>
        public TSql180ParserBaseInternal(bool initialQuotedIdentifiersOn)
            : base(initialQuotedIdentifiersOn)
        {
        }

        #endregion

        /// <summary>
        /// Parses security object kind with support for External Model (TSql180+)
        /// </summary>
        /// <param name="identifier1">The first identifier.</param>
        /// <param name="identifier2">The second identifier.</param>
        /// <returns>The security object kind.</returns>
        protected SecurityObjectKind ParseSecurityObjectKindTSql180(Identifier identifier1, Identifier identifier2)
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
                    return TSql170ParserBaseInternal.ParseSecurityObjectKind(identifier1, identifier2);
            }
        }

        /// <summary>
        /// Validates that EXTERNAL_MODEL is specified when CREATE SEMANTIC INDEX contains vector or hybrid columns.
        /// Vector and hybrid search types require an external model for embeddings.
        /// NotSpecified defaults to Vector, so EXTERNAL_MODEL is required unless ALL columns are explicitly Fulltext.
        /// </summary>
        /// <param name="statement">The CREATE SEMANTIC INDEX statement to validate.</param>
        protected static void ValidateSemanticIndexExternalModel(CreateSemanticIndexStatement statement)
        {
            // EXTERNAL_MODEL is NOT required only when ALL columns are explicitly FULLTEXT
            // If any column is Vector, Hybrid, or NotSpecified (defaults to Vector), EXTERNAL_MODEL is required
            bool allColumnsAreFulltext = statement.Columns.All(col =>
                col.SearchType == SemanticIndexSearchType.Fulltext);

            // EXTERNAL_MODEL is required unless all columns are explicitly fulltext
            if (!allColumnsAreFulltext && statement.ExternalModelName == null)
            {
                ThrowParseErrorException("SQL46144", statement,
                    TSqlParserResource.SQL46144Message, "EXTERNAL_MODEL");
            }
        }
    }
}

