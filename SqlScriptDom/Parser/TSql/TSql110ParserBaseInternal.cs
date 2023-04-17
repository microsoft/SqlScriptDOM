//------------------------------------------------------------------------------
// <copyright file="TSql110ParserBaseInternals.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using antlr;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    internal abstract class TSql110ParserBaseInternal : TSql100ParserBaseInternal
    {
        #region Constructors

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql110ParserBaseInternal(TokenBuffer tokenBuf, int k)
            : base(tokenBuf, k)
        {
        }

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql110ParserBaseInternal(ParserSharedInputState state, int k)
            : base(state, k)
        {
        }

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql110ParserBaseInternal(TokenStream lexer, int k)
            : base(lexer, k)
        {
        }

        /// <summary>
        /// Real constructor (the one which is used)
        /// </summary>
        /// <param name="initialQuotedIdentifiersOn">if set to <c>true</c> initial quoted identifiers will be set to on.</param>
        public TSql110ParserBaseInternal(bool initialQuotedIdentifiersOn)
            : base(initialQuotedIdentifiersOn)
        {
        }

        #endregion

        private static bool isFollowingDelimiter(WindowDelimiter delimiter)
        {
            return delimiter != null &&
                (delimiter.WindowDelimiterType == WindowDelimiterType.ValueFollowing ||
                 delimiter.WindowDelimiterType == WindowDelimiterType.UnboundedFollowing);
        }

        private static bool isPrecedingDelimiter(WindowDelimiter delimiter)
        {
            return delimiter != null &&
                (delimiter.WindowDelimiterType == WindowDelimiterType.ValuePreceding ||
                 delimiter.WindowDelimiterType == WindowDelimiterType.UnboundedPreceding);
        }

        protected static void CheckWindowFrame(WindowFrameClause windowFrameClause)
        {
            bool topHasValueSpecified = 
                windowFrameClause.Top != null &&
                (windowFrameClause.Top.WindowDelimiterType == WindowDelimiterType.ValuePreceding || 
                 windowFrameClause.Top.WindowDelimiterType == WindowDelimiterType.ValueFollowing);
            bool bottomHasValueSpecified = 
                windowFrameClause.Bottom != null &&
                (windowFrameClause.Bottom.WindowDelimiterType == WindowDelimiterType.ValuePreceding || 
                 windowFrameClause.Bottom.WindowDelimiterType == WindowDelimiterType.ValueFollowing);

            if (windowFrameClause.WindowFrameType == WindowFrameType.Range &&
                (topHasValueSpecified || bottomHasValueSpecified))
            {
                ThrowParseErrorException("SQL46099", windowFrameClause, TSqlParserResource.SQL46099Message);
            }

            if (windowFrameClause.Top == null)
            {
                ThrowParseErrorException("SQL46100", windowFrameClause, TSqlParserResource.SQL46100Message);
            }

            if (windowFrameClause.Bottom == null && isFollowingDelimiter(windowFrameClause.Top))
            {
                ThrowParseErrorException("SQL46100", windowFrameClause, TSqlParserResource.SQL46100Message);
            }

            if (isFollowingDelimiter(windowFrameClause.Top) && isPrecedingDelimiter(windowFrameClause.Bottom))
            {
                ThrowParseErrorException("SQL46100", windowFrameClause, TSqlParserResource.SQL46100Message);
            }

            if (windowFrameClause.Top.WindowDelimiterType == WindowDelimiterType.UnboundedFollowing ||
                (windowFrameClause.Bottom != null && windowFrameClause.Bottom.WindowDelimiterType == WindowDelimiterType.UnboundedPreceding))
            {
                ThrowParseErrorException("SQL46100", windowFrameClause, TSqlParserResource.SQL46100Message);
            }

            if (isFollowingDelimiter(windowFrameClause.Top) && windowFrameClause.Bottom != null && 
                windowFrameClause.Bottom.WindowDelimiterType == WindowDelimiterType.CurrentRow)
            {
                ThrowParseErrorException("SQL46100", windowFrameClause, TSqlParserResource.SQL46100Message);
            }

            if (windowFrameClause.Top.WindowDelimiterType == WindowDelimiterType.CurrentRow &&
                isPrecedingDelimiter(windowFrameClause.Bottom))
            {
                ThrowParseErrorException("SQL46100", windowFrameClause, TSqlParserResource.SQL46100Message);
            }
        }

        //Options that are valid for both Create and Alter Database.
        //Note that while DbChaining and Trustworthy are valid for both, we parse them using the regular
        //Alter Database rules because they allow duplicates in Alter Db, but not Create Db.
        //
        private static string[] _optionsValidForCreateDatabase = new string[] 
        {    CodeGenerationSupporter.Containment,
            CodeGenerationSupporter.DefaultLanguage,
            CodeGenerationSupporter.DefaultFullTextLanguage,
            CodeGenerationSupporter.FileStream,
            CodeGenerationSupporter.NestedTriggers,
            CodeGenerationSupporter.TransformNoiseWords,
            CodeGenerationSupporter.TwoDigitYearCutoff,
        };

        protected string[] OptionValidForCreateDatabase()
        {
            return _optionsValidForCreateDatabase;
        }

    }
}
