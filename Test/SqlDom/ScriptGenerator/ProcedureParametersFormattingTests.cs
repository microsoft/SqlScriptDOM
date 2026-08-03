//------------------------------------------------------------------------------
// <copyright file="ProcedureParametersFormattingTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;
using System.Collections.Generic;
using System.IO;
using static SqlStudio.Tests.UTSqlScriptDom.ScriptGeneratorTestHelper;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    // Tests for the MultilineProcedureParametersList script-generation option, which controls
    // whether CREATE/ALTER PROCEDURE and CREATE/ALTER FUNCTION parameters are written one per line
    // (true) or collapsed onto a single line (false, the default). The default preserves the
    // existing single-line behavior; multi-line output is opt-in. Procedure parameters are not
    // wrapped in parentheses; function parameters are.
    //
    // Work item: Formatter option: Procedure/function parameters across multiple lines
    [TestClass]
    public class ProcedureParametersFormattingTests
    {
        // Options that opt in to the multi-line parameter layout, leaving everything else at default.
        private static SqlScriptGeneratorOptions Multiline()
        {
            return new SqlScriptGeneratorOptions { MultilineProcedureParametersList = true };
        }

        // Generates a script for CREATE/ALTER EXTERNAL FUNCTION syntax, which is parsed and emitted
        // by the Fabric DW parser/generator. Asserts the input parses and the output reparses.
        private static string GenerateFabricDW(string sql, SqlScriptGeneratorOptions options)
        {
            var parser = new TSqlFabricDWParser(true);
            TSqlFragment fragment = parser.Parse(new StringReader(sql), out IList<ParseError> errors);
            Assert.AreEqual(0, errors.Count, "Input must parse without errors.");

            var generator = new SqlFabricDWScriptGenerator(options);
            generator.GenerateScript(fragment, out string generated);

            var reparser = new TSqlFabricDWParser(true);
            reparser.Parse(new StringReader(generated), out IList<ParseError> reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated script must reparse without errors. Actual:\n" + generated);
            return generated;
        }

        // -----------------------------------------------------------------------------------------
        // Default: the option is off, so existing single-line behavior is preserved.
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineProcedureParametersListDefaultIsFalse()
        {
            Assert.AreEqual(false, new SqlScriptGeneratorOptions().MultilineProcedureParametersList);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestProcedureDefaultKeepsParametersOnSingleLine()
        {
            // Default options must reproduce the previous behavior: all parameters on one line.
            const string input = "CREATE PROCEDURE p1 @a INT, @b INT AS SELECT 1;";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
CREATE PROCEDURE p1
@a INT, @b INT
AS
SELECT 1;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestFunctionDefaultKeepsParametersOnSingleLine()
        {
            // Default options must reproduce the previous behavior: parameters on one parenthesized line.
            const string input = "CREATE FUNCTION dbo.f (@a INT, @b INT) RETURNS INT AS BEGIN RETURN 1; END";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
CREATE FUNCTION dbo.f
(@a INT, @b INT)
RETURNS INT
AS
BEGIN
    RETURN 1;
END";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestProcedureNoParametersUnaffected()
        {
            const string input = "CREATE PROCEDURE p1 AS SELECT 1;";
            var options = Multiline();
            const string expected =
@"
CREATE PROCEDURE p1
AS
SELECT 1;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestFunctionNoParametersUnaffected()
        {
            const string input = "CREATE FUNCTION dbo.f () RETURNS INT AS BEGIN RETURN 1; END";
            var options = Multiline();
            const string expected =
@"
CREATE FUNCTION dbo.f
( )
RETURNS INT
AS
BEGIN
    RETURN 1;
END";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // CREATE PROCEDURE with the option enabled (parameters are not parenthesized)
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestProcedureMultilineWhenEnabled()
        {
            const string input = "CREATE PROCEDURE p1 @a INT, @b INT AS SELECT 1;";
            var options = Multiline();
            const string expected =
@"
CREATE PROCEDURE p1
    @a INT,
    @b INT
AS
SELECT 1;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestProcedureSingleParameterMultilineWhenEnabled()
        {
            const string input = "CREATE PROCEDURE p1 @a INT AS SELECT 1;";
            var options = Multiline();
            const string expected =
@"
CREATE PROCEDURE p1
    @a INT
AS
SELECT 1;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestProcedureMultilineLeadingComma()
        {
            const string input = "CREATE PROCEDURE p1 @a INT, @b INT AS SELECT 1;";
            var options = Multiline();
            options.CommaPlacement = CommaPlacement.Leading;
            const string expected =
@"
CREATE PROCEDURE p1
    @a INT
  , @b INT
AS
SELECT 1;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCreateOrAlterProcedureMultilineWhenEnabled()
        {
            const string input = "CREATE OR ALTER PROCEDURE p1 @a INT, @b INT AS SELECT 1;";
            var options = Multiline();
            const string expected =
@"
CREATE OR ALTER PROCEDURE p1
    @a INT,
    @b INT
AS
SELECT 1;";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // CREATE FUNCTION with the option enabled (parameters are parenthesized)
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestFunctionMultilineWhenEnabled()
        {
            const string input = "CREATE FUNCTION dbo.f (@a INT, @b INT) RETURNS INT AS BEGIN RETURN 1; END";
            var options = Multiline();
            const string expected =
@"
CREATE FUNCTION dbo.f (
    @a INT,
    @b INT
)
RETURNS INT
AS
BEGIN
    RETURN 1;
END";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestFunctionMultilineLeadingComma()
        {
            const string input = "CREATE FUNCTION dbo.f (@a INT, @b INT) RETURNS INT AS BEGIN RETURN 1; END";
            var options = Multiline();
            options.CommaPlacement = CommaPlacement.Leading;
            const string expected =
@"
CREATE FUNCTION dbo.f (
    @a INT
  , @b INT
)
RETURNS INT
AS
BEGIN
    RETURN 1;
END";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlterFunctionMultilineWhenEnabled()
        {
            const string input = "ALTER FUNCTION dbo.f (@a INT, @b INT) RETURNS INT AS BEGIN RETURN 1; END";
            var options = Multiline();
            const string expected =
@"
ALTER FUNCTION dbo.f (
    @a INT,
    @b INT
)
RETURNS INT
AS
BEGIN
    RETURN 1;
END";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // AlignColumnDefinitionFields must not affect procedure/function parameters
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlignColumnDefinitionFieldsDoesNotAffectParameters()
        {
            const string input = "CREATE PROCEDURE p1 @a INT, @bbbbb NVARCHAR (20) AS SELECT 1;";
            var options = Multiline();
            options.AlignColumnDefinitionFields = true;
            const string expected =
@"
CREATE PROCEDURE p1
    @a INT,
    @bbbbb NVARCHAR (20)
AS
SELECT 1;";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Parameter modifiers and parenthesis-placement interactions (multi-line enabled)
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestProcedureMultilinePreservesParameterModifiers()
        {
            // Each per-line parameter must retain its full modifiers (default value, OUTPUT, cursor).
            const string input = "CREATE PROCEDURE p1 @a INT = 5 OUTPUT, @c CURSOR VARYING OUTPUT AS SELECT 1;";
            var options = Multiline();
            const string expected =
@"
CREATE PROCEDURE p1
    @a INT=5 OUTPUT,
    @c CURSOR VARYING OUTPUT
AS
SELECT 1;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestFunctionMultilineNewLineBeforeOpenParenthesis()
        {
            // NewLineBeforeOpenParenthesisInMultilineList moves the '(' onto its own line for the
            // parenthesized (function) parameter list.
            const string input = "CREATE FUNCTION dbo.f (@a INT, @b INT) RETURNS INT AS BEGIN RETURN 1; END";
            var options = Multiline();
            options.NewLineBeforeOpenParenthesisInMultilineList = true;
            const string expected =
@"
CREATE FUNCTION dbo.f
(
    @a INT,
    @b INT
)
RETURNS INT
AS
BEGIN
    RETURN 1;
END";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestFunctionMultilineNoNewLineBeforeCloseParenthesis()
        {
            // NewLineBeforeCloseParenthesisInMultilineList = false keeps ')' on the last parameter's line.
            const string input = "CREATE FUNCTION dbo.f (@a INT, @b INT) RETURNS INT AS BEGIN RETURN 1; END";
            var options = Multiline();
            options.NewLineBeforeCloseParenthesisInMultilineList = false;
            const string expected =
@"
CREATE FUNCTION dbo.f (
    @a INT,
    @b INT)
RETURNS INT
AS
BEGIN
    RETURN 1;
END";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCreateOrAlterFunctionMultilineWhenEnabled()
        {
            const string input = "CREATE OR ALTER FUNCTION dbo.f (@a INT, @b INT) RETURNS INT AS BEGIN RETURN 1; END";
            var options = Multiline();
            const string expected =
@"
CREATE OR ALTER FUNCTION dbo.f (
    @a INT,
    @b INT
)
RETURNS INT
AS
BEGIN
    RETURN 1;
END";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // CREATE/ALTER EXTERNAL FUNCTION (Fabric DW; parameters are parenthesized)
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestExternalFunctionDefaultKeepsParametersOnSingleLine()
        {
            const string input = "CREATE FUNCTION dbo.fn (@x INT, @y NVARCHAR (50)) RETURNS INT AS EXTERNAL FUNCTION mySet.myFn;";
            var options = new SqlScriptGeneratorOptions();
            string generated = GenerateFabricDW(input, options);

            const string expected =
@"CREATE FUNCTION dbo.fn (@x INT, @y NVARCHAR (50)) RETURNS INT AS EXTERNAL FUNCTION mySet.myFn;";

            Assert.AreEqual(Normalize(expected).Trim(), Normalize(generated).Trim());
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestExternalFunctionMultilineWhenEnabled()
        {
            const string input = "CREATE FUNCTION dbo.fn (@x INT, @y NVARCHAR (50)) RETURNS INT AS EXTERNAL FUNCTION mySet.myFn;";
            var options = Multiline();
            string generated = GenerateFabricDW(input, options);

            const string expected =
@"CREATE FUNCTION dbo.fn (
    @x INT,
    @y NVARCHAR (50)
) RETURNS INT AS EXTERNAL FUNCTION mySet.myFn;";

            Assert.AreEqual(Normalize(expected).Trim(), Normalize(generated).Trim());
        }
    }
}
