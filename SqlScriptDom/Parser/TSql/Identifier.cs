//------------------------------------------------------------------------------
// <copyright file="Identifier.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Text;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    public partial class Identifier
    {
        private const string EscapedRSquareParen = "]]";
        private const string EscapedQuote = "\"\"";
        private const string Quote = "\"";

        private const char LSquareParenChar = '[';
        private const char RSquareParenChar = ']';
        private const char QuoteChar = '"';
        internal const int MaxIdentifierLength = 128;


        // TODO, jacl: since these methods don't actually work on identifier objects, we should do one of two things: 
        // a) introduce overloads that do work on identifier objects, or 
        // b) rename to be "IdentifierString" suffix
        
        /// <summary>
        /// Removes the escape characters.
        /// </summary>
        /// <param name="identifier">The identifier which has escape characters.</param>
        /// <param name="quote">The quote type</param>
        /// <returns>The unescaped identifier.</returns>
        
        public static string DecodeIdentifier(string identifier, out QuoteType quote)
        {            
            if (identifier == null || identifier.Length <=2)
            {
                quote = QuoteType.NotQuoted;
                return identifier;
            }            

            if ((identifier.Length != 0)
                && (identifier[0] == LSquareParenChar || identifier[0] == QuoteChar))
            {
                string value;
                // Trim the ends first.                
                value = identifier.Substring(1, identifier.Length - 2);

                if (identifier[0] == LSquareParenChar)
                {
                    quote = QuoteType.SquareBracket;
                    return value.Replace(EscapedRSquareParen, CodeGenerationSupporter.RSquareParen);
                }
                else
                {
                    Debug.Assert(identifier[0] == QuoteChar);
                    quote = QuoteType.DoubleQuote;
                    return value.Replace(EscapedQuote, Quote);
                }
            }
            else
            {
                quote = QuoteType.NotQuoted;
                return identifier;
            }
        }

        /// <summary>
        /// Adds the escape characters.
        /// </summary>
        /// <param name="identifier">The identifier which does not have escape characters.</param>
        /// <returns>The escaped identifier.</returns>

        public static string EncodeIdentifier(string identifier)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(CodeGenerationSupporter.LSquareParen);
            // Escape the special character
            builder.Append(identifier.Replace(CodeGenerationSupporter.RSquareParen, EscapedRSquareParen));
            builder.Append(CodeGenerationSupporter.RSquareParen);
            return builder.ToString();
        }

		/// <summary>
        /// Adds the escape characters.
        /// </summary>
        /// <param name="identifier">The identifier which does not have escape characters.</param>
        /// <param name="quoteType">The quote type</param>
        /// <returns>The escaped identifier.</returns>
        
        public static string EncodeIdentifier(string identifier, QuoteType quoteType)
        {
            StringBuilder builder = new StringBuilder();
            switch (quoteType)
            {
                case QuoteType.NotQuoted:
                    return identifier;
                case QuoteType.SquareBracket:
                    builder.Append(CodeGenerationSupporter.LSquareParen);
                    // Escape the special character
                    builder.Append(identifier.Replace(CodeGenerationSupporter.RSquareParen, EscapedRSquareParen));
                    builder.Append(CodeGenerationSupporter.RSquareParen);
                    break;
                case QuoteType.DoubleQuote:
                    builder.Append(Quote);
                    // Escape the special character
                    builder.Append(identifier.Replace(Quote, EscapedQuote));
                    builder.Append(Quote);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("quoteType");
            }
            return builder.ToString();
        }

        /// <summary>
        /// Sets the Value and the Quoted properties, should be used if the identifier is
        /// known to be unqouted.
        /// </summary>
        /// <param name="text">The parsed identifier.</param>
        internal void SetUnquotedIdentifier(string text)
        {
            _value = text;
            _quoteType = QuoteType.NotQuoted;
        }

        /// <summary>
        /// Sets the Value and the Quoted properties, by removing the delimiters
        /// and unescaping the special characters if necessary.
        /// </summary>
        /// <param name="text">The parsed identifier.</param>
        internal void SetIdentifier(string text)
        {
            _value = DecodeIdentifier(text, out _quoteType);
        }
    }
}
