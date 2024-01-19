//------------------------------------------------------------------------------
// <copyright file="LexingEntryPointsTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    /// <summary>
    /// This class is used in testing entry points which do some parsing
    /// </summary>
    public partial class SqlDomTests
    {
        readonly string[] CommonValidIdentifiers = { "a1", "Employees", "#temp", "[ ab ]", "[authors]", "[ auth]", "\"authors\"", "\" auth \"" };
        readonly string[] CommonInvalidIdentifiers = { "", "[]", "\"\"", "create", "table", " a", "[", "]", "[ab]]" };

        private void CheckIdentifiersAreValid(TSqlParser parser, bool isValid, string[] identsToCheck, params string[] additionalToCheck)
        {
            foreach (string identifier in identsToCheck)
            {
                Assert.AreEqual<bool>(isValid, parser.ValidateIdentifier(identifier), "Failed on:" + identifier);
            }
            foreach (string identifier in additionalToCheck)
            {
                Assert.AreEqual<bool>(isValid, parser.ValidateIdentifier(identifier), "Failed on:" + identifier);
            }
        }

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void ValidateIdentifier80Test()
        {
            TSql80Parser parser = new TSql80Parser(true);
            CheckIdentifiersAreValid(parser, true, CommonValidIdentifiers,
                "pivot", "external", "unpivot", "revert", "tablesample", "merge");
            CheckIdentifiersAreValid(parser, false, CommonInvalidIdentifiers,
                "disk", "precision", "dump", "load");
        }

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void ValidateIdentifier90Test()
        {
            TSql90Parser parser = new TSql90Parser(true);
            CheckIdentifiersAreValid(parser, true, CommonValidIdentifiers,
                "disk", "precision", "merge");
            CheckIdentifiersAreValid(parser, false, CommonInvalidIdentifiers,
                "pivot", "external", "unpivot", "revert", "tablesample", "dump", "load");
        }

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void ValidateIdentifier100Test()
        {
            TSql100Parser parser = new TSql100Parser(true);
            CheckIdentifiersAreValid(parser, true, CommonValidIdentifiers,
                "disk", "precision", "dump", "load");
            CheckIdentifiersAreValid(parser, false, CommonInvalidIdentifiers,
                "pivot", "external", "unpivot", "revert", "tablesample", "merge");
        }

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSqlLexerWhiteSpacesTest()
        {
            StringBuilder sb = new StringBuilder();
            // Append one of each possible meaningless chars from the beginning of ASCII table...
            for (int i = 1; i < 32; ++i)
            {
                if (i != (int)'\n' && i != '\r')
                    sb.Append((char)i);
            }

            string input = sb.ToString();

            ParserTestUtils.ExecuteTestForAllParsers(delegate(TSqlParser parser)
            {
                using (StringReader sr = new StringReader(input))
                {
                    IList<ParseError> errors;
                    IList<TSqlParserToken> tokens = parser.GetTokenStream(sr, out errors);
                    Assert.AreEqual<int>(2, tokens.Count);
                    VerifyToken(tokens[0], TSqlTokenType.WhiteSpace, input, 0, 1, 1);
                    VerifyToken(tokens[1], TSqlTokenType.EndOfFile, null, 29, 1, 30);
                }
            }, true);
        }

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void SeparateTokensForNewlines()
        {
            string input = "/* a \r\n*/\r\nGO  \r\n\r\n  \r\n   ";
          
            ParserTestUtils.ExecuteTestForAllParsers(delegate(TSqlParser parser)
            {
                using (StringReader sr = new StringReader(input))
                {
                    IList<ParseError> errors;
                    IList<TSqlParserToken> tokens = parser.GetTokenStream(sr, out errors);
                    Assert.AreEqual<int>(10, tokens.Count);
                    VerifyToken(tokens[0], TSqlTokenType.MultilineComment, "/* a \r\n*/", 0, 1, 1);
                    VerifyToken(tokens[1], TSqlTokenType.WhiteSpace, "\r\n", 9, 2, 3);
                    VerifyToken(tokens[2], TSqlTokenType.Go, "GO", 11, 3, 1);
                    VerifyToken(tokens[3], TSqlTokenType.WhiteSpace, "  ", 13, 3, 3);
                    VerifyToken(tokens[4], TSqlTokenType.WhiteSpace, "\r\n", 15, 3, 5);
                    VerifyToken(tokens[5], TSqlTokenType.WhiteSpace, "\r\n", 17, 4, 1);
                    VerifyToken(tokens[6], TSqlTokenType.WhiteSpace, "  ", 19, 5, 1);
                    VerifyToken(tokens[7], TSqlTokenType.WhiteSpace, "\r\n", 21, 5, 3);
                    VerifyToken(tokens[8], TSqlTokenType.WhiteSpace, "   ", 23, 6, 1);
                    VerifyToken(tokens[9], TSqlTokenType.EndOfFile, null, 26, 6, 4);
                }
            }, true);
        }

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void GetTokenStreamTest()
        {
            ParserTestUtils.ExecuteTestForAllParsers(GetTokenStreamTestImpl, true);
        }

        public void GetTokenStreamTestImpl(TSqlParser parser)
        {
            string testScript = "GetTokenTypesTests.sql";

            IList<ParseError> errors;
            IList<TSqlParserToken> tokens = ParserTestUtils.ParseTokensFromResource(parser, testScript, out errors);

            Assert.AreEqual<int>(0, errors.Count);
            Assert.AreEqual<int>(21, tokens.Count);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.SingleLineComment, tokens[0].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.WhiteSpace, tokens[1].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.WhiteSpace, tokens[2].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.Create, tokens[3].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.WhiteSpace, tokens[4].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.Table, tokens[5].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.WhiteSpace, tokens[6].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.Identifier, tokens[7].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.WhiteSpace, tokens[8].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.LeftParenthesis, tokens[9].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.Identifier, tokens[10].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.WhiteSpace, tokens[11].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.Identifier, tokens[12].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.Comma, tokens[13].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.WhiteSpace, tokens[14].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.WhiteSpace, tokens[15].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.MultilineComment, tokens[16].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.WhiteSpace, tokens[17].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.Go, tokens[18].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.WhiteSpace, tokens[19].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.EndOfFile, tokens[20].TokenType);
        }

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void GetTokenStreamWithErrorTest()
        {
            ParserTestUtils.ExecuteTestForAllParsers(GetTokenStreamWithErrorTestImpl, true);
        }

        public void GetTokenStreamWithErrorTestImpl(TSqlParser parser)
        {
            string testScript = "GetTokenTypesFailureTests.sql";
            IList<ParseError> errors;

            IList<TSqlParserToken> tokens = ParserTestUtils.ParseTokensFromResource(parser, testScript, out errors);

            Assert.IsNotNull(tokens);
            Assert.AreEqual<int>(10, tokens.Count);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.SingleLineComment, tokens[0].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.WhiteSpace, tokens[1].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.WhiteSpace, tokens[2].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.Create, tokens[3].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.WhiteSpace, tokens[4].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.Table, tokens[5].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.WhiteSpace, tokens[6].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.Identifier, tokens[7].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.WhiteSpace, tokens[8].TokenType);
            Assert.AreEqual<TSqlTokenType>(TSqlTokenType.EndOfFile, tokens[9].TokenType);
            Assert.AreEqual<int>(1, errors.Count);
            Assert.AreEqual<int>(94, errors[0].Offset);
        }


        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void LexingErrorHandler()
        {
            #if NET
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                ParserTestUtils.ExecuteTestForAllParsers(delegate(TSqlParser parser)
                {
                    using (TextReader sr = new StreamReader(Path.Combine(System.Environment.GetEnvironmentVariable("windir"), @"system32\notepad.exe")))
                    {
                        IList<ParseError> errors;
                        parser.GetTokenStream(sr, out errors);
                        ParserTestUtils.LogErrors(errors);
                        Assert.AreEqual<int>(1, errors.Count);
                    }
                }, false);
            }
            #else
            ParserTestUtils.ExecuteTestForAllParsers(delegate(TSqlParser parser)
            {
                using (TextReader sr = new StreamReader(Path.Combine(System.Environment.GetEnvironmentVariable("windir"), @"system32\notepad.exe")))
                {
                    IList<ParseError> errors;
                    parser.GetTokenStream(sr, out errors);
                    ParserTestUtils.LogErrors(errors);
                    Assert.AreEqual<int>(1, errors.Count);
                }
            }, false);
            #endif
        }


        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TokenPositionTest()
        {
            const string multilineTokens1 =
@"'aa
bb' n'cc
dd' [ee
ff] /* /*
*/ */";
            const string multilineTokens2 = "\"gg\rhh\" [ii\nkk]";

            ParserTestUtils.ExecuteTestForAllParsers(delegate(TSqlParser parser)
            {
                using (StringReader sr = new StringReader(multilineTokens1))
                {
                    IList<ParseError> errors;
                    IList<TSqlParserToken> tokens = parser.GetTokenStream(sr, out errors);
                    Assert.AreEqual<int>(8, tokens.Count);
                    VerifyToken(tokens[0], TSqlTokenType.AsciiStringLiteral, $"'aa{System.Environment.NewLine}bb'", 0, 1, 1);
                    VerifyToken(tokens[1], TSqlTokenType.WhiteSpace, " ", System.Environment.NewLine == "\n" ? 7:8, 2, 4);
                    VerifyToken(tokens[2], TSqlTokenType.UnicodeStringLiteral, $"n'cc{System.Environment.NewLine}dd'", System.Environment.NewLine == "\n" ? 8:9, 2, 5);
                    VerifyToken(tokens[3], TSqlTokenType.WhiteSpace, " ", System.Environment.NewLine == "\n" ? 16:18, 3, 4);
                    VerifyToken(tokens[4], TSqlTokenType.QuotedIdentifier, $"[ee{System.Environment.NewLine}ff]", System.Environment.NewLine == "\n" ? 17:19, 3, 5);
                    VerifyToken(tokens[5], TSqlTokenType.WhiteSpace, " ", System.Environment.NewLine == "\n" ? 24:27, 4, 4);
                    VerifyToken(tokens[6], TSqlTokenType.MultilineComment, $"/* /*{System.Environment.NewLine}*/ */", System.Environment.NewLine == "\n" ? 25:28, 4, 5);
                    VerifyToken(tokens[7], TSqlTokenType.EndOfFile, null, System.Environment.NewLine == "\n" ? 36:40, 5, 6);
                }

                using (StringReader sr = new StringReader(multilineTokens2))
                {
                    IList<ParseError> errors;
                    IList<TSqlParserToken> tokens = parser.GetTokenStream(sr, out errors);
                    Assert.AreEqual<int>(4, tokens.Count);
                    VerifyToken(tokens[0], TSqlTokenType.AsciiStringOrQuotedIdentifier, "\"gg\rhh\"", 0, 1, 1);
                    VerifyToken(tokens[1], TSqlTokenType.WhiteSpace, " ", 7, 2, 4);
                    VerifyToken(tokens[2], TSqlTokenType.QuotedIdentifier, "[ii\nkk]", 8, 2, 5);
                    VerifyToken(tokens[3], TSqlTokenType.EndOfFile, null, 15, 3, 4);
                }
            }, true);
        }

        private static void VerifyToken(TSqlParserToken token, TSqlTokenType tokenType, string text, int offset, int line, int column)
        {
            Assert.AreEqual<TSqlTokenType>(tokenType, token.TokenType);
            Assert.AreEqual<string>(text, token.Text);
            Assert.AreEqual<int>(offset, token.Offset);
            Assert.AreEqual<int>(line, token.Line);
            Assert.AreEqual<int>(column, token.Column);
        }


        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSqlLexerLiteralsTest()
        {
            VerifyLiteralTokens("1. 1.0 0.1 .1 2147483648 02147483648 002147483647 000000000000", TSqlTokenType.Numeric);
            VerifyLiteralTokens("1 1 2147483647 02147483647", TSqlTokenType.Integer);
            VerifyLiteralTokens("$1 $+1 £-10.00", TSqlTokenType.Money);
            VerifyLiteralTokens("1e1 1.0e1 10.0e-10", TSqlTokenType.Real);
            VerifyLiteralTokens("0x 0xABcD 0xA", TSqlTokenType.HexLiteral);
        }

        private void VerifyLiteralTokens(string input, TSqlTokenType expected)
        {
            ParserTestUtils.ExecuteTestForAllParsers(delegate(TSqlParser parser)
            {
                using (StringReader sr = new StringReader(input))
                {
                    IList<ParseError> errors;
                    IList<TSqlParserToken> tokens = parser.GetTokenStream(sr, out errors);
                    Assert.AreEqual<int>(0, errors.Count);
                    foreach (TSqlParserToken token in tokens)
                    {
                        if (token.TokenType != TSqlTokenType.WhiteSpace && token.TokenType != TSqlTokenType.EndOfFile)
                        {
                            Assert.AreEqual<TSqlTokenType>(expected, token.TokenType);
                        }
                    }
                }
            }, true);
        }

        /// <summary>
        /// Verifies behavior of the TSqlParserToken.IsKeyword method.
        /// </summary>
        
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSqlParserTokenKeywordTest()
        {
            Assert.AreEqual<int>(188, (int)TSqlTokenType.Bang, "List of keywords has changed - verify TSqlParserToken.IsKeyword logic remains correct.");
            Assert.AreEqual<int>(1, (int)TSqlTokenType.EndOfFile, "List of keywords has changed - verify TSqlParserToken.IsKeyword logic remains correct.");
            Assert.IsFalse(new TSqlParserToken(TSqlTokenType.Bang, "!").IsKeyword());
            Assert.IsFalse(new TSqlParserToken(TSqlTokenType.EndOfFile, "\n").IsKeyword());
            Assert.IsFalse(new TSqlParserToken(TSqlTokenType.None, "").IsKeyword());
            Assert.IsFalse(new TSqlParserToken(TSqlTokenType.Go, "go").IsKeyword());
            Assert.IsFalse(new TSqlParserToken(TSqlTokenType.Label, "label:").IsKeyword());
            Assert.IsFalse(new TSqlParserToken(TSqlTokenType.Identifier, "myname").IsKeyword());
            Assert.IsTrue(new TSqlParserToken(TSqlTokenType.Add, "add").IsKeyword());
            Assert.IsTrue(new TSqlParserToken(TSqlTokenType.StopList, "stoplist").IsKeyword());
        }
    }
}