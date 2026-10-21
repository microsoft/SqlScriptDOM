//------------------------------------------------------------------------------
// <copyright file="UnicodeNonCharacterLexerTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    /// <summary>
    /// Regression tests for the lexer treating the Unicode non-character U+FFFF
    /// (char.MaxValue) as an end-of-input sentinel. A literal U+FFFF appearing inside
    /// a string literal, delimited identifier, or comment must be scanned as a normal
    /// character; real end-of-input must be detected by stream position only.
    ///
    /// Downstream: DacFx GitHub issue #796 - stored procedures
    /// whose bodies contain NCHAR(0xFFFE)/NCHAR(0xFFFF) caused continuous schema drift
    /// because ScriptDom stopped scanning at the literal U+FFFF.
    /// </summary>
    public partial class SqlDomTests
    {
        // Unicode non-characters. U+FFFE is handled fine today; only U+FFFF
        // (== char.MaxValue == CharScanner.EOF_CHAR) triggers the failure.
        private const string UFFFE = "\uFFFE";
        private const string UFFFF = "\uFFFF";

        private static TSqlScript ParseScript(TSqlParser parser, string source, out IList<ParseError> errors)
        {
            using (StringReader reader = new StringReader(source))
            {
                return (TSqlScript)parser.Parse(reader, out errors);
            }
        }

        /// <summary>
        /// Lexer-level guarantee across every parser version: an N'...' string literal
        /// containing both U+FFFE and U+FFFF parses cleanly and the literal value
        /// round-trips with the U+FFFF preserved (proving the scanner consumed past it).
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void LexerTreatsUFFFFAsLiteralInStringLiteral()
        {
            string source = "SELECT N'hi" + UFFFE + UFFFF + "lo' AS c;";

            ParserTestUtils.ExecuteTestForAllParsers(parser =>
            {
                TSqlScript script = ParseScript(parser, source, out IList<ParseError> errors);

                ParserTestUtils.LogErrors(errors, source);
                Assert.AreEqual(0, errors.Count, "U+FFFF inside a string literal must not terminate scanning.");

                SelectStatement select = (SelectStatement)script.Batches[0].Statements[0];
                QuerySpecification query = (QuerySpecification)select.QueryExpression;
                SelectScalarExpression element = (SelectScalarExpression)query.SelectElements[0];
                StringLiteral literal = (StringLiteral)element.Expression;

                Assert.AreEqual("hi" + UFFFE + UFFFF + "lo", literal.Value,
                    "The U+FFFF must round-trip as a literal character inside the string literal.");
            }, true);
        }

        /// <summary>
        /// The verbatim DacFx minimal repro: a CREATE PROCEDURE whose body contains a
        /// string literal with U+FFFE + U+FFFF must parse to a single complete statement
        /// that spans the entire input.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void ParseProcedureWithUnicodeNonCharactersIsComplete()
        {
            string body =
                "CREATE PROCEDURE dbo.p AS BEGIN " +
                "DECLARE @s NVARCHAR(50) = N'hi" + UFFFE + UFFFF + "lo'; " +
                "SELECT @s; END";

            ParserTestUtils.ExecuteTestForParsers(parser =>
            {
                TSqlScript script = ParseScript(parser, body, out IList<ParseError> errors);

                ParserTestUtils.LogErrors(errors, body);
                Assert.AreEqual(0, errors.Count, "U+FFFF in the procedure body must not terminate scanning.");

                IList<TSqlStatement> statements = script.Batches[0].Statements;
                Assert.AreEqual(1, statements.Count, "Exactly one statement should be produced.");

                CreateProcedureStatement statement = (CreateProcedureStatement)statements[0];
                Assert.AreEqual(body.Length, statement.StartOffset + statement.FragmentLength,
                    "The statement must cover the whole input, including everything after U+FFFF.");
            }, new TSql160Parser(true), new TSql170Parser(true), new TSql180Parser(true));
        }

        /// <summary>
        /// U+FFFF inside the bodies of CREATE VIEW / FUNCTION / TRIGGER must not stop
        /// scanning; each produces a single statement covering the whole input.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void LexerHandlesUFFFFInViewFunctionTriggerBodies()
        {
            string literal = "N'hi" + UFFFE + UFFFF + "lo'";
            string view = "CREATE VIEW dbo.v AS SELECT " + literal + " AS c";
            string function =
                "CREATE FUNCTION dbo.f() RETURNS NVARCHAR(50) AS BEGIN RETURN " + literal + "; END";
            string trigger =
                "CREATE TRIGGER dbo.tr ON dbo.t AFTER INSERT AS SELECT " + literal + " AS c";

            ParserTestUtils.ExecuteTestForParsers(parser =>
            {
                foreach (string body in new[] { view, function, trigger })
                {
                    TSqlScript script = ParseScript(parser, body, out IList<ParseError> errors);

                    ParserTestUtils.LogErrors(errors, body);
                    Assert.AreEqual(0, errors.Count, "U+FFFF must not terminate scanning for: " + body);

                    IList<TSqlStatement> statements = script.Batches[0].Statements;
                    Assert.AreEqual(1, statements.Count, "Exactly one statement should be produced for: " + body);
                    Assert.AreEqual(body.Length,
                        statements[0].StartOffset + statements[0].FragmentLength,
                        "The statement must span the whole input for: " + body);
                }
            }, new TSql160Parser(true), new TSql170Parser(true), new TSql180Parser(true));
        }

        /// <summary>
        /// A delimited identifier containing U+FFFF must parse and preserve the character.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void LexerHandlesUFFFFInDelimitedIdentifier()
        {
            string source = "SELECT 1 AS [col" + UFFFF + "];";

            ParserTestUtils.ExecuteTestForAllParsers(parser =>
            {
                TSqlScript script = ParseScript(parser, source, out IList<ParseError> errors);

                ParserTestUtils.LogErrors(errors, source);
                Assert.AreEqual(0, errors.Count, "U+FFFF inside a delimited identifier must not terminate scanning.");

                SelectStatement select = (SelectStatement)script.Batches[0].Statements[0];
                QuerySpecification query = (QuerySpecification)select.QueryExpression;
                SelectScalarExpression element = (SelectScalarExpression)query.SelectElements[0];

                Assert.AreEqual("col" + UFFFF, element.ColumnName.Identifier.Value,
                    "The U+FFFF must round-trip inside the delimited identifier.");
            }, true);
        }

        /// <summary>
        /// U+FFFF inside a block comment and a line comment must not terminate scanning;
        /// the following SELECT must still be parsed.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void LexerHandlesUFFFFInComments()
        {
            string blockComment = "/* c" + UFFFF + " */ SELECT 1 AS c;";
            string lineComment = "-- c" + UFFFF + "\r\nSELECT 1 AS c;";

            ParserTestUtils.ExecuteTestForAllParsers(parser =>
            {
                foreach (string source in new[] { blockComment, lineComment })
                {
                    TSqlScript script = ParseScript(parser, source, out IList<ParseError> errors);

                    ParserTestUtils.LogErrors(errors, source);
                    Assert.AreEqual(0, errors.Count, "U+FFFF inside a comment must not terminate scanning for: " + source);

                    IList<TSqlStatement> statements = script.Batches[0].Statements;
                    Assert.AreEqual(1, statements.Count,
                        "The SELECT after the comment must still be parsed for: " + source);
                    Assert.IsInstanceOfType(statements[0], typeof(SelectStatement));
                }
            }, true);
        }

        /// <summary>
        /// Sanity guard: a string literal that genuinely runs to end-of-stream must still
        /// report the unterminated-literal error. The fix must not mask real EOF errors.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void LexerStillReportsUnterminatedLiteralAtEndOfStream()
        {
            string source = "SELECT N'abc";

            ParserTestUtils.ExecuteTestForAllParsers(parser =>
            {
                ParseScript(parser, source, out IList<ParseError> errors);
                Assert.AreEqual(1, errors.Count,
                    "A genuinely unterminated literal at end-of-stream must still produce an error.");
            }, true);
        }

        /// <summary>
        /// The integer lookahead contract must retain every char value while keeping
        /// end-of-input outside that range, without changing ordinary case folding.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void LexerIntegerLookaheadPreservesCharacterAndSentinelValues()
        {
            antlr.CharBuffer inputBuffer = new antlr.CharBuffer(new StringReader(UFFFF));

            Assert.AreEqual((int)char.MaxValue, inputBuffer.LA(1));
            Assert.AreEqual(antlr.CharScanner.EOF_CHAR, inputBuffer.LA(2));

            DiagnosticCharScanner scanner = new DiagnosticCharScanner();
            Assert.AreEqual((int)'a', scanner.toLower('A'));
            Assert.AreEqual(antlr.CharScanner.EOF_CHAR, scanner.toLower(antlr.CharScanner.EOF_CHAR));
            Assert.AreEqual(-1, scanner.toLower(-1));
        }

        /// <summary>
        /// Lexer exceptions must retain the out-of-range EOF sentinel so diagnostics do
        /// not confuse end-of-input with a real U+0000 or U+FFFF character.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void LexerExceptionDiagnosticsPreserveEndOfInput()
        {
            antlr.NoViableAltForCharException eofAlternative = new antlr.NoViableAltForCharException(
                antlr.CharScanner.EOF_CHAR, string.Empty, 1, 1);
            antlr.NoViableAltForCharException nullAlternative =
                new antlr.NoViableAltForCharException('\0', string.Empty, 1, 1);
            antlr.NoViableAltForCharException uffffAlternative =
                new antlr.NoViableAltForCharException(char.MaxValue, string.Empty, 1, 1);

            Assert.AreEqual(antlr.CharScanner.EOF_CHAR, eofAlternative.foundChar);
            Assert.AreEqual("unexpected char: <EOF>", eofAlternative.Message);
            Assert.AreEqual("unexpected char: 0x0", nullAlternative.Message);
            Assert.AreEqual("unexpected char: 0xFFFF", uffffAlternative.Message);

            antlr.TokenStreamRecognitionException eofTokenStreamException =
                new antlr.TokenStreamRecognitionException(eofAlternative);
            ParseError eofParseError = TSql80ParserBaseInternal.ProcessTokenStreamRecognitionException(
                eofTokenStreamException, 0);
            antlr.NoViableAltForCharException printableAlternative =
                new antlr.NoViableAltForCharException('@', string.Empty, 1, 1);
            ParseError printableParseError = TSql80ParserBaseInternal.ProcessTokenStreamRecognitionException(
                new antlr.TokenStreamRecognitionException(printableAlternative), 0);

            Assert.AreEqual("Incorrect syntax near '<EOF>'.", eofParseError.Message);
            Assert.AreEqual("Incorrect syntax near '@'.", printableParseError.Message);

            DiagnosticCharScanner scanner = new DiagnosticCharScanner();
            antlr.MismatchedCharException eofMismatch = new antlr.MismatchedCharException(
                antlr.CharScanner.EOF_CHAR, 'x', false, scanner);
            antlr.MismatchedCharException uffffMismatch = new antlr.MismatchedCharException(
                char.MaxValue, 'x', false, scanner);

            Assert.AreEqual(antlr.CharScanner.EOF_CHAR, eofMismatch.foundChar);
            StringAssert.Contains(eofMismatch.Message, "'<EOF>'");
            Assert.AreEqual(char.MaxValue, uffffMismatch.foundChar);
            Assert.IsFalse(uffffMismatch.Message.Contains("<EOF>"));
        }

        /// <summary>
        /// Generator round-trip: parse -> SqlScriptGenerator -> string must preserve U+FFFF,
        /// and the generated script must re-parse with zero errors.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void GeneratorRoundTripsUFFFFInStringLiteral()
        {
            string body =
                "CREATE PROCEDURE dbo.p AS BEGIN " +
                "DECLARE @s NVARCHAR(50) = N'hi" + UFFFE + UFFFF + "lo'; " +
                "SELECT @s; END";

            RoundTrip(new TSql160Parser(true), SqlVersion.Sql160, body);
            RoundTrip(new TSql170Parser(true), SqlVersion.Sql170, body);
            RoundTrip(new TSql180Parser(true), SqlVersion.Sql180, body);
        }

        private static void RoundTrip(TSqlParser parser, SqlVersion version, string body)
        {
            TSqlScript script = ParseScript(parser, body, out IList<ParseError> errors);
            ParserTestUtils.LogErrors(errors, body);
            Assert.AreEqual(0, errors.Count, "Source must parse cleanly before generation.");

            SqlScriptGenerator generator = ParserTestUtils.CreateScriptGen(version);
            StringWriter writer = new StringWriter();
            generator.GenerateScript(script, writer);
            string generated = writer.ToString();

            Assert.IsTrue(generated.Contains(UFFFF), "Generated script must preserve U+FFFF.");
            Assert.IsTrue(generated.Contains(UFFFE), "Generated script must preserve U+FFFE.");

            ParseScript(parser, generated, out IList<ParseError> reparseErrors);
            ParserTestUtils.LogErrors(reparseErrors, generated);
            Assert.AreEqual(0, reparseErrors.Count, "Generated script must re-parse with zero errors.");
        }

        private sealed class DiagnosticCharScanner : antlr.CharScanner
        {
            public DiagnosticCharScanner()
                : base(new antlr.CharBuffer(new StringReader(string.Empty)))
            {
            }
        }
    }
}
