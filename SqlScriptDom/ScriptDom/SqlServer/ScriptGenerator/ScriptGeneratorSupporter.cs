//------------------------------------------------------------------------------
// <copyright file="ScriptGeneratorSupporter.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;
using antlr;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    /// <summary>
    /// Converts token type id numbers to string representations using a lookup
    /// array
    /// </summary>
    internal static partial class ScriptGeneratorSupporter
    {
        #region Constants

        internal const string EscapedRSquareParen = "]]";
        internal const string EscapedQuote = "\"\"";
        internal const string Quote = "\"";

        #endregion
        
        #region GetNNNCase Methods

        public static Int32 TokenTypeCount
        {
            get
            {
                return _typeStrings.Length;
            }
        }

        /// <summary>
        /// Retrieves a version of the specified string, in the casing format specified
        /// </summary>
        /// <param name="str">The string to get a specially cased version of</param>
        /// <param name="casing">The casing method to use</param>
        /// <returns>A version of the string in the casing format specified in <paramref name="casing"/></returns>
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public static string GetCasedString(string str, KeywordCasing casing)
        {
            switch (casing)
            {
                case KeywordCasing.Lowercase:
                    return str.ToLowerInvariant();
                case KeywordCasing.Uppercase:
                    return str.ToUpperInvariant();
                case KeywordCasing.PascalCase:
                    return GetPascalCase(str);
                default:
                    Debug.Fail("Invalid KeywordCasing value");
                    break;
            }
            return String.Empty;
        }

        /// <summary>
        /// Retrieves a Pascal Cased version of the string
        /// </summary>
        /// <param name="str">The string to pascal case</param>
        /// <returns>A Pascal Cased version of the string</returns>
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public static string GetPascalCase(string str)
        {
            // Make the first letter upper case
            str = str.ToLowerInvariant();
            char firstLetter = Char.ToUpperInvariant(str[0]);

            // Return a new string
            StringBuilder sb = new StringBuilder();
            sb.Append(firstLetter);
            sb.Append(str.Substring(1));
            return sb.ToString();
        }

        /// <summary>
        /// Retrieves a string representation of the token, in lower case.
        /// </summary>
        /// <param name="tokenType">The type of token to retrieve a string representation of</param>
        /// <returns>A lower-cased string representation of the token</returns>
        /// <exception cref="System.InvalidOperationException">
        /// The specified token type does not have a single string representation (such as Identifier),
        /// it must be accompanied by a string representation.
        /// </exception>
        public static string GetLowerCase(TSqlTokenType tokenType)
        {
            // The array contains lower case representations already
            if (tokenType < 0 || (int)tokenType >= _typeStrings.Length)
            {
                throw new ArgumentOutOfRangeException("tokenType");
            }

            string typeString = _typeStrings[(int)tokenType];
            if (String.IsNullOrEmpty(typeString))
            {
                throw new InvalidOperationException(
                    String.Format(CultureInfo.CurrentCulture, SqlScriptGeneratorResource.TokenTypeDoesNotHaveStringRepresentation, tokenType));
            }

            return typeString;
        }

        /// <summary>
        /// Retrieves a string representation of the token, in upper case.
        /// </summary>
        /// <param name="tokenType">The type of token to retrieve a string representation of</param>
        /// <returns>A upper-cased string representation of the token</returns>
        /// <exception cref="System.InvalidOperationException">
        /// The specified token type does not have a single string representation (such as Identifier),
        /// it must be accompanied by a string representation.
        /// </exception>
        public static string GetUpperCase(TSqlTokenType tokenType)
        {
            // TODO: Consider a separate Upper Case array, for performance

            // Get the lower case representation, and convert to upper case
            return GetLowerCase(tokenType).ToUpperInvariant();
        }

        /// <summary>
        /// Retrieves a string representation of the token, in pascal case (first letter capitalized,
        /// remaining letters lower case).
        /// </summary>
        /// <param name="tokenType">The type of token to retrieve a string representation of</param>
        /// <returns>A pascal-cased string representation of the token</returns>
        /// <exception cref="System.InvalidOperationException">
        /// The specified token type does not have a single string representation (such as Identifier),
        /// it must be accompanied by a string representation.
        /// </exception>
        public static string GetPascalCase(TSqlTokenType tokenType)
        {
            // TODO: Consider a separate Pascal Cased array, to cover some of the special cases
            // such as RowGuidCol, IdentityCol, and other keywords with "internal words"

            // Get the lower case string and make it Pascal Cased
            return GetPascalCase(GetLowerCase(tokenType));
        }

        #endregion

        #region helper methods

        /// <summary>
        /// Retrieves a string representation of the token, in the casing format specified
        /// </summary>
        /// <param name="tokenType">The type of token to retrieve a string representation of</param>
        /// <param name="casing">The casing method to use</param>
        /// <returns>A string representation of the token</returns>
        /// <exception cref="System.InvalidOperationException">
        /// The specified token type does not have a single string representation (such as Identifier),
        /// it must be accompanied by a string representation.
        /// </exception>
        public static string GetTokenString(TSqlTokenType tokenType, KeywordCasing casing)
        {
            switch (casing)
            {
                case KeywordCasing.Lowercase:
                    return GetLowerCase(tokenType);
                case KeywordCasing.Uppercase:
                    return GetUpperCase(tokenType);
                case KeywordCasing.PascalCase:
                    return GetPascalCase(tokenType);
                default:
                    Debug.Fail("Invalid KeywordCasing value");
                    break;
            }
            return String.Empty;
        }

        /// <summary>
        /// Create a white space token 
        /// </summary>
        /// <param name="count">number of white space characters</param>
        /// <returns></returns>
        public static TSqlParserToken CreateWhitespaceToken(Int32 count)
        {
            // Build the whitespace
            String ws = new String(' ', count);

            // Create a whitespace token and add it to the layout table
            return new TSqlParserToken(TSqlTokenType.WhiteSpace, ws);
        }

        internal static void CheckForNullReference(object variable, string variableName)
        {
            if (variableName == null)
                throw new ArgumentNullException("variableName");

            if (variable == null)
                throw new ArgumentNullException(variableName);
        }

        #endregion
    }
}
