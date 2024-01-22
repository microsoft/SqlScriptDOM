//------------------------------------------------------------------------------
// <copyright file="TestUtilities.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    internal static class ParserTestUtils
    {
        public static void ExecuteTestForAllParsers(Action<TSqlParser> test, bool quotedIdentifiers)
        {
            ExecuteTestForParsers(test,
                new TSql80Parser(quotedIdentifiers),
                new TSql90Parser(quotedIdentifiers),
                new TSql100Parser(quotedIdentifiers),
                new TSql110Parser(quotedIdentifiers),
                new TSql120Parser(quotedIdentifiers),
                new TSql130Parser(quotedIdentifiers),
                new TSql140Parser(quotedIdentifiers),
                new TSql150Parser(quotedIdentifiers),
                new TSql160Parser(quotedIdentifiers));
        }

        public static void ExecuteTestForParsers(Action<TSqlParser> test, params TSqlParser[] parsers)
        {
            Assert.IsTrue(parsers.Length > 0);

            foreach (TSqlParser parser in parsers)
                test(parser);
        }

        internal static SqlScriptGenerator CreateScriptGen(SqlVersion sqlVersion)
        {
            SqlScriptGeneratorOptions options = new SqlScriptGeneratorOptions();
            options.AlignClauseBodies = false;
            switch (sqlVersion)
            {
                case SqlVersion.Sql80:
                    return new Sql80ScriptGenerator(options);
                case SqlVersion.Sql90:
                    return new Sql90ScriptGenerator(options);
                case SqlVersion.Sql100:
                    return new Sql100ScriptGenerator(options);
                case SqlVersion.Sql110:
                    return new Sql110ScriptGenerator(options);
                case SqlVersion.Sql120:
                    return new Sql120ScriptGenerator(options);
                case SqlVersion.Sql130:
                    return new Sql130ScriptGenerator(options);
                case SqlVersion.Sql140:
                    return new Sql140ScriptGenerator(options);
                case SqlVersion.Sql150:
                    return new Sql150ScriptGenerator(options);
                case SqlVersion.Sql160:
                    return new Sql160ScriptGenerator(options);
                default:
                    Debug.Assert(false, "Unknown SQL version");
                    return null;
            }
        }

        /// <summary>
        /// Tests for occured errors.
        /// </summary>
        internal static void ErrorTest(TSqlParser parser, string testScript, params ParserErrorInfo[] expectedErrors)
        {
            IList<ParseError> errors;
            TSqlFragment fragment = ParseString(parser, testScript, out errors);

            int len = expectedErrors.Length;
            if (len != errors.Count)
                ParserTestUtils.LogErrors(errors);

            Assert.AreEqual<int>(len, errors.Count, "Incorrect number of errors found");
            for (int i = 0; i < len; ++i)
            {
                expectedErrors[i].VerifyError(errors[i]);
            }
        }

        internal static void ErrorTestAllParsers(string testScript, params ParserErrorInfo[] expectedErrors)
        {
            const bool quotedIdentifiers = true;

            TSql80Parser parser80 = new TSql80Parser(quotedIdentifiers);
            ErrorTest(parser80, testScript, expectedErrors);
            TSql90Parser parser90 = new TSql90Parser(quotedIdentifiers);
            ErrorTest(parser90, testScript, expectedErrors);
            TSql100Parser parser100 = new TSql100Parser(quotedIdentifiers);
            ErrorTest(parser100, testScript, expectedErrors);
            TSql110Parser parser110 = new TSql110Parser(quotedIdentifiers);
            ErrorTest(parser110, testScript, expectedErrors);
            TSql120Parser parser120 = new TSql120Parser(quotedIdentifiers);
            ErrorTest(parser120, testScript, expectedErrors);
            TSql130Parser parser130 = new TSql130Parser(quotedIdentifiers);
            ErrorTest(parser130, testScript, expectedErrors);
            TSql140Parser parser140 = new TSql140Parser(quotedIdentifiers);
            ErrorTest(parser140, testScript, expectedErrors);
            TSql150Parser parser150 = new TSql150Parser(quotedIdentifiers);
            ErrorTest(parser150, testScript, expectedErrors);
            TSql160Parser parser160 = new TSql160Parser(quotedIdentifiers);
            ErrorTest(parser160, testScript, expectedErrors);
        }

        internal static void ErrorTestAllParsersUntil150(string testScript, params ParserErrorInfo[] expectedErrors)
        {
            const bool quotedIdentifiers = true;

            TSql80Parser parser80 = new TSql80Parser(quotedIdentifiers);
            ErrorTest(parser80, testScript, expectedErrors);
            TSql90Parser parser90 = new TSql90Parser(quotedIdentifiers);
            ErrorTest(parser90, testScript, expectedErrors);
            TSql100Parser parser100 = new TSql100Parser(quotedIdentifiers);
            ErrorTest(parser100, testScript, expectedErrors);
            TSql110Parser parser110 = new TSql110Parser(quotedIdentifiers);
            ErrorTest(parser110, testScript, expectedErrors);
            TSql120Parser parser120 = new TSql120Parser(quotedIdentifiers);
            ErrorTest(parser120, testScript, expectedErrors);
            TSql130Parser parser130 = new TSql130Parser(quotedIdentifiers);
            ErrorTest(parser130, testScript, expectedErrors);
            TSql140Parser parser140 = new TSql140Parser(quotedIdentifiers);
            ErrorTest(parser140, testScript, expectedErrors);
        }

        internal static void ErrorTest90AndAbove(string testScript, params ParserErrorInfo[] expectedErrors)
        {
            const bool quotedIdentifiers = true;

            TSql90Parser parser90 = new TSql90Parser(quotedIdentifiers);
            ErrorTest(parser90, testScript, expectedErrors);
            TSql100Parser parser100 = new TSql100Parser(quotedIdentifiers);
            ErrorTest(parser100, testScript, expectedErrors);
            TSql110Parser parser110 = new TSql110Parser(quotedIdentifiers);
            ErrorTest(parser110, testScript, expectedErrors);
            TSql120Parser parser120 = new TSql120Parser(quotedIdentifiers);
            ErrorTest(parser120, testScript, expectedErrors);
            TSql130Parser parser130 = new TSql130Parser(quotedIdentifiers);
            ErrorTest(parser130, testScript, expectedErrors);
            TSql140Parser parser140 = new TSql140Parser(quotedIdentifiers);
            ErrorTest(parser140, testScript, expectedErrors);
            TSql150Parser parser150 = new TSql150Parser(quotedIdentifiers);
            ErrorTest(parser150, testScript, expectedErrors);
            TSql160Parser parser160 = new TSql160Parser(quotedIdentifiers);
            ErrorTest(parser160, testScript, expectedErrors);
        }

        internal static void ErrorTest90andAboveUntil150(string testScript, params ParserErrorInfo[] expectedErrors)
        {
            const bool quotedIdentifiers = true;

            TSql90Parser parser90 = new TSql90Parser(quotedIdentifiers);
            ErrorTest(parser90, testScript, expectedErrors);
            TSql100Parser parser100 = new TSql100Parser(quotedIdentifiers);
            ErrorTest(parser100, testScript, expectedErrors);
            TSql110Parser parser110 = new TSql110Parser(quotedIdentifiers);
            ErrorTest(parser110, testScript, expectedErrors);
            TSql120Parser parser120 = new TSql120Parser(quotedIdentifiers);
            ErrorTest(parser120, testScript, expectedErrors);
            TSql130Parser parser130 = new TSql130Parser(quotedIdentifiers);
            ErrorTest(parser130, testScript, expectedErrors);
            TSql140Parser parser140 = new TSql140Parser(quotedIdentifiers);
            ErrorTest(parser140, testScript, expectedErrors);
        }

        internal static void ErrorTest100(string testScript, params ParserErrorInfo[] expectedErrors)
        {
            const bool quotedIdentifiers = true;

            TSql100Parser parser100 = new TSql100Parser(quotedIdentifiers);
            ErrorTest(parser100, testScript, expectedErrors);
        }

        internal static void ErrorTest100AndAbove(string testScript, params ParserErrorInfo[] expectedErrors)
        {
            const bool quotedIdentifiers = true;

            TSql100Parser parser100 = new TSql100Parser(quotedIdentifiers);
            ErrorTest(parser100, testScript, expectedErrors);
            TSql110Parser parser110 = new TSql110Parser(quotedIdentifiers);
            ErrorTest(parser110, testScript, expectedErrors);
            TSql120Parser parser120 = new TSql120Parser(quotedIdentifiers);
            ErrorTest(parser120, testScript, expectedErrors);
            TSql130Parser parser130 = new TSql130Parser(quotedIdentifiers);
            ErrorTest(parser130, testScript, expectedErrors);
            TSql140Parser parser140 = new TSql140Parser(quotedIdentifiers);
            ErrorTest(parser140, testScript, expectedErrors);
            TSql150Parser parser150 = new TSql150Parser(quotedIdentifiers);
            ErrorTest(parser150, testScript, expectedErrors);
            TSql160Parser parser160 = new TSql160Parser(quotedIdentifiers);
            ErrorTest(parser160, testScript, expectedErrors);
        }

        internal static void ErrorTest110(string testScript, params ParserErrorInfo[] expectedErrors)
        {
            const bool quotedIdentifiers = true;

            TSql110Parser parser110 = new TSql110Parser(quotedIdentifiers);
            ErrorTest(parser110, testScript, expectedErrors);
        }

        internal static void ErrorTest120(string testScript, params ParserErrorInfo[] expectedErrors)
        {
            const bool quotedIdentifiers = true;

            TSql120Parser parser120 = new TSql120Parser(quotedIdentifiers);
            ErrorTest(parser120, testScript, expectedErrors);
        }

        internal static void ErrorTest130(string testScript, params ParserErrorInfo[] expectedErrors)
        {
            const bool quotedIdentifiers = true;

            TSql130Parser parser130 = new TSql130Parser(quotedIdentifiers);
            ErrorTest(parser130, testScript, expectedErrors);
        }

        internal static void ErrorTest140(string testScript, params ParserErrorInfo[] expectedErrors)
        {
            const bool quotedIdentifiers = true;

            TSql140Parser parser140 = new TSql140Parser(quotedIdentifiers);
            ErrorTest(parser140, testScript, expectedErrors);
        }

        internal static void ErrorTest150(string testScript, params ParserErrorInfo[] expectedErrors)
        {
            const bool quotedIdentifiers = true;

            TSql150Parser parser150 = new TSql150Parser(quotedIdentifiers);
            ErrorTest(parser150, testScript, expectedErrors);
        }

        internal static void ErrorTest160(string testScript, params ParserErrorInfo[] expectedErrors)
        {
            const bool quotedIdentifiers = true;

            TSql160Parser parser160 = new TSql160Parser(quotedIdentifiers);
            ErrorTest(parser160, testScript, expectedErrors);
        }

        internal static void ErrorTest<T>(string testScript, params ParserErrorInfo[] expectedErrors) where T : TSqlParser
        {
            const bool quotedIdentifiers = true;

            T parser = (T)typeof(T).GetConstructor(new Type[] { typeof(bool) }).Invoke(new object[] { quotedIdentifiers });

            ErrorTest(parser, testScript, expectedErrors);
        }

        /// <summary>
        /// Creates a stream reader from the manifest resource.
        /// </summary>
        /// <param name="resourceName">The manifest resources name.</param>
        /// <returns>The created stream reader.</returns>
        public static StreamReader GetStreamReaderFromManifestResource(string resourceName)
        {
            return new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName));
        }

        public static string GetStringFromResource(string resourceName)
        {
            string result = null;
            using (StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)))
            {
                result = sr.ReadToEnd();
            }

            #if NET
            // Convert line endings from \n to \r\n
            if (System.Environment.NewLine == "\n")
                result = result.ReplaceLineEndings("\r\n");
            #endif

            return result;
        }

        internal static void LogErrors(IList<ParseError> errors)
        {
            foreach (ParseError error in errors)
                LogError(error);
        }

        internal static void LogError(ParseError error)
        {
            string errorText = String.Format("SQL{0}: {1} at offset {2}, line {3}, column {4}", error.Number, error.Message, error.Offset, error.Line, error.Column);
            System.Diagnostics.Trace.WriteLine(errorText);
        }

        internal static void LogErrors(IList<ParseError> errors, string source)
        {
            foreach (ParseError error in errors)
            {
                int row;
                int column;

                GetRowColumn(source, error.Offset, out row, out column);
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat(CultureInfo.InvariantCulture, "Parse Error SQL{0} in row:{1} column: {2}", error.Number, row, column);
                System.Diagnostics.Trace.WriteLine(builder.ToString());
            }
        }

        /// <summary>
        /// Assumes good input, otherwise take special care
        /// for offset > file length, also special care for \r\n
        /// also specially handle unicode
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="offset">The offset in the script.</param>
        /// <param name="row">The row number for the corresponding offset.</param>
        /// <param name="column">The column number for the corresponding offset.</param>
        static void GetRowColumn(string source, int offset, out int row, out int column)
        {
            row = 1;
            column = 1;

            // either do current < offset or
            // current < offset + 1 and then --column before returning
            for (int current = 0; current < offset; current++)
            {
                char ch1 = source[current];
                if (ch1 == '\n')
                {
                    ++row;
                    column = 1;
                }
                else if (ch1 == '\r')
                {
                    char ch2 = source[current+1];
                    if (ch2 == '\n')
                    {
                        ++current;
                    }
                    ++row;
                    column = 1;
                }
                else
                {
                    ++column;
                }
            }
        }

        public static TSqlFragment ParseStringNoErrors(TSqlParser parser, string source)
        {
            IList<ParseError> errors;
            TSqlFragment fragment = ParseString(parser, source, out errors);

            LogErrors(errors, source);
            Assert.AreEqual<int>(0, errors.Count);

            return fragment;
        }

        public static TSqlFragment ParseString(TSqlParser parser, string source, out IList<ParseError> errors)
        {
            TSqlFragment fragment;
            using (StringReader sr = new StringReader(source))
            {
                fragment = parser.Parse(sr, out errors);
            }
            return fragment;
        }

        public static TSqlFragment ParseFromResource(TSqlParser parser, string sourceFilename, out IList<ParseError> errors)
        {
            string source = null;
            using (StreamReader sr = ParserTestUtils.GetStreamReaderFromManifestResource(GlobalConstants.TestScriptsNameSpace + "." + sourceFilename))
            {
                source = sr.ReadToEnd();
            }

            #if NET
            // Convert line endings from \n to \r\n
            if (System.Environment.NewLine == "\n")
                source = source.ReplaceLineEndings("\r\n");
            #endif

            using(TextReader tr = new StringReader(source))
            {
                return parser.Parse(tr, out errors);
            }
        }

        public static IList<TSqlParserToken> ParseTokensFromResource(TSqlParser parser, string sourceFilename, out IList<ParseError> errors)
        {
            string source = null;
            using (StreamReader sr = ParserTestUtils.GetStreamReaderFromManifestResource(GlobalConstants.TestScriptsNameSpace + "." + sourceFilename))
            {
                source = sr.ReadToEnd();
            }

            #if NET
            // Convert line endings from \n to \r\n
            if (System.Environment.NewLine == "\n")
                source = source.ReplaceLineEndings("\r\n");
            #endif

            using (TextReader tr = new StringReader(source))
            {
                return parser.GetTokenStream(tr, out errors);
            }
        }
    }
}
